using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using VersionOne.ServerConnector;
using VersionOne.ServerConnector.Entities;
using VersionOne.ServerConnector.Filters;
using VersionOne.ServiceHost.Core.Logging;
using WorkitemEntity = VersionOne.ServerConnector.Entities.Workitem;

namespace VersionOne.ServiceHost.WorkitemServices {
    public class ClosedExternalWorkitemQuerier {
        private readonly IVersionOneProcessor v1Processor;
        private readonly ILogger logger;
        private readonly WorkitemWriterServiceConfiguration configuration;

        public ClosedExternalWorkitemQuerier(IVersionOneProcessor v1Processor, ILogger logger, WorkitemWriterServiceConfiguration configuration) {
            this.v1Processor = v1Processor;
            this.logger = logger;
            this.configuration = configuration;
        }

        public WorkitemStateChangeCollection GetWorkitemsClosedSince(DateTime closedSince, string baseWorkitemType, string sourceName, string lastCheckedDefectId) {
            var results = new WorkitemStateChangeCollection();
            var lastChangedIdLocal = lastCheckedDefectId;
            var dateLastChange = closedSince;

            try {
                var workitems = GetClosedWorkitems(closedSince, baseWorkitemType, sourceName);

                foreach(var item in workitems) {
                    var id = item.Number;
                    var changeDateUtc = item.ChangeDateUtc;

                    logger.Log(LogMessage.SeverityType.Debug, string.Format("Processing V1 Defect {0} closed at {1}", id, changeDateUtc));

                    if(lastCheckedDefectId.Equals(id)) {
                        logger.Log(LogMessage.SeverityType.Debug, "\tSkipped because this ID was processed last time");
                        continue;
                    }

                    if (WorkitemHasOpenDuplicate(item)) {
                        logger.Log(LogMessage.SeverityType.Debug, "\tSkipped because the workitem has opened duplicate.");
                        continue;                        
                    }

                    if((dateLastChange == DateTime.MinValue && changeDateUtc != DateTime.MinValue) || changeDateUtc.CompareTo(dateLastChange) > 0) {
                        logger.Log(LogMessage.SeverityType.Debug, "\tCaused an update to LastChangeID and dateLastChanged");
                        dateLastChange = changeDateUtc;
                        lastChangedIdLocal = id;
                    }

                    results.Add(new WorkitemStateChangeResult(item.GetProperty<string>(configuration.ExternalIdFieldName), item.Number));
                }
            } catch(WebException ex) {
                ShowError(ex);
            }

            results.LastCheckedDefectId = lastChangedIdLocal;
            results.QueryTimeStamp = dateLastChange;

            return results;
        }

        private bool WorkitemHasOpenDuplicate(WorkitemEntity item) {
            var filter = Filter.Equal(configuration.ExternalIdFieldName, item.GetProperty<string>(configuration.ExternalIdFieldName));
            var workitems = v1Processor.GetWorkitems(item.TypeName, filter);
            return workitems.Any(workitem => !workitem.IsClosed);
        }

        protected virtual void ShowError(WebException ex) {
            if(ex.Response == null) {
                return;
            }
            
            using(var reader = new StreamReader(ex.Response.GetResponseStream())) {
                var responseMessage = reader.ReadToEnd();
                logger.Log(LogMessage.SeverityType.Error, 
                    string.Format("Error querying VersionOne ({0}) for closed external defects:{3}{1}{3}{3}{2}", ex.Response.ResponseUri, responseMessage, ex, Environment.NewLine));
            }
        }

        private IEnumerable<WorkitemEntity> GetClosedWorkitems(DateTime closedSince, string baseWorkitemType, string sourceName) {
            var filters = new List<IFilter> {
                Filter.Closed(true),
                Filter.OfTypes(VersionOneProcessor.StoryType, VersionOneProcessor.DefectType),
                Filter.Equal(Entity.SourceNameProperty, sourceName),
            };

            if(closedSince != DateTime.MinValue) {
                filters.Add(Filter.Greater(ServerConnector.Entities.Workitem.ChangeDateUtcProperty, closedSince));
            }

            var filter = GroupFilter.And(filters.ToArray());
            return v1Processor.GetWorkitems(baseWorkitemType, filter);
        }
    }
}