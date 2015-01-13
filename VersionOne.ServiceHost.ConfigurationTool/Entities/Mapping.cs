using System.Xml.Serialization;

namespace VersionOne.ServiceHost.ConfigurationTool.Entities {
    public class Mapping {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlText]
        public string Name { get; set; }
    }
}