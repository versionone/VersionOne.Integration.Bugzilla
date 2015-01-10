using VersionOne.ServiceHost.Core.Logging;

namespace VersionOne.ServerConnector.StartupValidation {
    public class V1ConnectionValidator : BaseValidator {
        public override bool Validate() {
            Log(LogMessage.SeverityType.Info, "Validating connection to VersionOne");
            V1Processor.LogConnectionConfiguration();

            if(!V1Processor.ValidateConnection()) {
                Log(LogMessage.SeverityType.Error, "Cannot establish connection to VersionOne");
                return false;
            }

            V1Processor.LogConnectionInformation();
            Log(LogMessage.SeverityType.Info, "Connection to VersionOne is established successfully");
            return true;
        }
    }
}