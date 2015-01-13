using System.Xml.Serialization;
using VersionOne.ServiceHost.ConfigurationTool.Validation;

namespace VersionOne.ServiceHost.ConfigurationTool.Entities {
    public class BugzillaProjectMapping {
        public const string VersionOneProjectTokenProperty = "VersionOneProjectToken";
        public const string BugzillaProjectNameProperty = "BugzillaProjectName";

        public BugzillaProjectMapping() {
            BugzillaProject = new Mapping();
            VersionOneProject = new Mapping();
        }

        public Mapping BugzillaProject { get; set; }
        public Mapping VersionOneProject { get; set; }

        [XmlIgnore]
        [NonEmptyStringValidator]
        public string VersionOneProjectToken {
            get { return VersionOneProject.Id; }
            set { VersionOneProject.Id = value; }
        }

        [XmlIgnore]
        [NonEmptyStringValidator]
        public string BugzillaProjectName {
            get { return BugzillaProject.Name; }
            set { BugzillaProject.Name = value; }
        }
    }
}