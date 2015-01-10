using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace VersionOne.ServiceHost.Core.Configuration {
    [XmlRoot("Settings")]
    public class VersionOneSettings {
        private const string DefaultApiVersion = "6.5.0.0";

        [XmlElement("ApplicationUrl")]
        public string Url { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        [XmlElement("APIVersion")]
        public string ApiVersion { get; set; }

        public bool IntegratedAuth { get; set; }
        public ProxySettings ProxySettings { get; set; }

        public VersionOneSettings() {
            ApiVersion = DefaultApiVersion;
            ProxySettings = new ProxySettings {Enabled = false, };
        }

        public XmlElement ToXmlElement() {
            var xmlSerializer = new XmlSerializer(GetType());
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            
            using (var memoryStream = new MemoryStream()) {
                try {
                    xmlSerializer.Serialize(memoryStream, this, namespaces);
                } catch (InvalidOperationException) {
                    return null;
                }

                memoryStream.Position = 0;
                var serializationDoc = new XmlDocument();
                serializationDoc.Load(memoryStream);
                return serializationDoc.DocumentElement;
            }
        }

        public static VersionOneSettings FromXmlElement(XmlElement element) {
            if(element == null) {
                throw new ArgumentNullException("element");
            }

            var xmlSerializer = new XmlSerializer(typeof(VersionOneSettings));
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            return (VersionOneSettings) xmlSerializer.Deserialize(new XmlNodeReader(element));
        }
    }
}