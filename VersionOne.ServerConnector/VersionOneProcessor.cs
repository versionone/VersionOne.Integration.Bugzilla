using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using VersionOne.SDK.APIClient;
using VersionOne.ServerConnector.Entities;
using VersionOne.ServerConnector.Filters;
using VersionOne.ServiceHost.Core.Configuration;
using VersionOne.ServiceHost.Core.Logging;
using System.Xml;

namespace VersionOne.ServerConnector
{

    public class VersionOneProcessor : IVersionOneProcessor
    {
        public const string ScopeType = "Scope";
        public const string FeatureGroupType = "Theme";
        public const string StoryType = "Story";
        public const string DefectType = "Defect";
        public const string TaskType = "Task";
        public const string TestType = "Test";
        public const string ChangeSetType = "ChangeSet";
        public const string BuildProjectType = "BuildProject";
        public const string BuildRunType = "BuildRun";
        public const string PrimaryWorkitemType = "PrimaryWorkitem";
        public const string WorkitemType = "Workitem";
        public const string MemberType = "Member";
        public const string LinkType = "Link";
        public const string AttributeDefinitionType = "AttributeDefinition";

        public const string SystemAdminRoleName = "Role.Name'System Admin";
        public const string SystemAdminRoleId = "Role:1";

        public const string OwnersAttribute = "Owners";
        public const string AssetStateAttribute = "AssetState";
        public const string AssetTypeAttribute = "AssetType";

        public const string DeleteOperation = "Delete";
        public const string InactivateOperation = "Inactivate";
        public const string ReactivateOperation = "Reactivate";

        public const string WorkitemPriorityType = "WorkitemPriority";
        public const string WorkitemSourceType = "StorySource";
        public const string WorkitemStatusType = "StoryStatus";
        public const string BuildRunStatusType = "BuildStatus";

        private const string IdAttribute = "ID";
        private const string AssetAttribute = "Asset";

        private IServices services;
        private IMetaModel metaModel;
        private ILocalizer localizer;
        private readonly ILogger logger;
        private readonly XmlElement configuration;

        private IQueryBuilder queryBuilder;

        private IDictionary<string, PropertyValues> ListPropertyValues
        {
            get { return queryBuilder.ListPropertyValues; }
        }

        public VersionOneProcessor(VersionOneSettings settings, ILogger logger) : this(settings.ToXmlElement(), logger) { }

        public VersionOneProcessor(VersionOneSettings settings) : this(settings, null) { }

        public VersionOneProcessor(XmlElement config, ILogger logger)
        {
            configuration = config;
            this.logger = logger;

            queryBuilder = new QueryBuilder();
        }

        public VersionOneProcessor(XmlElement config) : this(config, null) { }

        private void Connect()
        {
            var connector = new V1Central(configuration);
            connector.Validate();
            services = connector.Services;
            metaModel = connector.MetaModel;
            localizer = connector.Loc;

            queryBuilder.Setup(services, metaModel, connector.Loc);
        }

        protected void Connect(IServices testServices, IMetaModel testMetaData, IQueryBuilder testQueryBuilder)
        {
            services = testServices;
            metaModel = testMetaData;
            queryBuilder = testQueryBuilder;
        }

        public bool ValidateConnection()
        {
            try
            {
                Connect();
            }
            catch (Exception ex)
            {
                logger.MaybeLog(LogMessage.SeverityType.Error, "Connection is not valid. " + ex.Message);
                return false;
            }

            return true;
        }

        public Member GetLoggedInMember()
        {
            return GetMembers(Filter.Empty()).FirstOrDefault(item => item.Asset.Oid.Equals(services.LoggedIn));
        }

        public ICollection<Member> GetMembers(IFilter filter)
        {
            return queryBuilder.Query(MemberType, filter).Select(item => new Member(item)).ToList();
        }

        // TODO make this Story-agnostic. In case of criteria based ex. on Story-only custom fields current filter approach won't let an easy solution.
        public IList<FeatureGroup> GetFeatureGroups(IFilter filter, IFilter childrenFilter)
        {
            var allMembers = GetMembers(Filter.Empty());

            return queryBuilder.Query(FeatureGroupType, filter)
                .Select(asset => new FeatureGroup(
                    asset, ListPropertyValues,
                    GetWorkitems(StoryType, GroupFilter.And(Filter.Equal(Entity.ParentAndUpProperty, asset.Oid.Momentless.Token), childrenFilter)),
                    ChooseOwners(asset, allMembers),
                    queryBuilder.TypeResolver))
                .ToList();
        }

        public void SaveEntities<T>(ICollection<T> entities) where T : BaseEntity
        {
            if (entities == null || entities.Count == 0)
            {
                return;
            }

            foreach (var entity in entities)
            {
                Save(entity);
            }
        }

        public void Save(BaseEntity entity)
        {
            try
            {
                services.Save(entity.Asset);
            }
            catch (V1Exception ex)
            {
                logger.MaybeLog(LogMessage.SeverityType.Error, string.Format(queryBuilder.Localize(GetMessageFromException(ex)) + " '{0}' ({1}).", entity.Asset.Oid.Token, entity.TypeToken));
            }
            catch (Exception ex)
            {
                logger.MaybeLog(LogMessage.SeverityType.Error, "Internal error: " + ex.Message);
            }
        }

        private static string GetMessageFromException(V1Exception exception)
        {
            var message = exception.Message;

            return message.Split(':')[0];
        }

        public void CloseWorkitem(PrimaryWorkitem workitem)
        {
            try
            {
                var closeOperation = workitem.Asset.AssetType.GetOperation(InactivateOperation);
                services.ExecuteOperation(closeOperation, workitem.Asset.Oid);
            }
            catch (V1Exception ex)
            {
                throw new VersionOneException(queryBuilder.Localize(ex.Message));
            }
            catch (Exception ex)
            {
                throw new VersionOneException(ex.Message);
            }
        }

        public IList<ValueId> GetWorkitemStatuses()
        {
            try
            {
                return queryBuilder.QueryPropertyValues(WorkitemStatusType).ToList();
            }
            catch (V1Exception ex)
            {
                throw new VersionOneException(queryBuilder.Localize(ex.Message));
            }
            catch (Exception ex)
            {
                throw new VersionOneException(ex.Message);
            }
        }

        public IList<ValueId> GetBuildRunStatuses()
        {
            try
            {
                return queryBuilder.QueryPropertyValues(BuildRunStatusType).ToList();
            }
            catch (V1Exception ex)
            {
                throw new VersionOneException(queryBuilder.Localize(ex.Message));
            }
            catch (Exception ex)
            {
                throw new VersionOneException(ex.Message);
            }
        }

        public ValueId CreateWorkitemStatus(string statusName)
        {
            try
            {
                var statusAsset = GetEntityFactory().Create(WorkitemStatusType, new[] {
                    AttributeValue.Single(Entity.NameProperty, statusName)
                });
                return new ValueId(statusAsset.Oid.Momentless, statusName);
            }
            catch (V1Exception ex)
            {
                throw new VersionOneException(queryBuilder.Localize(ex.Message));
            }
            catch (Exception ex)
            {
                throw new VersionOneException(ex.Message);
            }
        }

        public ValueId CreateWorkitemPriority(string priorityName)
        {
            try
            {
                var statusAsset = GetEntityFactory().Create(WorkitemPriorityType, new[] {
                    AttributeValue.Single(Entity.NameProperty, priorityName)
                });
                return new ValueId(statusAsset.Oid.Momentless, priorityName);
            }
            catch (V1Exception ex)
            {
                throw new VersionOneException(queryBuilder.Localize(ex.Message));
            }
            catch (Exception ex)
            {
                throw new VersionOneException(ex.Message);
            }
        }

        public void UpdateProject(string projectId, Link link)
        {
            try
            {
                if (link != null && !string.IsNullOrEmpty(link.Url))
                {
                    var projectAsset = GetProjectById(projectId);
                    AddLinkToAsset(projectAsset, link);
                }
            }
            catch (V1Exception ex)
            {
                throw new VersionOneException(queryBuilder.Localize(ex.Message));
            }
            catch (Exception ex)
            {
                throw new VersionOneException(ex.Message);
            }
        }

        public string GetWorkitemLink(Workitem workitem)
        {
            return string.Format("{0}assetdetail.v1?oid={1}", configuration["ApplicationUrl"].InnerText, workitem.Id);
        }

        public string GetSummaryLink(Workitem workitem)
        {
            return string.Format("{0}{1}.mvc/Summary?oidToken={2}", configuration["ApplicationUrl"].InnerText, workitem.TypeName.ToLower(), workitem.Id);
        }

        public PropertyValues GetAvailableListValues(string typeToken, string fieldName)
        {
            try
            {
                var type = metaModel.GetAssetType(typeToken);
                var attributeDefinition = type.GetAttributeDefinition(fieldName);

                if (attributeDefinition.AttributeType != AttributeType.Relation)
                {
                    throw new VersionOneException("Not a Relation field");
                }

                var listTypeToken = attributeDefinition.RelatedAsset.Token;
                return queryBuilder.QueryPropertyValues(listTypeToken);
            }
            catch (MetaException)
            {
                throw new VersionOneException("Invalid type or field name");
            }
        }

        public IList<ValueId> GetWorkitemPriorities()
        {
            try
            {
                return queryBuilder.QueryPropertyValues(WorkitemPriorityType).ToList();
            }
            catch (V1Exception ex)
            {
                throw new VersionOneException(queryBuilder.Localize(ex.Message));
            }
            catch (Exception ex)
            {
                throw new VersionOneException(ex.Message);
            }
        }

        // TODO get rid of it
        public bool ProjectExists(string projectId)
        {
            return GetProjectById(projectId) != null;
        }

        public bool AttributeExists(string typeName, string attributeName)
        {
            try
            {
                var type = metaModel.GetAssetType(typeName);
                var attributeDefinition = type.GetAttributeDefinition(attributeName);
                return attributeDefinition != null;
            }
            catch (MetaException)
            {
                return false;
            }
        }

        public void AddProperty(string attr, string prefix, bool isList)
        {
            queryBuilder.AddProperty(attr, prefix, isList);
        }

        public void AddListProperty(string fieldName, string typeToken)
        {
            queryBuilder.AddListProperty(fieldName, typeToken);
        }

        public void AddOptionalProperty(string attr, string prefix)
        {
            if (!string.IsNullOrEmpty(attr))
            {
                queryBuilder.AddOptionalProperty(attr, prefix);
            }
        }

        // TODO use filters
        private Asset GetProjectById(string projectId)
        {
            var scopeType = metaModel.GetAssetType(Workitem.ScopeProperty);
            var scopeState = scopeType.GetAttributeDefinition(AssetStateAttribute);

            var scopeStateTerm = new FilterTerm(scopeState);
            scopeStateTerm.NotEqual(AssetState.Closed);

            var query = new Query(Oid.FromToken(projectId, metaModel)) { Filter = scopeStateTerm };
            var result = services.Retrieve(query);

            return result.Assets.FirstOrDefault();
        }

        private List<Asset> GetAssetLinks(Oid assetOid, IFilter filter)
        {
            var fullFilter = GroupFilter.And(filter, Filter.Equal(AssetAttribute, assetOid.Momentless));
            return queryBuilder.Query(LinkType, fullFilter);
        }

        public List<Link> GetWorkitemLinks(Workitem workitem, IFilter filter)
        {
            return GetAssetLinks(Oid.FromToken(workitem.Id, metaModel), filter).Select(x => new Link(x)).ToList();
        }

        public void AddLinkToEntity(BaseEntity entity, Link link)
        {
            try
            {
                if (link != null && !string.IsNullOrEmpty(link.Url))
                {
                    AddLinkToAsset(entity.Asset, link);
                }
            }
            catch (V1Exception ex)
            {
                throw new VersionOneException(queryBuilder.Localize(ex.Message));
            }
            catch (Exception ex)
            {
                throw new VersionOneException(ex.Message);
            }
        }

        private void AddLinkToAsset(Asset asset, Link link)
        {
            if (asset == null)
            {
                return;
            }

            var linkType = metaModel.GetAssetType(LinkType);

            var existingLinks = GetAssetLinks(asset.Oid, Filter.Equal(Link.UrlProperty, link.Url));

            if (existingLinks.Count > 0)
            {
                logger.MaybeLog(LogMessage.SeverityType.Debug, string.Format("No need to create link - it already exists."));
                return;
            }

            logger.MaybeLog(LogMessage.SeverityType.Info, string.Format("Creating new link with title {0} for asset {1}", link.Title, asset.Oid));

            var linkAsset = services.New(linkType, asset.Oid.Momentless);
            linkAsset.SetAttributeValue(linkType.GetAttributeDefinition(Entity.NameProperty), link.Title);
            linkAsset.SetAttributeValue(linkType.GetAttributeDefinition(Link.OnMenuProperty), link.OnMenu);
            linkAsset.SetAttributeValue(linkType.GetAttributeDefinition(Link.UrlProperty), link.Url);

            services.Save(linkAsset);
            logger.MaybeLog(LogMessage.SeverityType.Info, string.Format("{0} link saved", link.Title));
        }

        public IList<BuildProject> GetBuildProjects(IFilter filter)
        {
            var buildProjectType = metaModel.GetAssetType(BuildProjectType);
            var terms = filter.GetFilter(buildProjectType);

            return queryBuilder.Query(BuildProjectType, terms).Select(asset => new BuildProject(asset)).ToList();
        }

        public IList<BuildRun> GetBuildRuns(IFilter filter)
        {
            var buildRunType = metaModel.GetAssetType(BuildRunType);
            var terms = filter.GetFilter(buildRunType);

            return queryBuilder.Query(BuildRunType, terms).Select(asset => new BuildRun(asset, queryBuilder.ListPropertyValues, queryBuilder.TypeResolver)).ToList();
        }

        public IList<ChangeSet> GetChangeSets(IFilter filter)
        {
            var changeSetType = metaModel.GetAssetType(ChangeSetType);
            var terms = filter.GetFilter(changeSetType);

            return queryBuilder.Query(ChangeSetType, terms).Select(asset => new ChangeSet(asset)).ToList();
        }

        public IList<PrimaryWorkitem> GetPrimaryWorkitems(IFilter filter, SortBy sortBy = null)
        {
            var workitemType = metaModel.GetAssetType(PrimaryWorkitemType);
            var terms = filter.GetFilter(workitemType);

            var allMembers = GetMembers(Filter.Empty());

            return queryBuilder.Query(PrimaryWorkitemType, terms)
                .Select(asset => PrimaryWorkitem.Create(asset, ListPropertyValues, queryBuilder.TypeResolver, ChooseOwners(asset, allMembers)))
                .ToList();
        }

        public IList<Workitem> GetWorkitems(string type, IFilter filter, SortBy sortBy = null)
        {
            var workitemType = metaModel.GetAssetType(type);
            var terms = filter.GetFilter(workitemType);

            var allMembers = GetMembers(Filter.Empty());

            return queryBuilder.Query(type, terms)
                .Select(asset => Workitem.Create(asset, ListPropertyValues, queryBuilder.TypeResolver, ChooseOwners(asset, allMembers)))
                .ToList();
        }

        // TODO see if this method should really fail as it does now if Owners attribute is unavailable
        private static IList<Member> ChooseOwners(Asset asset, IEnumerable<Member> allMembers)
        {
            var ownersDef = asset.AssetType.GetAttributeDefinition(OwnersAttribute);
            var ownersAttribute = asset.GetAttribute(ownersDef);

            if (ownersDef == null)
            {
                throw new V1Exception("Cannot set Owners of workitem, corresponding attribute not enlisted for asset type " + asset.AssetType.Token);
            }

            var owners = asset.GetAttribute(ownersDef).Values.Cast<Oid>().ToList();
            return allMembers.Where(x => owners.Contains(x.Asset.Oid)).ToList();
        }

        //TODO refactor
        public Workitem CreateWorkitem(string assetType, string title, string description, string projectToken,
                                       string externalFieldName, string externalId, string externalSystemName,
                                       string priorityId, string owners)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException("Empty title");
            }

            var projectOid = Oid.FromToken(projectToken, metaModel);
            var source = GetSourceByName(externalSystemName);
            var sourceOid = source.Oid.Momentless;

            var attributeValues = new List<AttributeValue> {
                AttributeValue.Single(Entity.NameProperty, title),
                AttributeValue.Single(ScopeType, projectOid),
                AttributeValue.Single(Entity.DescriptionProperty, description),
                AttributeValue.Single("Source", sourceOid),
                AttributeValue.Single(externalFieldName, externalId),
                AttributeValue.Multi("Owners", GetOwnerOids(owners).ToArray())
            };

            if (!string.IsNullOrEmpty(priorityId))
            {
                attributeValues.Add(AttributeValue.Single("Priority", Oid.FromToken(priorityId, metaModel)));
            }

            var workitemAsset = GetEntityFactory().Create(assetType, attributeValues);

            //TODO refactor
            //NOTE Save doesn't return all the needed data, therefore we need another query
            return GetWorkitems(workitemAsset.AssetType.Token, Filter.Equal("ID", workitemAsset.Oid.Momentless.Token)).FirstOrDefault();
        }

        public BuildRun CreateBuildRun(BuildProject buildProject, string name, DateTime date, double elapsed)
        {
            var asset = GetEntityFactory().Create(BuildRunType, new[] {
                AttributeValue.Single(BuildRun.BuildProjectProperty, buildProject.Asset.Oid.Momentless),
                AttributeValue.Single(Entity.NameProperty, name),
                AttributeValue.Single(BuildRun.DateProperty, date),
                AttributeValue.Single(BuildRun.ElapsedProperty, elapsed)
            });
            return new BuildRun(asset, ListPropertyValues, queryBuilder.TypeResolver);
        }

        public ChangeSet CreateChangeSet(string name, string reference, string description)
        {
            var asset = GetEntityFactory().Create(ChangeSetType, new[] {
                AttributeValue.Single(Entity.NameProperty, name),
                AttributeValue.Single(BaseEntity.ReferenceProperty, reference),
                AttributeValue.Single(Entity.DescriptionProperty, description)
            });
            return new ChangeSet(asset);
        }

        private ValueId GetSourceByName(string externalSystemName)
        {
            var sourceValues = queryBuilder.QueryPropertyValues(WorkitemSourceType);
            var source = sourceValues.FirstOrDefault(item => string.Equals(item.Name, externalSystemName));

            if (source == null)
            {
                throw new ArgumentException("Can't find proper source");
            }

            return source;
        }

        public string GetProjectTokenByName(string projectName)
        {
            var project = GetProjectByName(projectName);
            return project != null ? project.Oid.Momentless.Token : null;
        }

        private Asset GetProjectByName(string projectName)
        {
            var scopeType = metaModel.GetAssetType(Workitem.ScopeProperty);
            var scopeName = scopeType.GetAttributeDefinition(Entity.NameProperty);

            var filter = GroupFilter.And(
                Filter.Equal(Entity.NameProperty, projectName),
                Filter.Closed(false)
            );

            var query = new Query(scopeType);
            query.Selection.Add(scopeName);

            var result = queryBuilder.Query(Workitem.ScopeProperty, filter);

            return result.FirstOrDefault();
        }

        public string GetRootProjectToken()
        {
            var project = GetRootProject();
            return project == null ? null : project.Oid.Momentless.Token;
        }

        //TODO refactor
        private Asset GetRootProject()
        {
            var scopeType = metaModel.GetAssetType(Workitem.ScopeProperty);
            var scopeName = scopeType.GetAttributeDefinition(Entity.NameProperty);

            var scopeState = scopeType.GetAttributeDefinition(AssetStateAttribute);
            var scopeStateTerm = new FilterTerm(scopeState);
            scopeStateTerm.NotEqual(AssetState.Closed);

            var scopeQuery = new Query(scopeType, scopeType.GetAttributeDefinition(Entity.ParentProperty)) { Filter = scopeStateTerm };
            scopeQuery.Selection.Add(scopeName);

            var nameQueryResult = services.Retrieve(scopeQuery);

            return nameQueryResult.Assets.FirstOrDefault();
        }

        /// <summary>
        /// Attempts to match owners of the workitem in the external system to users in VersionOne.
        /// </summary>
        /// <param name="ownerNames">Comma seperated list of usernames.</param>
        /// <returns>Oids of matching users in VersionOne.</returns>
        //TODO refactor
        private IEnumerable<Oid> GetOwnerOids(string ownerNames)
        {
            var result = new List<Oid>();

            if (!string.IsNullOrEmpty(ownerNames))
            {
                var memberType = metaModel.GetAssetType("Member");
                var ownerQuery = new Query(memberType);

                var terms = new List<IFilterTerm>();

                foreach (var ownerName in ownerNames.Split(','))
                {
                    var term = new FilterTerm(memberType.GetAttributeDefinition("Username"));
                    term.Equal(ownerName);
                    terms.Add(term);
                }

                ownerQuery.Filter = new AndFilterTerm(terms.ToArray());

                var matches = services.Retrieve(ownerQuery).Assets;
                result.AddRange(matches.Select(owner => owner.Oid));
            }

            return result.ToArray();
        }

        public ICollection<Scope> LookupProjects(string term)
        {
            var projectType = metaModel.GetAssetType(ScopeType);
            var parentDef = projectType.GetAttributeDefinition("Parent");
            var nameDef = projectType.GetAttributeDefinition(Entity.NameProperty);
            var stateDef = projectType.GetAttributeDefinition(AssetStateAttribute);

            var filter = new FilterTerm(stateDef);
            filter.NotEqual(AssetState.Closed);

            var query = new Query(projectType, parentDef) { Filter = filter };
            query.Selection.Add(nameDef);
            query.OrderBy.MajorSort(projectType.DefaultOrderBy, OrderBy.Order.Ascending);

            var assets = services.Retrieve(query).Assets.Flatten();

            var selectAll = string.IsNullOrEmpty(term.Trim());
            return assets
                .Where(x => selectAll || x.GetAttribute(nameDef).Value.ToString().ToLowerInvariant().Contains(term.ToLowerInvariant()))
                .Select(asset => new Scope(asset))
                .ToList();
        }

        private EntityFactory GetEntityFactory()
        {
            return new EntityFactory(metaModel, services, queryBuilder.AttributesToQuery);
        }

        public Scope CreateProject(string name)
        {
            var rootProjectOid = GetRootProjectToken();
            var newProject = GetEntityFactory().Create(ScopeType,
                                                       new[] {
                                                           AttributeValue.Single(Entity.NameProperty, name),
                                                           AttributeValue.Single(Entity.ParentProperty, rootProjectOid),
                                                           AttributeValue.Single("BeginDate", DateTime.Now)
                                                       });
            return new Scope(newProject);
        }

        public IList<ListValue> GetCustomTextFields(string typeName)
        {
            try
            {
                var fields = GetCustomFields(typeName, FieldType.Text);
                return fields.Select(x => new ListValue(ConvertFromCamelCase(x), x)).ToList();
            }
            catch (Exception ex)
            {
                throw new VersionOneException("Failed to get custom list fields: " + ex.Message);
            }
        }

        private IEnumerable<string> GetCustomFields(string assetTypeName, FieldType fieldType)
        {
            var attrType = metaModel.GetAssetType(AttributeDefinitionType);
            var assetType = metaModel.GetAssetType(assetTypeName);
            var isCustomAttributeDef = attrType.GetAttributeDefinition("IsCustom");
            var nameAttrDef = attrType.GetAttributeDefinition(Entity.NameProperty);

            var termType = new FilterTerm(attrType.GetAttributeDefinition("Asset.AssetTypesMeAndDown.Name"));
            termType.Equal(assetTypeName);

            IAttributeDefinition inactiveDef;
            FilterTerm termState = null;

            if (assetType.TryGetAttributeDefinition("Inactive", out inactiveDef))
            {
                termState = new FilterTerm(inactiveDef);
                termState.Equal("False");
            }

            var fieldTypeName = string.Empty;
            var attributeTypeName = string.Empty;

            switch (fieldType)
            {
                case FieldType.List:
                    fieldTypeName = "OneToManyRelationDefinition";
                    attributeTypeName = "Relation";
                    break;
                case FieldType.Numeric:
                    fieldTypeName = "SimpleAttributeDefinition";
                    attributeTypeName = "Numeric";
                    break;
                case FieldType.Text:
                    fieldTypeName = "SimpleAttributeDefinition";
                    attributeTypeName = "Text";
                    break;
            }

            var assetTypeTerm = new FilterTerm(attrType.GetAttributeDefinition(AssetTypeAttribute));
            assetTypeTerm.Equal(fieldTypeName);

            var attributeTypeTerm = new FilterTerm(attrType.GetAttributeDefinition("AttributeType"));
            attributeTypeTerm.Equal(attributeTypeName);

            var isCustomTerm = new FilterTerm(isCustomAttributeDef);
            isCustomTerm.Equal("true");

            var result = GetFieldList(new AndFilterTerm(termState, termType, assetTypeTerm, isCustomTerm, attributeTypeTerm),
                new List<IAttributeDefinition> { nameAttrDef });

            var fieldList = new List<string>();
            result.ForEach(x => fieldList.Add(x.GetAttribute(nameAttrDef).Value.ToString()));

            return fieldList;
        }

        private AssetList GetFieldList(IFilterTerm filter, IEnumerable<IAttributeDefinition> selection)
        {
            var attributeDefinitionAssetType = metaModel.GetAssetType(AttributeDefinitionType);

            var query = new Query(attributeDefinitionAssetType);
            foreach (var attribute in selection)
            {
                query.Selection.Add(attribute);
            }

            query.Filter = filter;
            return services.Retrieve(query).Assets;
        }

        private static string ConvertFromCamelCase(string camelCasedString)
        {
            const string customPrefix = "Custom_";

            if (camelCasedString.StartsWith(customPrefix))
            {
                camelCasedString = camelCasedString.Remove(0, customPrefix.Length);
            }

            return Regex.Replace(camelCasedString,
                @"(?<a>(?<!^)((?:[A-Z][a-z])|(?:(?<!^[A-Z]+)[A-Z0-9]+(?:(?=[A-Z][a-z])|$))|(?:[0-9]+)))", @" ${a}");
        }

        public void LogConnectionConfiguration()
        {
            logger.LogVersionOneConfiguration(LogMessage.SeverityType.Info, configuration);
        }

        public void LogConnectionInformation()
        {
            try
            {
                var metaVersion = ((MetaModel)metaModel).Version;
                var loggedInMember = GetLoggedInMember();
                var defaultRole = localizer.Resolve(loggedInMember.DefaultRole);
                logger.LogVersionOneConnectionInformation(LogMessage.SeverityType.Info, metaVersion.ToString(), services.LoggedIn.ToString(), defaultRole);
            }
            catch (Exception ex)
            {
                logger.Log(LogMessage.SeverityType.Warning, "Failed to log VersionOne connection information.", ex);
            }
        }
    }
}