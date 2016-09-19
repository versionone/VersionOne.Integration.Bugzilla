using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace VersionOne.Bugzilla.BugzillaAPI
{
	public class Bug
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