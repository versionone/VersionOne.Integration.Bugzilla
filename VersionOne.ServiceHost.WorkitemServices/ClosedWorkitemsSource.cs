using VersionOne.ServerConnector;

namespace VersionOne.ServiceHost.WorkitemServices {
    public class ClosedWorkitemsSource {
        public readonly string BaseWorkitemType;
        public readonly string SourceValue;

        public ClosedWorkitemsSource(string sourceValue) : this(sourceValue, VersionOneProcessor.DefectType) { }

        public ClosedWorkitemsSource(string sourceValue, string baseWorkitemType) {
            SourceValue = sourceValue;
            BaseWorkitemType = baseWorkitemType;
        }
    }
}