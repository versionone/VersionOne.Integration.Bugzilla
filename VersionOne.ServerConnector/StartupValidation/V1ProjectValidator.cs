using VersionOne.ServiceHost.Core.Logging;

namespace VersionOne.ServerConnector.StartupValidation {
    public class V1ProjectValidator : BaseValidator {
        private readonly string projectToken;

        public V1ProjectValidator(string projectToken) {
            this.projectToken = projectToken;
        }
        
        public override bool Validate() {
            Log(LogMessage.SeverityType.Info, "Checking VersionOne project");

            if(!V1Processor.ProjectExists(projectToken)) {
                Log(LogMessage.SeverityType.Error, string.Format("VersionOne project with '{0}' id doesn't exist", projectToken));
                return false;
            }

            Log(LogMessage.SeverityType.Info, string.Format("VersionOne project with '{0}' id successfully found", projectToken));
            return true;
        }
    }
}