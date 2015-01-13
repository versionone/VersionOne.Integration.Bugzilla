using System;

namespace VersionOne.ServiceHost.ConfigurationTool.Attributes {
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class DependsOnServiceAttribute : Attribute {
        private readonly Type serviceType;

        public Type ServiceType {
            get { return serviceType; }
        }

        public DependsOnServiceAttribute(Type serviceType) {
            if (serviceType == null) {
                throw new ArgumentNullException("serviceType");
            }

            this.serviceType = serviceType;
        }
    }
}