using System;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace VersionOne.ServiceHost.ConfigurationTool.Validation {
    [AttributeUsage(AttributeTargets.Property)]
    public class ConditionalNonEmptyStringValidatorAttribute : ValidatorAttribute {
        private readonly string messageTemplate;
        private readonly string conditionalPropertyName;
        private readonly bool skipOnFalse;

        public ConditionalNonEmptyStringValidatorAttribute(string conditionalPropertyName, bool skipOnFalse) {
            this.conditionalPropertyName = conditionalPropertyName;
            this.skipOnFalse = skipOnFalse;
        }

        public ConditionalNonEmptyStringValidatorAttribute(string messageTemplate, string conditionalPropertyName, bool skipOnFalse) : this(conditionalPropertyName, skipOnFalse) {
            this.messageTemplate = messageTemplate;
        }

        protected override Validator DoCreateValidator(Type targetType) {
            return new ConditionalNonEmptyStringValidator(messageTemplate, conditionalPropertyName, skipOnFalse);
        }
    }
}