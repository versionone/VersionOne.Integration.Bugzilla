using System;

namespace VersionOne.Bugzilla.XmlRpcProxy {
    // TODO apply in XML-RPC calls to handle fault responses
    public class BugzillaException : Exception {
        public BugzillaException(string message, Exception innerException) : base(message, innerException) { }
    }
}