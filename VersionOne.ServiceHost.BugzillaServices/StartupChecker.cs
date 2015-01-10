using System.Collections.Generic;
using VersionOne.ServerConnector.StartupValidation;
using VersionOne.ServiceHost.Core.StartupValidation;
using VersionOne.ServiceHost.Eventing;

namespace VersionOne.ServiceHost.WorkitemServices {
    public class StartupChecker : StartupCheckerBase {
        public StartupChecker(IEventManager eventManager) : base(eventManager) { }

        protected override IEnumerable<IValidationStep> CreateValidators() {
            return new List<IValidationStep> {
                new ValidationSimpleStep(new V1ConnectionValidator(), null),
            };            
        }
    }
}