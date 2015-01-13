using System;

namespace VersionOne.ServiceHost.ConfigurationTool.BZ {
    public class AssemblyLoadException : BusinessException {
        public AssemblyLoadException(string message) : base(message) { }
        public AssemblyLoadException(string message, Exception innerException) : base(message, innerException) { } 
    }
}
