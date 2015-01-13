using System.Xml.Serialization;
using VersionOne.ServiceHost.ConfigurationTool.Validation;

namespace VersionOne.ServiceHost.ConfigurationTool.Entities {
    public class BugzillaPriorityMapping {
        public const string VersionOnePriorityIdProperty = "VersionOnePriorityId";
        public const string BugzillaPriorityNameProperty = "BugzillaPriorityName";

        public BugzillaPriorityMapping() {
            BugzillaPriority = new Mapping();
            VersionOnePriority = new Mapping();
        }

        public Mapping BugzillaPriority { get; set; }
        public Mapping VersionOnePriority { get; set; }

        [XmlIgnore]
        [NonEmptyStringValidator]
        public string VersionOnePriorityId {
            get { return VersionOnePriority.Id; }
            set { VersionOnePriority.Id = value; }
        }

        [XmlIgnore]
        [NonEmptyStringValidator]
        public string BugzillaPriorityName {
            get { return BugzillaPriority.Name; }
            set { BugzillaPriority.Name = value; }
        }
    }
}