using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using VersionOne.ServiceHost.ConfigurationTool.Validation;
using VersionOne.ServiceHost.ConfigurationTool.Attributes;

namespace VersionOne.ServiceHost.ConfigurationTool.Entities {
    [XmlRoot("ProxySettings")]
    public class ProxyConnectionSettings {
        public readonly static string UriProperty = "Uri";
        public readonly static string UsernameProperty = "UserName";
        public readonly static string PasswordProperty = "Password";
        public readonly static string EnabledProperty = "Enabled";
        public readonly static string DomainProperty = "Domain";

        [NonEmptyStringValidator]
        public string Uri { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Domain { get; set; }

        [HelpString(HelpResourceKey = "V1PageProxyEnabled")]
        [XmlIgnore]
        public bool Enabled { get; set; }

        [XmlAttribute("disabled")]
        public int DisabledNumeric {
            get { return Convert.ToInt32(!Enabled); }
            set { Enabled = !Convert.ToBoolean(value); }
        }
    }
}