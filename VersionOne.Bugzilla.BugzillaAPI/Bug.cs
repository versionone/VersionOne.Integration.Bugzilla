using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace VersionOne.Bugzilla.BugzillaAPI
{
	public class Bug : IBug
	{
	    public Bug(JToken bugData, string comment)
	    {
	        ID = bugData["id"].ToString();
	        Name = bugData["summary"].ToString();
	        AssignedTo = bugData["assigned_to"].ToString();
	        Component = bugData["component"].ToString();
	        Priority = bugData["priority"].ToString();
	        Product = bugData["product"].ToString();
	        Status = bugData["status"].ToString();
	        Description = comment;
	        IsOpen = (bool) bugData["is_open"];
	        DependesOn = bugData["depends_on"].ToList().Select(dependantId => (int) dependantId );

	    }

	    public string ID { get; }
		public string Name { get; }
		public string Priority { get; }
		public string Product { get; }
        public int ProductId { get; set; }
        public string Component { get; }
		public string AssignedTo { get; set; }
        public string Description { get; }
        public string Status { get; }
        public IEnumerable<int> DependesOn { get; }
        public bool IsOpen { get; }

        public string GetReassignBugPayload(string integrationUserToken)
	    {
            JObject reassignBugPayload = new JObject();

	        reassignBugPayload["assigned_to"] = AssignedTo;
	        reassignBugPayload["status"] = BugzillaAPI.Status.CONFIRMED.ToString();
	        reassignBugPayload["token"] = integrationUserToken;
            
            return reassignBugPayload.ToString();
	    }

	}
}