using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using VersionOne.ServiceHost.Core.Services;

namespace VersionOne.ServiceHost {
    public class ServicesConfigurationHandler : IConfigurationSectionHandler {
        public object Create(object parent, object configContext, XmlNode section) {
            return new ServicesConfiguration(section);
        }
    }

    public class ServiceInfo {
        public readonly string Name;
        public readonly XmlElement Config;
        public readonly IHostedService Service;

        public ServiceInfo(string name, IHostedService svc, XmlElement config) {
            Name = name;
            Service = svc;
            Config = config;
        }
    }

    internal class ServicesConfiguration : List<ServiceInfo> {
        public ServicesConfiguration(XmlNode section) {
            foreach (XmlNode child in section.ChildNodes) {
                if(child.NodeType != XmlNodeType.Element)
                    continue;

                var disabledAttribute = child.Attributes["disabled"];
                
                if(disabledAttribute != null && (disabledAttribute.Value == "1" || disabledAttribute.Value.ToLower() == "true")) {
                    continue;
                }

                var attrib = child.Attributes["class"];
                
                if(attrib == null) {
                    continue;
                }

                var type = Type.GetType(attrib.Value);

                try {
                    var svc = (IHostedService) Activator.CreateInstance(type);
                    Add(new ServiceInfo(child.LocalName, svc, (XmlElement) child));
                    Console.WriteLine("Loaded {0}.", attrib.Value);
                } catch (Exception ex) {
                    Console.WriteLine("Failed to load {0}.{1}{2}", attrib.Value, Environment.NewLine, ex);
                }
            }
        }
    }
}