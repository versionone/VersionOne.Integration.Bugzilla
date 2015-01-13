using System;

namespace VersionOne.ServiceHost.ConfigurationTool.BZ {
    public class InvalidFilenameException : BusinessException {
        public InvalidFilenameException(string message) : base(message) { }
        public InvalidFilenameException(string message, Exception innerException) : base(message, innerException) { } 
    }
}
