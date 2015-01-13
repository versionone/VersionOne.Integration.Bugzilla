using System;
namespace VersionOne.ServiceHost.ConfigurationTool.DL.Exceptions {
    public class VersionOneException : Exception {
        public VersionOneException(string message) : base(message) {
        }

        public VersionOneException(string message, Exception exception) : base(message, exception) {            
        }
    }
}