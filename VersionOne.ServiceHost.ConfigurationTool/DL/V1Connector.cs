using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VersionOne.SDK.APIClient;
using VersionOne.ServerConnector.Entities;
using VersionOne.ServiceHost.ConfigurationTool.DL.Exceptions;
using VersionOne.ServiceHost.ConfigurationTool.Entities;
using VersionOne.ServiceHost.Core.Configuration;
using ListValue = VersionOne.ServiceHost.ConfigurationTool.BZ.ListValue;
using VersionOneSettings = VersionOne.ServiceHost.ConfigurationTool.Entities.VersionOneSettings;

namespace VersionOne.ServiceHost.ConfigurationTool.DL {
    /// <summary>
    ///  VersionOne interaction handler.
    /// </summary>
    public class V1Connector {
        public const string TestTypeToken = "Test";
        public const string DefectTypeToken = "Defect";
        public const string FeatureGroupTypeToken = "Theme";
        public const string StoryTypeToken = "Story";
        public const string PrimaryWorkitemTypeToken = "PrimaryWorkitem";

        private const string TestStatusTypeToken = "TestStatus";
        private const string StoryStatusTypeToken = "StoryStatus";
        private const string WorkitemPriorityToken = "WorkitemPriority";
        private const string ProjectTypeToken = "Scope";

        private IServices services;

        public bool IsConnected { get; private set; }

        private static V1Connector instance;
        public static V1Connector Instance {
            get { return instance ?? (instance = new V1Connector()); }
        }

        private V1Connector() { }

        /// <summary>
        /// Validate V1 connection
        /// </summary>
        /// <param name="settings">settings for connection to VersionOne.</param>
        /// <returns>true, if validation succeeds; false, otherwise.</returns>
        public bool ValidateConnection(VersionOneSettings settings) {
            Connect(settings);
            return IsConnected;
        }

        /// <summary>
        /// Create connection to V1 server.
        /// </summary>
        /// <param name="settings">Connection settings</param>
        public void Connect(VersionOneSettings settings) {
            var url = settings.ApplicationUrl;
            var accessToken = settings.AccessToken;
            
            try
            {

                var connector = SDK.APIClient.V1Connector
                    .WithInstanceUrl(url)
                    .WithUserAgentHeader("VersionOne.Integration.Bugzilla", Assembly.GetEntryAssembly().GetName().Version.ToString());

                ICanSetProxyOrEndpointOrGetConnector connectorWithAuth;

                switch (settings.AuthenticationType)
                {
                    case AuthenticationTypes.AccessToken:
                        connectorWithAuth = connector.WithAccessToken(accessToken);
                        break;
                    default:
                        throw new Exception("Invalid authentication type");
                }
                if (settings.ProxySettings.Enabled)
                {
                    connectorWithAuth.WithProxy(GetProxy(settings.ProxySettings));
                }

                services = new Services(connectorWithAuth.Build());

                if (!services.LoggedIn.IsNull)
                {
                    IsConnected = true;
                    ListPropertyValues = new Dictionary<string, IList<ListValue>>();
                }
                else
                    IsConnected = false;

            }
            catch (Exception)
            {
                IsConnected = false;
            }
        }

        private ProxyProvider GetProxy(ProxyConnectionSettings proxySettings) {            
            if (proxySettings == null || !proxySettings.Enabled) {
                return null;
            }
            
            var uri = new Uri(proxySettings.Uri);
            return new ProxyProvider(uri,proxySettings.UserName, proxySettings.Password, proxySettings.Domain);
        }

        /// <summary>
        /// Reset connection
        /// </summary>
        public void ResetConnection() {
            IsConnected = false;
        }

        // TODO it is known that primary workitem statuses do not have to be unique in VersionOne. In this case, the following method fails.
        private IDictionary<string, string> QueryPropertyValues(string propertyName) {
            var res = new Dictionary<string, string>();
            var assetType = services.Meta.GetAssetType(propertyName);
            var valueDef = assetType.GetAttributeDefinition("Name");
            IAttributeDefinition inactiveDef;

            var query = new Query(assetType);
            query.Selection.Add(valueDef);
            
            if(assetType.TryGetAttributeDefinition("Inactive", out inactiveDef)) {
                var filter = new FilterTerm(inactiveDef);
                filter.Equal("False");
                query.Filter = filter;
            }

            query.OrderBy.MajorSort(assetType.DefaultOrderBy, OrderBy.Order.Ascending);

            foreach(var asset in services.Retrieve(query).Assets) {
                var name = asset.GetAttribute(valueDef).Value.ToString();
                res.Add(name, asset.Oid.ToString());
            }

            return res;
        }

        /// <summary>
        /// Get available test statuses.
        /// </summary>
        public IDictionary<string, string> GetTestStatuses() {
            return QueryPropertyValues(TestStatusTypeToken);
        }

        /// <summary>
        /// Get primary backlog item statuses.
        /// </summary>
        public IDictionary<string, string> GetStoryStatuses() {
            return QueryPropertyValues(StoryStatusTypeToken);
        }

        /// <summary>
        /// Get available workitem priorities.
        /// </summary>
        public IDictionary<string, string> GetWorkitemPriorities() {
            return QueryPropertyValues(WorkitemPriorityToken);
        }

        /// <summary>
        /// Get collection of reference fields for asset type.
        /// </summary>
        /// <param name="assetTypeToken">AssetType token</param>
        public List<string> GetReferenceFieldList(string assetTypeToken) {
            var attributeDefinitionAssetType = services.Meta.GetAssetType("AttributeDefinition");

            var nameAttributeDef = attributeDefinitionAssetType.GetAttributeDefinition("Name");
            var assetNameAttributeDef = attributeDefinitionAssetType.GetAttributeDefinition("Asset.AssetTypesMeAndDown.Name");
            var isCustomAttributeDef = attributeDefinitionAssetType.GetAttributeDefinition("IsCustom");
            var attributeTypeAttributeDef = attributeDefinitionAssetType.GetAttributeDefinition("AttributeType");

            var assetTypeTerm = new FilterTerm(assetNameAttributeDef);
            assetTypeTerm.Equal(assetTypeToken);
            
            var isCustomTerm = new FilterTerm(isCustomAttributeDef);
            isCustomTerm.Equal("true");
            
            var attributeTypeTerm = new FilterTerm(attributeTypeAttributeDef);
            attributeTypeTerm.Equal("Text");

            var result = GetFieldList(new AndFilterTerm(assetTypeTerm, isCustomTerm, attributeTypeTerm), new List<IAttributeDefinition> { nameAttributeDef });
            var fieldList = new List<string>();

            result.ForEach(x => fieldList.Add(x.GetAttribute(nameAttributeDef).Value.ToString()));

            return fieldList;
        }

        /// <summary>
        /// Gets collection of custom list fields for specified asset type.
        /// </summary>
        /// <param name="assetTypeName">Name of the asset type</param>
        /// <param name="fieldType">Field type</param>
        /// <returns>collection of custom list fields</returns>
        public IList<string> GetCustomFields(string assetTypeName, FieldType fieldType) {
            var attrType = services.Meta.GetAssetType("AttributeDefinition");
            var assetType = services.Meta.GetAssetType(assetTypeName);
            var isCustomAttributeDef = attrType.GetAttributeDefinition("IsCustom");
            var nameAttrDef = attrType.GetAttributeDefinition("Name");

            var termType = new FilterTerm(attrType.GetAttributeDefinition("Asset.AssetTypesMeAndDown.Name"));
            termType.Equal(assetTypeName);
            
            IAttributeDefinition inactiveDef;
            FilterTerm termState = null;

            if (assetType.TryGetAttributeDefinition("Inactive", out inactiveDef)) {
                termState = new FilterTerm(inactiveDef);
                termState.Equal("False");
            }

            var fieldTypeName = string.Empty;
            var attributeTypeName = string.Empty;

            switch(fieldType) {
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

            var assetTypeTerm = new FilterTerm(attrType.GetAttributeDefinition("AssetType"));
            assetTypeTerm.Equal(fieldTypeName);

            var attributeTypeTerm = new FilterTerm(attrType.GetAttributeDefinition("AttributeType"));
            attributeTypeTerm.Equal(attributeTypeName);

            var isCustomTerm = new FilterTerm(isCustomAttributeDef);
            isCustomTerm.Equal("true");
            
            var result = GetFieldList(new AndFilterTerm(termState, termType, assetTypeTerm, isCustomTerm, attributeTypeTerm),
                new List<IAttributeDefinition> {nameAttrDef});

            var fieldList = new List<string>();
            result.ForEach(x => fieldList.Add(x.GetAttribute(nameAttrDef).Value.ToString()));

            return fieldList;
        }

        private AssetList GetFieldList(IFilterTerm filter, IEnumerable<IAttributeDefinition> selection) {
            var attributeDefinitionAssetType = services.Meta.GetAssetType("AttributeDefinition");

            var query = new Query(attributeDefinitionAssetType);
            foreach(var attribute in selection) {
                query.Selection.Add(attribute);                
            }
            query.Filter = filter;
            return services.Retrieve(query).Assets;
        }

        /// <summary>
        /// Get Source values from VersionOne server
        /// </summary>
        public List<string> GetSourceList() {
            var assetType = services.Meta.GetAssetType("StorySource");
            var nameDef = assetType.GetAttributeDefinition("Name");
            IAttributeDefinition inactiveDef;

            var query = new Query(assetType);
            query.Selection.Add(nameDef);
            
            if(assetType.TryGetAttributeDefinition("Inactive", out inactiveDef)) {
                var filter = new FilterTerm(inactiveDef);
                filter.Equal("False");
                query.Filter = filter;
            }

            query.OrderBy.MajorSort(assetType.DefaultOrderBy, OrderBy.Order.Ascending);

            return services.Retrieve(query).Assets.Select(asset => asset.GetAttribute(nameDef).Value.ToString()).ToList();
        }

        public List<ProjectWrapper> GetProjectList() {
            var projectType = services.Meta.GetAssetType(ProjectTypeToken);
            var scopeQuery = new Query(projectType, projectType.GetAttributeDefinition("Parent"));
            var stateTerm = new FilterTerm(projectType.GetAttributeDefinition("AssetState"));
            stateTerm.NotEqual(AssetState.Closed);
            scopeQuery.Filter = stateTerm;
            var nameDef = projectType.GetAttributeDefinition("Name");
            scopeQuery.Selection.Add(nameDef);
            scopeQuery.OrderBy.MajorSort(projectType.DefaultOrderBy, OrderBy.Order.Ascending);

            var result = services.Retrieve(scopeQuery);

            var roots = new List<ProjectWrapper>(result.Assets.Count);
            
            foreach (Asset asset in result.Assets) {
                roots.AddRange(GetProjectWrapperList(asset, nameDef, 0));
            }
            
            return roots;
        }

        private IEnumerable<ProjectWrapper> GetProjectWrapperList(Asset asset, IAttributeDefinition attrName, int depth) {
            var list = new List<ProjectWrapper>{new ProjectWrapper(asset, asset.GetAttribute(attrName).Value.ToString(), depth)};

            foreach (var child in asset.Children) {
                list.AddRange(GetProjectWrapperList(child, attrName, depth + 1));
            }
            
            return list;
        }

        public virtual IEnumerable<string> GetPrimaryWorkitemTypes() {
            return new[] { "Story", "Defect" };
        }

        public string GetTypeByFieldName(string fieldSystemName, string assetTypeName) {
            IAssetType assetType;
            
            try {
                assetType = services.Meta.GetAssetType(assetTypeName);
            } catch (MetaException ex) {
                throw new AssetTypeException(string.Format("{0} is unknown asset type.", assetTypeName), ex);
            }
            
            IAttributeDefinition attrDef;
            
            try {
                attrDef = assetType.GetAttributeDefinition(fieldSystemName);
            } catch(MetaException ex) {
                throw new FieldNameException(string.Format("{0} is unknown field name for {1}", fieldSystemName, assetTypeName), ex);
            }
            
            return attrDef.RelatedAsset == null ? null : attrDef.RelatedAsset.Token;
        }

        public IDictionary<string, IList<ListValue>> ListPropertyValues { get; private set; }


        /// <summary>
        /// Gets values for specified asset type name.
        /// </summary>
        /// <param name="typeName">Asset type name.</param>
        /// <returns>List of values for the asset type.</returns>
        public IList<ListValue> GetValuesForType(string typeName) {
            if (ListPropertyValues == null) {
                ListPropertyValues = new Dictionary<string, IList<ListValue>>();
            }
            if (!ListPropertyValues.ContainsKey(typeName)) {
                ListPropertyValues.Add(typeName, QueryPropertyOidValues(typeName));
            }
            return ListPropertyValues[typeName];
        }

        private IList<ListValue> QueryPropertyOidValues(string propertyName) {
            var res = new List<ListValue>();
            IAttributeDefinition nameDef;
            var query = GetPropertyValuesQuery(propertyName, out nameDef);

            foreach (var asset in services.Retrieve(query).Assets) {
                var name = asset.GetAttribute(nameDef).Value.ToString();
                res.Add(new ListValue(name, asset.Oid.Momentless.Token));
            }
            
            return res;
        }

        private Query GetPropertyValuesQuery(string propertyName, out IAttributeDefinition nameDef) {
            IAssetType assetType;
            
            try {
                assetType = services.Meta.GetAssetType(propertyName);
            } catch(MetaException ex) {
                throw new AssetTypeException(string.Format("{0} is unknown asset type.", propertyName), ex);
            }
            
            nameDef = assetType.GetAttributeDefinition("Name");

            IAttributeDefinition inactiveDef;
            var query = new Query(assetType);
            query.Selection.Add(nameDef);

            if (assetType.TryGetAttributeDefinition("Inactive", out inactiveDef)) {
                var filter = new FilterTerm(inactiveDef);
                filter.Equal("False");
                query.Filter = filter;
            }
            
            query.OrderBy.MajorSort(assetType.DefaultOrderBy, OrderBy.Order.Ascending);
            return query;
        }
    }
}