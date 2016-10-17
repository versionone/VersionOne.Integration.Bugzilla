using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VersionOne.Bugzilla.BugzillaAPI;
using VersionOne.ServiceHost.Core.Logging;

using VersionOne.ServiceHost.WorkitemServices;
using MappingInfo = VersionOne.ServiceHost.Core.Configuration.MappingInfo;

namespace VersionOne.ServiceHost.BugzillaServices 
{
	public class BugzillaReaderUpdater : IBugzillaReader, IBugzillaUpdater 
    {
		private readonly BugzillaServiceConfiguration configuration;
		private readonly IBugzillaClientFactory bugzillaClientFactory;
        private readonly ILogger logger;

		public BugzillaReaderUpdater(BugzillaServiceConfiguration configuration, IBugzillaClientFactory bugzillaClientFactory, ILogger logger) 
        {
			this.configuration = configuration;
            this.bugzillaClientFactory = bugzillaClientFactory;
            this.logger = logger;
		}

        public List<Defect> GetBugs() 
        {
            var bugzillaClient = bugzillaClientFactory.CreateNew();

            bugzillaClient.Login();

            var ids = bugzillaClient.Search(configuration.OpenIssueFilterId);

            var defects = new List<Defect>(ids.Count());

            foreach (int id in ids)
            {
                var bug = bugzillaClient.GetBug(id);

                var projectMapping = ResolveVersionOneProjectMapping(bug.Product);
                
                var priorityMapping = ResolveVersionOnePriorityMapping(bug.Priority);

                var defect = new Defect(bug.Name, bug.Description, bug.Product, bug.AssignedTo)
                    { ExternalId = bug.ID.ToString(CultureInfo.InvariantCulture),
                      ProjectId = projectMapping.Id };

                // If the BugzillaBugUrlTemplate tag of the config file is set, then build a URL to the issue in Bugzilla.
                if (!string.IsNullOrEmpty(configuration.UrlTemplateToIssue) && !string.IsNullOrEmpty(configuration.UrlTitleToIssue)) 
                {
					defect.ExternalLink = new UrlToExternalSystem(configuration.UrlTemplateToIssue.Replace("#key#", bug.ID.ToString()), configuration.UrlTitleToIssue);
				}

                if (priorityMapping != null) 
                {
                    defect.Priority = priorityMapping.Id;
                }

                logger.Log(string.Format("Product: ({0}) Bug: ({1}) Defect: ({2}) AssignedTo: ({3})", bug.Product, bug, defect, bug.AssignedTo));
                defects.Add(defect);
            }

            return defects;
		}

		public void OnDefectCreated(WorkitemCreationResult createdResult) 
        {
			var bugId = int.Parse(createdResult.Source.ExternalId);

            var bugzillaClient = bugzillaClientFactory.CreateNew();

            bugzillaClient.Login();
            
            if (configuration.OnCreateAccept && !bugzillaClient.AcceptBug(bugId, configuration.OnCreateResolveValue)) 
            {
				logger.Log(LogMessage.SeverityType.Error, string.Format("Failed to accept bug {0}.", bugId));
			}

            if (!string.IsNullOrEmpty(configuration.OnCreateFieldName) && !bugzillaClient.UpdateBug(bugId, configuration.OnCreateFieldName, configuration.OnCreateFieldValue)) 
            {
    			logger.Log(LogMessage.SeverityType.Error, string.Format("Failed to set {0} to {1}.", configuration.OnCreateFieldName, configuration.OnCreateFieldValue));
			}

            if (!string.IsNullOrEmpty(configuration.DefectLinkFieldName)) 
            {
				if (!bugzillaClient.UpdateBug(bugId, configuration.DefectLinkFieldName, createdResult.Permalink)) 
                {
					logger.Log(LogMessage.SeverityType.Error, string.Format("Failed to set {0} to {1}.", configuration.DefectLinkFieldName, createdResult.Permalink));
				}
			}

            if (!string.IsNullOrEmpty(configuration.OnCreateReassignValue)) 
            {
				if (!bugzillaClient.ReassignBug(bugId, configuration.OnCreateReassignValue)) 
                {
					logger.Log(LogMessage.SeverityType.Error, string.Format("Failed to reassign bug to {0}.", configuration.OnCreateReassignValue));
				}
			}
		}

        public bool OnDefectStateChange(WorkitemStateChangeResult stateChangeResult) 
        {
			logger.Log(LogMessage.SeverityType.Debug, stateChangeResult.ToString());

			var bugId = int.Parse(stateChangeResult.ExternalId);

            var bugzillaClient = bugzillaClientFactory.CreateNew();

            bugzillaClient.Login();
            
            if (configuration.OnStateChangeAccept && !bugzillaClient.AcceptBug(bugId , configuration.OnCreateResolveValue))
            {
    			logger.Log(LogMessage.SeverityType.Error, string.Format("Failed to accept bug {0}.", bugId));
			}

			if (!string.IsNullOrEmpty(configuration.OnStateChangeFieldName)) 
            {
				if (!bugzillaClient.UpdateBug(bugId, configuration.OnStateChangeFieldName, configuration.OnStateChangeFieldValue)) 
                {
					logger.Log(LogMessage.SeverityType.Error, string.Format("Failed to set {0} to {1}.", configuration.OnStateChangeFieldName, configuration.OnStateChangeFieldValue));
				}
			}

			if (!string.IsNullOrEmpty(configuration.OnStateChangeReassignValue)) 
            {
				if (!bugzillaClient.ReassignBug(bugId, configuration.OnStateChangeReassignValue)) 
                {
					logger.Log(LogMessage.SeverityType.Error, string.Format("Failed to reassign bug to {0}.", configuration.OnStateChangeReassignValue));
				}
			}

            ResolveBugIfRequired(configuration.OnStateChangeResolveValue, bugId, bugzillaClient);

            return true;
		}

        private void ResolveBugIfRequired(string resolution, int bugId, IBugzillaClient client)
        {
            try
            {

                if (string.IsNullOrEmpty(resolution))
                {
                    throw new Exception("There was an attempt to resolve a bug without having a configured resolution value.");
                }

                if (!client.ResolveBug(bugId, resolution))
                {
                    logger.Log(LogMessage.SeverityType.Error, string.Format("Failed to resolve bug to {0}.", resolution));
                }
            }
            catch (Exception ex)
            {
                logger.Log(LogMessage.SeverityType.Error, "Failed to resolve bug: " + ex.InnerException.Message);
            }
        }

        /// <summary>
        /// Return corresponding mapping of Bugzilla and VersionOne projects, if any.
        /// Mappings are provided by users in configuration file.
        /// If there's no explicit mapping defined, we try to use exact matching by project names.
        /// </summary>
        /// <param name="bugzillaProject">Bugzilla project name</param>
        private MappingInfo ResolveVersionOneProjectMapping(string bugzillaProject) 
        {
            foreach (var mapping in configuration.ProjectMappings.Where(mapping => mapping.Key.Name.Equals(bugzillaProject))) 
            {
                return mapping.Value;
            }

            return new MappingInfo(null, bugzillaProject);
        }

	    /// <summary>
        /// Return corresponding mapping of Bugzilla and VersionOne priorities, if any.
        /// Mappings are provided by users in configuration file.
        /// If there's no explicit mapping defined, we should not set priority.
        /// </summary>
        /// <param name="bugzillaPriority">Bugzilla project name</param>
        private MappingInfo ResolveVersionOnePriorityMapping(string bugzillaPriority) 
        {
	        return configuration.PriorityMappings.Where(x => x.Key.Name.Equals(bugzillaPriority)).Select(mapping => mapping.Value).FirstOrDefault();
	    }
	}
}