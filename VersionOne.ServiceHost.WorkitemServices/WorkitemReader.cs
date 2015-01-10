using System.Collections.Generic;
using VersionOne.ServerConnector;
using VersionOne.ServerConnector.Entities;
using VersionOne.ServerConnector.Filters;
using VersionOne.ServiceHost.Core.Logging;

namespace VersionOne.ServiceHost.WorkitemServices {
    public class WorkitemReader : IWorkitemReader {
        private readonly WorkitemWriterServiceConfiguration configuration;
        private readonly ILogger logger;
        private readonly IVersionOneProcessor v1Processor;

        public WorkitemReader(WorkitemWriterServiceConfiguration configuration, ILogger logger, IVersionOneProcessor v1Processor) {
            this.configuration = configuration;
            this.logger = logger;
            this.v1Processor = v1Processor;
        }

        public IList<ServerConnector.Entities.Workitem> GetDuplicates(Workitem item) {
            var emptyDuplicateList = new List<ServerConnector.Entities.Workitem>();

            if(string.IsNullOrEmpty(item.ExternalSystemName) || string.IsNullOrEmpty(configuration.ExternalIdFieldName)) {
                return emptyDuplicateList;
            }

            try {
                var filter = GroupFilter.And(
                    Filter.Equal(Entity.SourceNameProperty, item.ExternalSystemName),
                    Filter.Equal(configuration.ExternalIdFieldName, item.ExternalId),
                    Filter.OfTypes(VersionOneProcessor.PrimaryWorkitemType)
                );

                var duplicates = v1Processor.GetWorkitems(item.Type, filter);
                return duplicates;
            } catch (VersionOneException ex) {
                logger.Log(LogMessage.SeverityType.Error, string.Format("Can't get duplicates for {0} {1}. Cause:\n{2}", item.Number, item.Type, ex.Message));
            }

            return emptyDuplicateList;
        }
    }
}