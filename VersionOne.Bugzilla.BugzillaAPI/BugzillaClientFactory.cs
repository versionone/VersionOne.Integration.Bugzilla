using VersionOne.ServiceHost.Core.Logging;

namespace VersionOne.Bugzilla.BugzillaAPI
{
    public class BugzillaClientFactory : IBugzillaClientFactory
    {
        public IBugzillaClient CreateNew(string url, ILogger logger)
        {
            return new BugzillaClient(url, logger);
        }
    }
}