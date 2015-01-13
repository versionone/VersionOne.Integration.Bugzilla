using System;

namespace VersionOne.ServiceHost.ConfigurationTool.DL.Exceptions {
    public class AssetTypeException : VersionOneException {
        public AssetTypeException(string message) : base(message) {
        }

        public AssetTypeException(string message, Exception exception) : base(message, exception) {            
        }
    }
}