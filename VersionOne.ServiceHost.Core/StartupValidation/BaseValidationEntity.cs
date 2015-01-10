using System;
using Ninject;
using VersionOne.ServiceHost.Core.Logging;

namespace VersionOne.ServiceHost.Core.StartupValidation {
    public class BaseValidationEntity : IBaseValidationEntity {
        [Inject]
        public ILogger Logger { get; set; }

        public bool TreatErrorsAsWarnings { get; set; }

        protected void Log(LogMessage.SeverityType severity, string message) {
            var resultingSeverity = ResolveSeverity(severity);
            Logger.Log(resultingSeverity, message);
        }

        protected void Log(LogMessage.SeverityType severity, string message, Exception ex) {
            var resultingSeverity = ResolveSeverity(severity);
            Logger.Log(resultingSeverity, message, ex);
        }

        private LogMessage.SeverityType ResolveSeverity(LogMessage.SeverityType originalSeverity) {
            return TreatErrorsAsWarnings && originalSeverity == LogMessage.SeverityType.Error
                ? LogMessage.SeverityType.Warning
                : originalSeverity;
        }
    }
}