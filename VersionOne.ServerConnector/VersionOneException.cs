using System;

namespace VersionOne.ServerConnector {
    public class VersionOneException : Exception {
        public VersionOneException(string message) : base(message) { }
    }
}