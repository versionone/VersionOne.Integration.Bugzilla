using System;

namespace VersionOne.Bugzilla.BugzillaAPI
{
    public interface IBug {
        string GetReasignBugPayload(string integrationUserToken);
        string AssignedTo { get; set; }
    }
}