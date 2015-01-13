using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace VersionOne.ServiceHost.ConfigurationTool.Validation {
    internal class PropertyValueAccess : ValueAccess {
        private readonly PropertyInfo propertyInfo;
        
        public override string Key {
            get { return propertyInfo.Name; }
        }

        internal PropertyValueAccess(PropertyInfo propertyInfo) {
            this.propertyInfo = propertyInfo;
        }
        
        public override bool GetValue(object source, out object value, out string valueAccessFailureMessage) {
            valueAccessFailureMessage = null;

            var proxy = (GridValidationIntegrationProxy) source;
            value = proxy.GetRawValue();

            return true;
        }
    }
}