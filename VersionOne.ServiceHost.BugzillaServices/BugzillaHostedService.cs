using System;
using System.Xml;
using VersionOne.Bugzilla.XmlRpcProxy;
using VersionOne.Profile;
using VersionOne.ServiceHost.Core.Logging;
using VersionOne.ServiceHost.Core.Services;
using VersionOne.ServiceHost.Core.Utility;
using VersionOne.ServiceHost.Eventing;
using VersionOne.ServiceHost.WorkitemServices;

namespace VersionOne.ServiceHost.BugzillaServices {
    public class BugzillaHostedService : IHostedService {
        private IBugzillaReader bugzillaReader;
        private IBugzillaUpdater bugzillaUpdater;
        private IEventManager eventManager;
        private string sourceFieldValue;
        private ILogger logger;

        private const string ProjectMappingsNode = "ProjectMappings";
        private const string PriorityMappingsNode = "PriorityMappings";
        private const string BugzillaProjectNode = "BugzillaProject";
        private const string VersionOneProjectNode = "VersionOneProject";
        private const string BugzillaPriorityNode = "BugzillaPriority";
        private const string VersionOnePriorityNode = "VersionOnePriority";

        public void Initialize(XmlElement config, IEventManager eventManager, IProfile profile) {
            var bugzillaConfig = new BugzillaServiceConfiguration();

            ConfigurationReader.ReadConfigurationValues(bugzillaConfig, config);

            ConfigurationReader.ProcessMappingSettings(bugzillaConfig.ProjectMappings, config[ProjectMappingsNode], BugzillaProjectNode, VersionOneProjectNode);
            ConfigurationReader.ProcessMappingSettings(bugzillaConfig.PriorityMappings, config[PriorityMappingsNode], BugzillaPriorityNode, VersionOnePriorityNode);

            sourceFieldValue = config["SourceFieldValue"].InnerText;

            this.eventManager = eventManager;
            logger = new Logger(eventManager);

            var readerUpdater = new BugzillaReaderUpdater(bugzillaConfig, new BugzillaClientFactory(), logger);
            bugzillaReader = readerUpdater;
            bugzillaUpdater = readerUpdater;

            this.eventManager.Subscribe(typeof(IntervalSync), OnInterval);
            this.eventManager.Subscribe(typeof(WorkitemCreationResult), OnDefectCreated);
            this.eventManager.Subscribe(typeof(WorkitemStateChangeCollection), OnDefectStateChanged);
        }

        public void Start() {
            // TODO move subscriptions to timer events, etc. here
        }

        private void OnInterval(object pubobj) {
            try {
                var bugs = bugzillaReader.GetBugs();

                if (bugs.Count > 0) {
                    logger.Log(string.Format("Found {0} issues in Bugzilla to create in VersionOne.", bugs.Count));
                }

                foreach (var bug in bugs) {
                    bug.ExternalSystemName = sourceFieldValue;
                    eventManager.Publish(bug);
                }

                var source = new ClosedWorkitemsSource(sourceFieldValue);
                eventManager.Publish(source);
            } catch (Exception ex) {
                logger.Log(LogMessage.SeverityType.Error, "Error getting Issues from Bugzilla:");
                logger.Log(LogMessage.SeverityType.Error, ex.ToString());
            }
        }

        /// <summary>
        ///   A Defect was created in V1 that corresponds to an Issue in Bugzilla. We update the bug in 
        ///   Bugzilla to reflect that.
        /// </summary>
        /// <param name = "pubobj">DefectCreationResult of created defect.</param>
        private void OnDefectCreated(object pubobj) {
            var workitemCreationResult = pubobj as WorkitemCreationResult;

            if(workitemCreationResult != null) {
                bugzillaUpdater.OnDefectCreated(workitemCreationResult);
            }
        }

        private void OnDefectStateChanged(object pubobj) {
            // to avoid creating closed issues
            var workitemStateChangeCollection = pubobj as WorkitemStateChangeCollection;

            if(workitemStateChangeCollection != null) {
              //  eventManager.Unsubscribe(typeof(WorkitemCreationResult), OnDefectCreated);
                var success = true;

                foreach(var defectStateChangeResult in workitemStateChangeCollection) {
                    if(!bugzillaUpdater.OnDefectStateChange(defectStateChangeResult)) {
                        success = false;
                    }
                }

                workitemStateChangeCollection.ChangesProcessed = success;

                //eventManager.Subscribe(typeof(WorkitemCreationResult), OnDefectCreated);
            }
        } 

        public class IntervalSync { }
    }
}