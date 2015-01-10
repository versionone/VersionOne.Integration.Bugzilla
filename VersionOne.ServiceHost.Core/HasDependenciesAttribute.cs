using System;

namespace VersionOne.ServiceHost.Core {
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class HasDependenciesAttribute : Attribute { }
}