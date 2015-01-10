using System.Configuration;
using System.Xml;

namespace VersionOne.ServiceHost.Core.Configuration {
    public class InstallerConfiguration {
        public readonly string ShortName;
        public readonly string LongName;

        public InstallerConfiguration(XmlNode section) {
            var shortnode = section["ShortName"];

            if (shortnode == null) {
                throw new ConfigurationErrorsException("Missing Short Name Element", section);
            }

            ShortName = shortnode.InnerText;
            var longnode = section["LongName"];

            if (longnode == null) {
                throw new ConfigurationErrorsException("Missing Long Name Element", section);
            }

            LongName = longnode.InnerText;
        }
    }
}