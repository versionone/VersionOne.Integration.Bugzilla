using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace VersionOne.ServiceHost.ConfigurationTool.Validation {
    public class UniqueFieldValueValidator : ValueValidator {
        private readonly IList<ValueAccess> valueAccesses;

        public UniqueFieldValueValidator(List<ValueAccess> valueAccesses) : base(null, null, false) {
            this.valueAccesses = valueAccesses;
        }

        protected override string DefaultNonNegatedMessageTemplate {
            get { return "Value must be unique among all others"; }
        }

        protected override string DefaultNegatedMessageTemplate {
            get { return "Value must not be unique among all others"; }
        }

        protected override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults) {
            foreach(var valueAccess in valueAccesses) {
                object comparand;
                string valueAccessFailureMessage;

                var status = valueAccess.GetValue(currentTarget, out comparand, out valueAccessFailureMessage);

                if(!status) {
                    LogValidationResult(validationResults, "Failed to retrieve field comparand value", currentTarget, key);
                }

                if(string.Equals(objectToValidate, comparand)) {
                    LogValidationResult(validationResults, DefaultNonNegatedMessageTemplate, currentTarget, key);
                }
            }
        }
    }
}