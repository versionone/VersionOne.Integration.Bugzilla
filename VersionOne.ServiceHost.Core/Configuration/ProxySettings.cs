using System.Xml.Serialization;

namespace VersionOne.ServiceHost.Core.Configuration {
    public class ProxySettings {
        [XmlIgnore]
        public bool Enabled { get; set; }

        [XmlAttribute("disabled")]
        public int Disabled {
            get { return Enabled ? 0 : 1; }
            set { Enabled = value == 0; }
        }

        public string Url { get; set; }

        [XmlElement("UserName")]
        public string Username { get; set; }

        public string Password { get; set; }
        public string Domain { get; set; }
    }
}