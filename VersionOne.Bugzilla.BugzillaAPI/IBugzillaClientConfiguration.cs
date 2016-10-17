namespace VersionOne.Bugzilla.BugzillaAPI
{
    public interface IBugzillaClientConfiguration
    {
        string UserName { get; set; }
        string Password { get; set; }
        string Url { get; set; }
    }
}