using System.Xml.Serialization;
using VersionOne.ServiceHost.ConfigurationTool.Attributes;
using VersionOne.ServiceHost.ConfigurationTool.Validation;

namespace VersionOne.ServiceHost.ConfigurationTool.Entities {
    /// <summary>
    /// Link settings/node backing class. These settings control how Links are created on V1 server.
    /// </summary>
    [XmlRoot("Link")]
    public class ChangesetLink {
        public const string NameProperty = "Name";
        public const string UrlProperty = "Url";
        public const string OnMenuProperty = "OnMenu";

        [NonEmptyStringValidator]
        [HelpString(HelpResourceKey = "ChangesetsName")]
        public string Name { get; set; }

        [NonEmptyStringValidator]
        [XmlElement("URL")]
        [HelpString(HelpResourceKey = "ChangesetsUrl")]
        public string Url { get; set; }

        [HelpString(HelpResourceKey = "ChangesetsAddToMenu")]
        public NullableBool OnMenu { get; set; }

        public ChangesetLink() {
            OnMenu = new NullableBool();
        }

        public override bool Equals(object obj) {
            if(obj == null || obj.GetType() != GetType()) {
                return false;
            }

            var link = (ChangesetLink)obj;

            return OnMenu == link.OnMenu && string.Equals(Name, link.Name) && string.Equals(Url, link.Url);
        }

        public static bool Equals(ChangesetLink a, ChangesetLink b) {
            if(a == null) {
                return b == null;
            }

            return a.Equals(b);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}