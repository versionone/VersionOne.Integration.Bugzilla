using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using Ninject;
using VersionOne.Profile;
using VersionOne.ServerConnector;
using VersionOne.ServerConnector.Entities;
using VersionOne.ServiceHost.Core.Logging;
using VersionOne.ServiceHost.Core.Services;
using VersionOne.ServiceHost.Core.Utility;
using VersionOne.ServiceHost.Eventing;

namespace VersionOne.ServiceHost.WorkitemServices {
    public class WorkitemWriterHostedService : IHostedService, IComponentProvider {
        private const string DateFormat = "MM/dd/yyyy HH:mm:ss.fff";
        private XmlElement configElement;
        private IEventManager eventManager;
        private ClosedExternalWorkitemQuerier externalWorkitemQuerier;
        private ILogger logger;
        private IProfile profile;
        private IVersionOneProcessor v1Processor;
        private IWorkitemWriter workitemWriter;
        private IWorkitemReader workitemReader;

        private readonly WorkitemWriterServiceConfiguration configuration = new WorkitemWriterServiceConfiguration();

        private StartupChecker startupChecker;

        private DateTime LastCheckForClosedWorkitems {
            get {
                var lastCheckString = profile["LastCheckForClosedWorkitems"].Value;
                DateTime lastClosedWorkitemCheck;

                if(!DateTime.TryParseExact(lastCheckString, DateFormat, null, DateTimeStyles.None, out lastClosedWorkitemCheck)) {
                    lastClosedWorkitemCheck = DateTime.MinValue;
                }

                return lastClosedWorkitemCheck;
            }
            set { profile["LastCheckForClosedWorkitems"].Value = value.ToString(DateFormat); }
        }

        private string LastClosedWorkitemId {
            get {
                var lastCheckString = string.Empty;

                if(profile["LastClosedWorkitemId"] != null && profile["LastClosedWorkitemId"].Value != null) {
                    lastCheckString = profile["LastClosedWorkitemId"].Value;
                }

                return lastCheckString;
            }
            set { profile["LastClosedWorkitemId"].Value = value; }
        }

        public void Initialize(XmlElement configElement, IEventManager manager, IProfile profile) {
            eventManager = manager;
            this.profile = profile;
            logger = new Logger(eventManager);
            this.configElement = configElement;

            ConfigurationReader.ReadConfigurationValues(configuration, configElement);

            InitializeComponents();
        }

        public void RegisterComponents(IKernel container) {
            container.Rebind<IEventManager>().ToConstant(eventManager);
            container.Bind<IVersionOneProcessor>().ToConstant(v1Processor);
            container.Rebind<ILogger>().To<Logger>();
            container.Bind<WorkitemWriterServiceConfiguration>().ToConstant(configuration);
            container.Bind<IWorkitemWriter>().To<WorkitemWriter>();
            container.Bind<IWorkitemReader>().To<WorkitemReader>();

            workitemWriter = container.Get<IWorkitemWriter>();
            workitemReader = container.Get<IWorkitemReader>();
            externalWorkitemQuerier = container.Get<ClosedExternalWorkitemQuerier>();
            startupChecker = container.Get<StartupChecker>();
        }

        public void Start() {
            eventManager.Subscribe(typeof(Defect), ProcessWorkitem);
            eventManager.Subscribe(typeof(Story), ProcessWorkitem);
            eventManager.Subscribe(typeof(ClosedWorkitemsSource), GetClosedExternalWorkitems);
            startupChecker.Initialize();
        }

        private void InitializeComponents() {
            v1Processor = new VersionOneProcessor(configElement["Settings"], logger);

            v1Processor.AddProperty(Entity.NameProperty, VersionOneProcessor.PrimaryWorkitemType, false);
            v1Processor.AddProperty(Entity.DescriptionProperty, VersionOneProcessor.PrimaryWorkitemType, false);
            v1Processor.AddProperty(ServerConnector.Entities.Workitem.NumberProperty, VersionOneProcessor.PrimaryWorkitemType, false);
            v1Processor.AddProperty(ServerConnector.Entities.Workitem.OwnersProperty, VersionOneProcessor.PrimaryWorkitemType, false);
            v1Processor.AddProperty(configuration.ExternalIdFieldName, VersionOneProcessor.PrimaryWorkitemType, false);
            v1Processor.AddProperty(ServerConnector.Entities.Workitem.ChangeDateUtcProperty, VersionOneProcessor.PrimaryWorkitemType, false);
            v1Processor.AddProperty(ServerConnector.Entities.Workitem.ScopeProperty, VersionOneProcessor.PrimaryWorkitemType, false);
            v1Processor.AddProperty(ServerConnector.Entities.Workitem.ScopeNameProperty, VersionOneProcessor.DefectType, false);
            v1Processor.AddProperty(ServerConnector.Entities.Workitem.PriorityProperty, VersionOneProcessor.PrimaryWorkitemType, false);

            v1Processor.AddProperty(Entity.NameProperty, VersionOneProcessor.DefectType, false);
            v1Processor.AddProperty(Entity.DescriptionProperty, VersionOneProcessor.DefectType, false);
            v1Processor.AddProperty(ServerConnector.Entities.Workitem.NumberProperty, VersionOneProcessor.DefectType, false);
            v1Processor.AddProperty(ServerConnector.Entities.Workitem.OwnersProperty, VersionOneProcessor.DefectType, false);
            v1Processor.AddProperty(configuration.ExternalIdFieldName, VersionOneProcessor.DefectType, false);
            v1Processor.AddProperty(ServerConnector.Entities.Workitem.ChangeDateUtcProperty, VersionOneProcessor.DefectType, false);
            v1Processor.AddProperty(ServerConnector.Entities.Workitem.ScopeProperty, VersionOneProcessor.DefectType, false);
            v1Processor.AddProperty(ServerConnector.Entities.Workitem.ScopeNameProperty, VersionOneProcessor.DefectType, false);
            v1Processor.AddProperty(ServerConnector.Entities.Workitem.PriorityProperty, VersionOneProcessor.DefectType, false);
            v1Processor.AddProperty(ServerConnector.Entities.Workitem.AssetStateProperty, VersionOneProcessor.DefectType, false);

            v1Processor.AddProperty(Entity.NameProperty, VersionOneProcessor.StoryType, false);
            v1Processor.AddProperty(Entity.DescriptionProperty, VersionOneProcessor.StoryType, false);
            v1Processor.AddProperty(ServerConnector.Entities.Workitem.NumberProperty, VersionOneProcessor.StoryType, false);
            v1Processor.AddProperty(ServerConnector.Entities.Workitem.OwnersProperty, VersionOneProcessor.StoryType, false);
            v1Processor.AddProperty(configuration.ExternalIdFieldName, VersionOneProcessor.StoryType, false);
            v1Processor.AddProperty(ServerConnector.Entities.Workitem.ChangeDateUtcProperty, VersionOneProcessor.StoryType, false);
            v1Processor.AddProperty(ServerConnector.Entities.Workitem.ScopeProperty, VersionOneProcessor.StoryType, false);
            v1Processor.AddProperty(ServerConnector.Entities.Workitem.ScopeNameProperty, VersionOneProcessor.StoryType, false);
            v1Processor.AddProperty(ServerConnector.Entities.Workitem.PriorityProperty, VersionOneProcessor.StoryType, false);
            v1Processor.AddProperty(ServerConnector.Entities.Workitem.AssetStateProperty, VersionOneProcessor.StoryType, false);

            v1Processor.AddProperty(Member.EmailProperty, VersionOneProcessor.MemberType, false);
            v1Processor.AddProperty(Member.DefaultRoleNameProperty, VersionOneProcessor.MemberType, false);
            v1Processor.AddProperty(Entity.NameProperty, VersionOneProcessor.MemberType, false);
        }

        private void GetClosedExternalWorkitems(object pubobj) {
            var sourceToCheckFor = pubobj as ClosedWorkitemsSource;

            if(sourceToCheckFor == null || string.IsNullOrEmpty(sourceToCheckFor.SourceValue)) {
                return;
            }

            logger.Log(LogMessage.SeverityType.Info,
                string.Format("Checking V1 workitems closed since {0} was closed at {1}.", LastClosedWorkitemId, LastCheckForClosedWorkitems));
            var closedWorkitems = externalWorkitemQuerier.GetWorkitemsClosedSince(LastCheckForClosedWorkitems, sourceToCheckFor.BaseWorkitemType, 
                sourceToCheckFor.SourceValue, LastClosedWorkitemId);

            logger.Log(LogMessage.SeverityType.Info,
                string.Format("Found {0} workitems closed since {1} where Source is '{2}'.", closedWorkitems.Count, LastCheckForClosedWorkitems,
                    sourceToCheckFor.SourceValue));

            if(closedWorkitems.Count > 0) {
                logger.Log(LogMessage.SeverityType.Info, string.Format("Closing issues in {0}.", sourceToCheckFor.SourceValue));
                eventManager.Publish(closedWorkitems);

                foreach(var result in closedWorkitems.Where(result => result.ChangesProcessed)) {
                    logger.Log(LogMessage.SeverityType.Info, string.Format("Issue {0} closed successfully.", result.ExternalId));
                }
            }

            LastCheckForClosedWorkitems = closedWorkitems.QueryTimeStamp;
            LastClosedWorkitemId = closedWorkitems.LastCheckedDefectId;
            logger.Log(LogMessage.SeverityType.Debug, string.Format("Updating last check time to {0}.", LastCheckForClosedWorkitems));
        }

        private void ProcessWorkitem(object pubobj) {
            var toSendToV1 = pubobj as Workitem;

            if(toSendToV1 == null) {
                return;
            }

            try {
                var duplicates = workitemReader.GetDuplicates(toSendToV1);

                if (!AllDuplicatesClosed(duplicates)) {
                    logger.Log(LogMessage.SeverityType.Info,
                        string.Format("Found existing workitem for {0} issue {1}.", toSendToV1.ExternalSystemName, toSendToV1.ExternalId));
                    return;
                }

                var result = CreateWorkitem(toSendToV1, duplicates);

                foreach(var warning in result.Warnings) {
                    logger.Log(LogMessage.SeverityType.Info, string.Format("\t{0}", warning));
                }

                eventManager.Publish(result);
            } catch(Exception ex) {
                logger.Log(LogMessage.SeverityType.Error,
                    string.Format("Error trying to create Workitem in VersionOne for {0} in {1}:", toSendToV1, toSendToV1.ExternalSystemName));
                logger.Log(LogMessage.SeverityType.Error, ex.ToString());
            }
        }

        private WorkitemCreationResult CreateWorkitem(Workitem toSendToV1, IList<ServerConnector.Entities.Workitem> duplicates) {
            if(duplicates.Count == 0) {
                return workitemWriter.CreateWorkitem(toSendToV1);
            }

            var lastDuplicate = FindLastDuplicate(duplicates);

            var result = workitemWriter.CreateWorkitem(toSendToV1, lastDuplicate);
            return result;
        }

        private static bool AllDuplicatesClosed(IEnumerable<ServerConnector.Entities.Workitem> duplicates) {
            return duplicates.All(item => item.IsClosed);
        }

        private static ServerConnector.Entities.Workitem FindLastDuplicate(IEnumerable<ServerConnector.Entities.Workitem> duplicates) {
            return duplicates.OrderByDescending(item => item.ChangeDateUtc).FirstOrDefault();
        }
    }
}