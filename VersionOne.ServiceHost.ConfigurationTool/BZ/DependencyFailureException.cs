using System;

namespace VersionOne.ServiceHost.ConfigurationTool.BZ {
    public class DependencyFailureException : BusinessException {
        public readonly Type DependencyType;
        public readonly string DependencyKey;

        public DependencyFailureException(Type dependencyType, string key) {
            DependencyType = dependencyType;
            DependencyKey = key;
        }

        public DependencyFailureException(Type dependencyType, string key, string message) : base(message) {
            DependencyType = dependencyType;
            DependencyKey = key;
        }
    }
}