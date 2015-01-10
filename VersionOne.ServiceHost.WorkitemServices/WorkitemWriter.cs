using System;
using VersionOne.ServerConnector;
using VersionOne.ServerConnector.Entities;
using VersionOne.ServiceHost.Core.Logging;

namespace VersionOne.ServiceHost.WorkitemServices {
    /// <summary>
    /// Creates workitems in VersionOne to match issues that come from an external system.
    /// </summary>
    public class WorkitemWriter : IWorkitemWriter {
        private readonly WorkitemWriterServiceConfiguration configuration;
        private readonly IVersionOneProcessor v1Processor;
        private readonly ILogger logger;

        public WorkitemWriter(WorkitemWriterServiceConfiguration configuration, ILogger logger, IVersionOneProcessor v1Processor) {
            this.configuration = configuration;
            this.logger = logger;
            this.v1Processor = v1Processor;
        }

        public WorkitemCreationResult CreateWorkitem(Workitem item) {
            var workitem = CreateNewWorkitem(item);
            return GetWorkitemCreationResult(item, workitem);
        }

        public WorkitemCreationResult CreateWorkitem(Workitem item, ServerConnector.Entities.Workitem closedDuplicate) {
            item.Description += string.Format("\n\n This is a copy of '{0}' {1}", closedDuplicate.Number, closedDuplicate.TypeName);
            var workitem = CreateNewWorkitem(item);

            var url = v1Processor.GetSummaryLink(closedDuplicate);
            var link = new Link(url, "Previous item");
            v1Processor.AddLinkToEntity(workitem, link);

            return GetWorkitemCreationResult(item, workitem);
        }

        private WorkitemCreationResult GetWorkitemCreationResult(Workitem item, ServerConnector.Entities.Workitem newWorkitem) {
            if(newWorkitem != null) {
                var result = ConvertToWorkitemCreationResult(item, newWorkitem);

                result.Messages.Add(string.Format("Created item \"{0}\" ({1}) in Project \"{2}\" URL: {3}",
                    item.Title,
                    result.Source.Number,
                    item.Project,
                    result.Permalink));

                return result;
            }

            return null;
        }

        private ServerConnector.Entities.Workitem CreateNewWorkitem(Workitem item) {
            if(item == null) {
                throw new ArgumentNullException("item");
            }

            var type = item.Type;

            logger.Log(LogMessage.SeverityType.Info,
                string.Format("Creating VersionOne {0} for item from {1} system with identifier {2}", type,
                    item.ExternalSystemName, item.ExternalId));

            var projectToken = FindProperProjectToken(item.ProjectId, item.Project);

            try {
                var newWorkitem = v1Processor.CreateWorkitem(type, item.Title, item.Description, projectToken,
                    configuration.ExternalIdFieldName, item.ExternalId, item.ExternalSystemName, item.Priority, item.Owners);
                AddLinkToWorkitemToOriginalIssue(item, newWorkitem);
                logger.Log(LogMessage.SeverityType.Info, string.Format("VersionOne asset {0} succesfully created.", newWorkitem.Id));
                return newWorkitem;
            } catch(Exception ex) {
                logger.Log(LogMessage.SeverityType.Error, string.Format("Error during saving workitems: {0}", ex.Message));
                return null;
            }
        }

        private WorkitemCreationResult ConvertToWorkitemCreationResult(Workitem item, ServerConnector.Entities.Workitem newWorkitem) {
            var result = new WorkitemCreationResult(item) {
                Source = {
                    Number = newWorkitem.Number,
                    ExternalId = item.ExternalId,
                    Description = newWorkitem.Description,
                    ExternalSystemName = item.ExternalSystemName,
                    ProjectId = newWorkitem.Project.Key,
                    Project = newWorkitem.Project.Value,
                    Title = newWorkitem.Name,
                    Priority = newWorkitem.PriorityToken,
                },
                WorkitemId = newWorkitem.Id,
                Permalink = v1Processor.GetWorkitemLink(newWorkitem),
            };
            return result;
        }

        private void AddLinkToWorkitemToOriginalIssue(Workitem item, ServerConnector.Entities.Workitem newWorkitem) {
            if(item.ExternalLink == null) {
                return;
            }

            var url = item.ExternalLink.Url;
            var urlTitle = item.ExternalLink.Title;
            var title = !string.IsNullOrEmpty(urlTitle) ? urlTitle : url;
            var link = new Link(url, title, true);

            v1Processor.AddLinkToEntity(newWorkitem, link);
        }

        private string FindProperProjectToken(string projectId, string projectName) {
            if (!string.IsNullOrEmpty(projectId)) {
                return projectId;
            } 
            
            if (!string.IsNullOrEmpty(projectName)) {
                var projectToken = v1Processor.GetProjectTokenByName(projectName);

                if(projectToken != null) {
                    return projectToken;
                } 

                logger.MaybeLog(LogMessage.SeverityType.Info, string.Format("Could not assign to project with ID '{0}'. Used first accessible project instead.", projectId));
                projectToken = v1Processor.GetRootProjectToken();
                
                if(projectToken == null) {
                    logger.MaybeLog(LogMessage.SeverityType.Error, "Could not resolve VersionOne project token or fallback to root project.");
                } else {
                    return projectToken;
                }
            }

            throw new InvalidOperationException("Cannot resolve VersionOne project token");
        }
    }
}