using System;

namespace VersionOne.ServiceHost.ConfigurationTool.BZ {
    public class BusinessException : Exception {
        protected BusinessException() { }
        public BusinessException(string message) : base(message) { }
        public BusinessException(string message, Exception innerException) : base(message, innerException) { }
    }
}
