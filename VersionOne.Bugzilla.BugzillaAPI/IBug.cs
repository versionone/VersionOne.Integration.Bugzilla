using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace VersionOne.Bugzilla.BugzillaAPI
{
    public interface IBug {
        string ID { get; }
        string Name { get; }
        string Priority { get; }
        string Product { get; }
        int ProductId { get; set; }
        string Component { get; }
        string AssignedTo { get; set; }
        string Description { get; }
        string Status { get; }
        IEnumerable<int> DependesOn { get; }
        bool IsOpen { get; }
        string GetReassignBugPayload(string integrationUserToken);
    }
}