using VersionOne.ServiceHost.Core.Logging;

namespace VersionOne.Bugzilla.BugzillaAPI
{
    public class BugzillaClientFactory : IBugzillaClientFactory
    {
        private readonly IBugzillaClientConfiguration _bugzillaClientConfiguration;
        private readonly ILogger _logger;

        public BugzillaClientFactory(IBugzillaClientConfiguration bugzillaClientConfiguration, ILogger logger)
        {
            _bugzillaClientConfiguration = bugzillaClientConfiguration;
            _logger = logger;
        }
        
        public IBugzillaClient CreateNew()
        {
            return new BugzillaClient(_bugzillaClientConfiguration, _logger);
        }
    }
}