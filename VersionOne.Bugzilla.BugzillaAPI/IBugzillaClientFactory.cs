﻿namespace VersionOne.Bugzilla.BugzillaAPI
{
    public interface IBugzillaClientFactory
    {
        IBugzillaClient CreateNew(string url);
    }
}
