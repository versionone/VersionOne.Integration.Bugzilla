using System.Xml.Serialization;
using VersionOne.ServiceHost.ConfigurationTool.Validation;

namespace VersionOne.ServiceHost.ConfigurationTool.Entities {
    [XmlRoot("V1Project")]
    public class TestPublishProjectMapping {
        public const string NameProperty = "Name";
        public const string IncludeChildrenProperty = "IncludeChildren";
        public const string DestinationProjectProperty = "DestinationProject";

        [NonEmptyStringValidator, XmlAttribute]
        public string Name { get; set; }

        [XmlIgnore]
        public bool IncludeChildren { get; set; }

        // TODO refactor TestReaderService
        /// <summary>
        /// Whether to include child projects
        /// This is supposed to represent a boolean value, "Y" for true and "N" (in fact, something not containing "Y") for false (by service design)
        /// </summary>
        [XmlAttribute("IncludeChildren")]
        public string IncludeChildrenString {
            get { return IncludeChildren ? "Y" : "N"; }
            set { IncludeChildren = value != null && value.ToUpperInvariant().Contains("Y"); }
        }

        [NonEmptyStringValidator, XmlText]
        public string DestinationProject { get; set; }

        public override bool Equals(object obj) {
            if (obj == null || obj.GetType() != typeof(TestPublishProjectMapping)) {
                return false;
            }

            var other = (TestPublishProjectMapping) obj;

            return string.Equals(Name, other.Name) && string.Equals(DestinationProject, other.DestinationProject)
                && IncludeChildren == other.IncludeChildren;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}