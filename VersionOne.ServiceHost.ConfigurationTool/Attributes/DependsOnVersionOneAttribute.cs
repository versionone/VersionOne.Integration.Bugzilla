using System;

namespace VersionOne.ServiceHost.ConfigurationTool.Attributes {
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class DependsOnVersionOneAttribute : Attribute { }
}
