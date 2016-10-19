using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace VersionOne.Bugzilla.BugzillaAPI
{
    public interface IBug {
        string ID { get; set; }
        string Name { get; set; }
        string Priority { get; set; }
        string Product { get; set; }
        int ProductId { get; set; }
        string ComponentID { get; set; }
        string AssignedTo { get; set; }
        string Description { get; set; }
        string Status { get; set; }
        List<JToken> DependesOn { get; set; }
        string IsOpen { get; set; }
        string GetReassignBugPayload(string integrationUserToken);
    }
}