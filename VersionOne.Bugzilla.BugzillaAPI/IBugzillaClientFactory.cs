using VersionOne.ServiceHost.Core.Logging;

namespace VersionOne.Bugzilla.BugzillaAPI
{
    public interface IBugzillaClientFactory
    {
        IBugzillaClient CreateNew(string url, ILogger logger);
    }
}
