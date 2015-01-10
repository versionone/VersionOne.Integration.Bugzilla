using VersionOne.ServiceHost.Core.Logging;

namespace VersionOne.ServerConnector.StartupValidation {
    public class MandatoryOrEmptyFieldValidator : BaseValidator {
        private readonly string fieldName;
        private readonly string description;
        private readonly string containingTypeToken; 

        public MandatoryOrEmptyFieldValidator(string fieldName, string description, string containingTypeToken) {
            this.fieldName = fieldName;
            this.description = description;
            this.containingTypeToken = containingTypeToken;
        }

        public override bool Validate() {
            Log(LogMessage.SeverityType.Info, string.Format("Checking {0} field...", description));

            if (string.IsNullOrEmpty(fieldName)) {
                Log(LogMessage.SeverityType.Warning, "Field name is omitted, corresponding functionality will not run.");
                return true;
            }

            if (!V1Processor.AttributeExists(containingTypeToken, fieldName)) {
                Log(LogMessage.SeverityType.Error, string.Format("Custom field {0} is not assigned to type {1}", fieldName, containingTypeToken));
                return false;
            }

            Log(LogMessage.SeverityType.Info, "Custom field check successful.");
            return true;
        }
    }
}