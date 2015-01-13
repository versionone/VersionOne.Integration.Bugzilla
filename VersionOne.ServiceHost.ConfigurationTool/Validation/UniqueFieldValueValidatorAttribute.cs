using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace VersionOne.ServiceHost.ConfigurationTool.Validation {
    [AttributeUsage(AttributeTargets.Property)]
    public class UniqueFieldValueValidatorAttribute : ValueValidatorAttribute {
        private readonly string[] fieldsToCompare;

        public UniqueFieldValueValidatorAttribute(params string[] fieldsToCompare) {
            this.fieldsToCompare = fieldsToCompare;
        }

        protected override Validator DoCreateValidator(Type targetType) {
            throw new InvalidOperationException("Don't use this method.");
        }

        protected override Validator DoCreateValidator(Type targetType, Type ownerType, MemberValueAccessBuilder memberValueAccessBuilder) {
            var fieldValueAccesses = new List<ValueAccess>();

            foreach(var field in fieldsToCompare) {
                var propertyInfo = ValidationReflectionHelper.GetProperty(ownerType, field);

                if(propertyInfo == null) {
                    throw new InvalidOperationException();
                }

                fieldValueAccesses.Add(memberValueAccessBuilder.GetPropertyValueAccess(propertyInfo));
            }

            return new UniqueFieldValueValidator(fieldValueAccesses);
        }
    }
}