using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VersionOne.Bugzilla.BugzillaAPI
{
	public class Bug
	{
		public string ID { get; set; }
		public string Name { get; set; }
		public string Priority { get; set; }
		public string Product { get; set; }
		public string ComponentID { get; set; }
		public string AssignedTo { get; set; }
        public string Description { get; set; }
        
    }
}