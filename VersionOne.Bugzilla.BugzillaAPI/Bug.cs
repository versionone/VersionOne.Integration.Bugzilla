using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace VersionOne.Bugzilla.BugzillaAPI
{
	public class Bug : IBug
	{
		public string ID { get; set; }
		public string Name { get; set; }
		public string Priority { get; set; }
		public string Product { get; set; }
        public int ProductId { get; set; }
        public string ComponentID { get; set; }
		public string AssignedTo { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public List<JToken> DependesOn { get; set; }
        public string IsOpen { get; set; }

	    public string GetReassignBugPayload(string integrationUserToken)
	    {
            JObject reassignBugPayload = new JObject();

	        reassignBugPayload["assigned_to"] = AssignedTo;
	        reassignBugPayload["status"] = BugzillaAPI.Status.CONFIRMED.ToString();
	        reassignBugPayload["token"] = integrationUserToken;
            
            return reassignBugPayload.ToString();
	    }

	}

    public enum Status
    {   
        CONFIRMED,
        RESOLVED,
        IN_PROGRESS,
        VERIFIED,
        UNCONFIRMED
    }

    public enum Resolution
    {
        FIXED,
        WONTFIX,
        INVALID,
        WORKSFORME,
        DUPLICATE
    }
}