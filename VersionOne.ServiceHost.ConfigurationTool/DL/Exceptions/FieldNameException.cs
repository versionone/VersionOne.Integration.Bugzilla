using System;

namespace VersionOne.ServiceHost.ConfigurationTool.DL.Exceptions {
    public class FieldNameException : VersionOneException {
        public FieldNameException(string message) : base(message) {
        }

        public FieldNameException(string message, Exception exception) : base(message, exception) {            
        }
    }
}