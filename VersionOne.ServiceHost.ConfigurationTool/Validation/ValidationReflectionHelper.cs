using System;
using System.Globalization;
using System.Reflection;

namespace VersionOne.ServiceHost.ConfigurationTool.Validation {
    internal static class ValidationReflectionHelper {
        public static PropertyInfo GetProperty(Type type, string propertyName) {
            if(string.IsNullOrEmpty(propertyName)) {
                throw new ArgumentNullException("propertyName");
            }

            var propertyInfo = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

            if(!IsValidProperty(propertyInfo)) {
                throw new ArgumentException("Invalid property");
            }

            return propertyInfo;
        }

        public static bool IsValidProperty(PropertyInfo propertyInfo) {
            return null != propertyInfo && propertyInfo.CanRead;
        }

        public static FieldInfo GetField(Type type, string fieldName, bool throwIfInvalid) {
            if(string.IsNullOrEmpty(fieldName)) {
                throw new ArgumentNullException("fieldName");
            }

            var fieldInfo = type.GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);

            if(!IsValidField(fieldInfo)) {
                throw new ArgumentException("Invalid field");
            }

            return fieldInfo;
        }

        public static bool IsValidField(FieldInfo fieldInfo) {
            return null != fieldInfo;
        }

        public static MethodInfo GetMethod(Type type, string methodName) {
            if(string.IsNullOrEmpty(methodName)) {
                throw new ArgumentNullException("methodName");
            }

            var methodInfo = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);

            if(!IsValidMethod(methodInfo)) {
                    throw new ArgumentException("Invalid method");
            }

            return methodInfo;
        }

        public static bool IsValidMethod(MethodInfo methodInfo) {
            return null != methodInfo
              && typeof(void) != methodInfo.ReturnType
              && methodInfo.GetParameters().Length == 0;
        }
    }
}