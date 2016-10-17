namespace VersionOne.Bugzilla.BugzillaAPI
{
    public class BugzillaClientConfiguration : IBugzillaClientConfiguration
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }
    }
}