using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace VersionOne.ServiceHost.ConfigurationTool.Validation {
    public class NonEmptyStringValidator : Validator<string> {
        public NonEmptyStringValidator() : this(null) { }

        public NonEmptyStringValidator(string messageTemplate) : base(messageTemplate, null) { }

        protected override string DefaultMessageTemplate {
            get { return "Value can not be empty or contain only whitespace symbols"; }
        }

        protected override void DoValidate(string objectToValidate, object currentTarget, string key, ValidationResults validationResults) {
            if(objectToValidate == null || objectToValidate.Trim().Length < 1) {
                LogValidationResult(validationResults, MessageTemplate, currentTarget, key);
            }
        }
    }
}