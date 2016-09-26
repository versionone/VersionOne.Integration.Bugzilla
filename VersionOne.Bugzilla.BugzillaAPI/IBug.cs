using System;

namespace VersionOne.Bugzilla.BugzillaAPI
{
    public interface IBug {
        string GetReassignBugPayload(string integrationUserToken);
        string AssignedTo { get; set; }
    }
}