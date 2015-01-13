using System;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace VersionOne.ServiceHost.ConfigurationTool.Validation {
    internal class GridMemberValueAccessBuilder : MemberValueAccessBuilder {
        protected override ValueAccess DoGetFieldValueAccess(FieldInfo fieldInfo) {
            throw new NotSupportedException();
        }

        protected override ValueAccess DoGetMethodValueAccess(MethodInfo methodInfo) {
            throw new NotSupportedException();
        }

        protected override ValueAccess DoGetPropertyValueAccess(PropertyInfo propertyInfo) {
            return new PropertyValueAccess(propertyInfo);
        }
    }
}