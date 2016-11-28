using System.Xml.Serialization;

namespace VersionOne.ServiceHost.Core.Configuration
{
    public enum AuthenticationTypes
    {
        [XmlEnum("0")]
        AccessToken = 0
    }
}