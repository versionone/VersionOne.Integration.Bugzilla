using Newtonsoft.Json.Linq;
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
            var bugzillaClient = bugzillaClientFactory.CreateNew(configuration.Url);
            //var bugzillaClient = new BugzillaClient(configuration.Url);

            var ids = bugzillaClient.Search(configuration.OpenIssueFilterId);
            
            //var bugs = bugzillaClient.Search(configuration.OpenIssueFilterId);
            // var ids = bugzillaClient.LoginSearch(configuration.UserName, configuration.Password, true, configuration.OpenIssueFilterId, configuration.IgnoreCert);
            
            //creates a list of defects
            var defects = new List<Defect>(ids.Count());

            //foreach (var id in ids) 
            foreach (JToken id in ids)
            {

                //get bug id from bug
                var bug = bugzillaClient.GetBug(id.Value<int>());

                //get product definition from bug
               // var product = bugzillaClient.GetProduct(bug.ProductID);
                //get user info from bug
               // var user = bugzillaClient.GetUser(bug.AssignedToID);


                var projectMapping = ResolveVersionOneProjectMapping(bug.Product);
                //var projectMapping = ResolveVersionOneProjectMapping(product.Name);

                var priorityMapping = ResolveVersionOnePriorityMapping(bug.Priority);



                //var defect = new Defect(bug.Name, bug.Description, projectMapping.Name, user.Login) 
                //    { ExternalId = bug.ID.ToString(CultureInfo.InvariantCulture),
                //      ProjectId = projectMapping.Id };
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
                //logger.Log(string.Format("Product: ({0}) Bug: ({1}) Defect: ({2}) AssignedTo: ({3})", product, bug, defect, user));
				defects.Add(defect);
            }

            //bugzillaClient.Logout();
			return defects;
		}

		public void OnDefectCreated(WorkitemCreationResult createdResult) 
        {
			var bugId = int.Parse(createdResult.Source.ExternalId);

            var bugzillaClient = new BugzillaClient(configuration.Url);
            //var bugzillaClient = bugzillaClientFactory.CreateNew(configuration.Url);

            //bugzillaClient.Login(configuration.UserName, configuration.Password, true, configuration.IgnoreCert);

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

            ResolveBugIfRequired(configuration.OnCreateResolveValue, bugId, bugzillaClient);
			//bugzillaClient.Logout();
		}

        private void ResolveBugIfRequired(string resolution, int bugId, IBugzillaClient client) 
        {
            if(string.IsNullOrEmpty(resolution)) 
            {
                return;
            }

            try 
            {
                //if(!client.ResolveBug(bugId, resolution, string.Empty))
                if (!client.ResolveBug(bugId, resolution))
                    {
                    logger.Log(LogMessage.SeverityType.Error, string.Format("Failed to resolve bug to {0}.", resolution));
                }
                //} catch(BugzillaException ex) 
            }
            catch (Exception ex)
            {
                logger.Log(LogMessage.SeverityType.Error, "Failed to resolve bug: " + ex.InnerException.Message);
            }
        }

		public bool OnDefectStateChange(WorkitemStateChangeResult stateChangeResult) 
        {
			logger.Log(LogMessage.SeverityType.Debug, stateChangeResult.ToString());

			var bugId = int.Parse(stateChangeResult.ExternalId);

            var bugzillaClient = bugzillaClientFactory.CreateNew(configuration.Url);

            //bugzillaClient.Login(configuration.UserName, configuration.Password, true, configuration.IgnoreCert);

            // We do not need to push changes to Defects that have been processed as we could break their state.
            if(SkipCloseActions(bugId, bugzillaClient)) 
            {
                logger.Log(LogMessage.SeverityType.Info, string.Format("Defect {0} has already been processed, check CloseFieldId and CloseReassignValue.", bugId));
                return true;
            }

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
			//bugzillaClient.Logout();
			return true;
		}

        private bool SkipCloseActions(int bugId, IBugzillaClient client) 
        {
            if(!string.IsNullOrEmpty(configuration.OnStateChangeFieldName)) 
            {
                var fieldValue = client.GetFieldValue(bugId, configuration.OnStateChangeFieldName);
                
                if(fieldValue.Equals(configuration.OnStateChangeFieldValue)) 
                {
                    return true;
                }
            }

            var reassignValue = configuration.OnStateChangeReassignValue;
            
            if(!string.IsNullOrEmpty(reassignValue)) 
            {
                //valid the AssignTo user

                return client.IsValidUser(reassignValue);
                //var bug = client.GetBug(bugId);
                //var user = client.GetUser(bug.AssignedToID);
                //if(!string.IsNullOrEmpty(user.Login) && user.Login.Equals(reassignValue)) 
                //{
                //    return ;
                // }
            }

            return false;
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