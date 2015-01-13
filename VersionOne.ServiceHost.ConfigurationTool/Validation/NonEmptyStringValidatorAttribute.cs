using System;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace VersionOne.ServiceHost.ConfigurationTool.Validation {
    [AttributeUsage(AttributeTargets.Property)]
    public class NonEmptyStringValidatorAttribute : ValidatorAttribute {
        private readonly string messageTemplate;

        public NonEmptyStringValidatorAttribute() { }

        public NonEmptyStringValidatorAttribute(string messageTemplate) {
            this.messageTemplate = messageTemplate;
        }

        protected override Validator DoCreateValidator(Type targetType) {
            return new NonEmptyStringValidator(messageTemplate);
        }
    }
}