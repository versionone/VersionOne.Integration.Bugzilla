using VersionOne.ServiceHost.Core.Utility;

namespace VersionOne.ServiceHost.WorkitemServices {
    public class WorkitemWriterServiceConfiguration {
        [ConfigFileValue("ExternalIdFieldName")]
        public string ExternalIdFieldName;
    }
}