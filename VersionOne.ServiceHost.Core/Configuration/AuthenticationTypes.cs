using System.Xml.Serialization;

namespace VersionOne.ServiceHost.Core.Configuration
{
    public enum AuthenticationTypes
    {
        [XmlEnum("0")]
        AccessToken = 0,
        [XmlEnum("1")]
        Basic,
        [XmlEnum("2")]
        Integrated,
        [XmlEnum("3")]
        IntegratedWithCredentials
    }
}