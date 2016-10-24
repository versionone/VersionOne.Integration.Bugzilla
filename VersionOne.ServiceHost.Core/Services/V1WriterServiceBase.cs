using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using VersionOne.SDK.APIClient;
using VersionOne.ServiceHost.Eventing;
using VersionOne.Profile;
using VersionOne.ServiceHost.Core.Configuration;
using VersionOne.ServiceHost.Core.Logging;

namespace VersionOne.ServiceHost.Core.Services
{
    // TODO apply ServerConnector
    public abstract class V1WriterServiceBase : IHostedService
    {
        private IServices services;
        protected XmlElement Config;
        protected IEventManager EventManager;
        protected ILogger Logger;

        private const string MemberType = "Member";
        private const string DefaultRoleNameProperty = "DefaultRole.Name";

        protected virtual IServices Services
        {
            get
            {
                if (services == null)
                {
                    try
                    {
                        var settings = VersionOneSettings.FromXmlElement(Config);

                        var connector = V1Connector
                                    .WithInstanceUrl(settings.Url)
                                    .WithUserAgentHeader("VersionOne.Integration.JIRASync", Assembly.GetEntryAssembly().GetName().Version.ToString());

                        ICanSetProxyOrEndpointOrGetConnector connectorWithAuth;

                        switch (settings.AuthenticationType)
                        {
                            case AuthenticationTypes.AccessToken:
                                connectorWithAuth = connector.WithAccessToken(settings.AccessToken);
                                break;
                            default:
                                throw new Exception("Invalid authentication type");
                        }

                        if (settings.ProxySettings.Enabled)
                        {
                            connectorWithAuth.WithProxy(
                                new ProxyProvider(
                                        new Uri(settings.ProxySettings.Url), 
                                        settings.ProxySettings.Username, 
                                        settings.ProxySettings.Password, 
                                        settings.ProxySettings.Domain)
                                    );
                        }

                        services = new SDK.APIClient.Services(connectorWithAuth.Build());

                        if (!services.LoggedIn.IsNull)
                        {
                            LogVersionOneConnectionInformation();
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log("Failed to connect to VersionOne server", ex);
                        throw;
                    }
                }

                return services;
            }
        }

        private void LogVersionOneConnectionInformation()
        {
            try
            {
                var metaVersion = ((MetaModel) Services.Meta).Version.ToString();
                var memberOid = Services.LoggedIn.Momentless.ToString();
                var defaultRole = GetLoggedInMemberRole();

                Logger.LogVersionOneConnectionInformation(LogMessage.SeverityType.Info, metaVersion, memberOid, defaultRole);
            }
            catch (Exception ex)
            {
                Logger.Log(LogMessage.SeverityType.Warning, "Failed to log VersionOne connection information.", ex);
            }
        }

        private string GetLoggedInMemberRole()
        {
            var query = new Query(Services.LoggedIn);
            var defaultRoleAttribute = Services.Meta.GetAssetType(MemberType).GetAttributeDefinition(DefaultRoleNameProperty);
            query.Selection.Add(defaultRoleAttribute);

            var asset = Services.Retrieve(query).Assets[0];
            var role = asset.GetAttribute(defaultRoleAttribute);
            return Services.Localization(role.Value.ToString());
        }

        public virtual void Initialize(XmlElement config, IEventManager eventManager, IProfile profile)
        {
            Config = config;
            EventManager = eventManager;
            Logger = new Logger(eventManager);

            Logger.LogVersionOneConfiguration(LogMessage.SeverityType.Info, Config["Settings"]);
        }

        public void Start()
        {
            // TODO move subscriptions to timer events, etc. here
        }

        protected abstract IEnumerable<NeededAssetType> NeededAssetTypes { get; }

        protected void VerifyMeta()
        {
            try
            {
                VerifyNeededMeta(NeededAssetTypes);
                VerifyRuntimeMeta();
            }
            catch (MetaException ex)
            {
                throw new ApplicationException("Necessary meta is not present in this VersionOne system", ex);
            }
        }

        protected virtual void VerifyRuntimeMeta()
        {
        }

        protected struct NeededAssetType
        {
            public readonly string Name;
            public readonly string[] AttributeDefinitionNames;

            public NeededAssetType(string name, string[] attributedefinitionnames)
            {
                Name = name;
                AttributeDefinitionNames = attributedefinitionnames;
            }
        }

        protected void VerifyNeededMeta(IEnumerable<NeededAssetType> neededassettypes)
        {
            foreach (var neededAssetType in neededassettypes)
            {
                var assettype = Services.Meta.GetAssetType(neededAssetType.Name);

                foreach (var attributeDefinitionName in neededAssetType.AttributeDefinitionNames)
                {
                    var attribdef = assettype.GetAttributeDefinition(attributeDefinitionName);
                }
            }
        }

        #region Meta wrappers

        protected IAssetType RequestType
        {
            get { return Services.Meta.GetAssetType("Request"); }
        }

        protected IAssetType DefectType
        {
            get { return Services.Meta.GetAssetType("Defect"); }
        }

        protected IAssetType StoryType
        {
            get { return Services.Meta.GetAssetType("Story"); }
        }

        protected IAssetType ReleaseVersionType
        {
            get { return Services.Meta.GetAssetType("StoryCategory"); }
        }

        protected IAssetType LinkType
        {
            get { return Services.Meta.GetAssetType("Link"); }
        }

        protected IAssetType NoteType
        {
            get { return Services.Meta.GetAssetType("Note"); }
        }

        protected IAttributeDefinition DefectName
        {
            get { return DefectType.GetAttributeDefinition("Name"); }
        }

        protected IAttributeDefinition DefectDescription
        {
            get { return DefectType.GetAttributeDefinition("Description"); }
        }

        protected IAttributeDefinition DefectOwners
        {
            get { return DefectType.GetAttributeDefinition("Owners"); }
        }

        protected IAttributeDefinition DefectScope
        {
            get { return DefectType.GetAttributeDefinition("Scope"); }
        }

        protected IAttributeDefinition DefectAssetState
        {
            get { return RequestType.GetAttributeDefinition("AssetState"); }
        }

        protected IAttributeDefinition RequestCompanyName
        {
            get { return RequestType.GetAttributeDefinition("Name"); }
        }

        protected IAttributeDefinition RequestNumber
        {
            get { return RequestType.GetAttributeDefinition("Number"); }
        }

        protected IAttributeDefinition RequestSuggestedInstance
        {
            get { return RequestType.GetAttributeDefinition("Reference"); }
        }

        protected IAttributeDefinition RequestMethodology
        {
            get { return RequestType.GetAttributeDefinition("Source"); }
        }

        protected IAttributeDefinition RequestMethodologyName
        {
            get { return RequestType.GetAttributeDefinition("Source.Name"); }
        }

        protected IAttributeDefinition RequestCommunityEdition
        {
            get { return RequestType.GetAttributeDefinition("Custom_CommunityEdition"); }
        }

        protected IAttributeDefinition RequestAssetState
        {
            get { return RequestType.GetAttributeDefinition("AssetState"); }
        }

        protected IAttributeDefinition RequestCreateDate
        {
            get { return RequestType.GetAttributeDefinition("CreateDate"); }
        }

        protected IAttributeDefinition RequestCreatedBy
        {
            get { return RequestType.GetAttributeDefinition("CreatedBy"); }
        }

        protected IOperation RequestInactivate
        {
            get { return Services.Meta.GetOperation("Request.Inactivate"); }
        }

        protected IAttributeDefinition StoryName
        {
            get { return StoryType.GetAttributeDefinition("Name"); }
        }

        protected IAttributeDefinition StoryActualInstance
        {
            get { return StoryType.GetAttributeDefinition("Reference"); }
        }

        protected IAttributeDefinition StoryRequests
        {
            get { return StoryType.GetAttributeDefinition("Requests"); }
        }

        protected IAttributeDefinition StoryReleaseVersion
        {
            get { return StoryType.GetAttributeDefinition("Category"); }
        }

        protected IAttributeDefinition StoryMethodology
        {
            get { return StoryType.GetAttributeDefinition("Source"); }
        }

        protected IAttributeDefinition StoryCommunitySite
        {
            get { return StoryType.GetAttributeDefinition("Custom_CommunitySite"); }
        }

        protected IAttributeDefinition StoryScope
        {
            get { return StoryType.GetAttributeDefinition("Scope"); }
        }

        protected IAttributeDefinition StoryOwners
        {
            get { return StoryType.GetAttributeDefinition("Owners"); }
        }

        protected IAttributeDefinition ReleaseVersionName
        {
            get { return ReleaseVersionType.GetAttributeDefinition("Name"); }
        }

        protected IAttributeDefinition LinkAsset
        {
            get { return LinkType.GetAttributeDefinition("Asset"); }
        }

        protected IAttributeDefinition LinkOnMenu
        {
            get { return LinkType.GetAttributeDefinition("OnMenu"); }
        }

        protected IAttributeDefinition LinkUrl
        {
            get { return LinkType.GetAttributeDefinition("URL"); }
        }

        protected IAttributeDefinition LinkName
        {
            get { return LinkType.GetAttributeDefinition("Name"); }
        }

        protected IAttributeDefinition NoteName
        {
            get { return NoteType.GetAttributeDefinition("Name"); }
        }

        protected IAttributeDefinition NoteAsset
        {
            get { return NoteType.GetAttributeDefinition("Asset"); }
        }

        protected IAttributeDefinition NotePersonal
        {
            get { return NoteType.GetAttributeDefinition("Personal"); }
        }

        protected IAttributeDefinition NoteContent
        {
            get { return NoteType.GetAttributeDefinition("Content"); }
        }

        #endregion
    }
}