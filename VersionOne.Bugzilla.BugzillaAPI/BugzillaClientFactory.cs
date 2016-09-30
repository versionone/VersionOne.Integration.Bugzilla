namespace VersionOne.Bugzilla.BugzillaAPI
{
    public class BugzillaClientFactory : IBugzillaClientFactory
    {
        public IBugzillaClient CreateNew(string url)
        {
            return new BugzillaClient(url);
        }
    }
}