using System.Collections.Generic;
using VersionOne.ServerConnector.StartupValidation;
using VersionOne.ServiceHost.Core;
using VersionOne.ServiceHost.Core.StartupValidation;
using VersionOne.ServiceHost.Eventing;

namespace VersionOne.ServiceHost.WorkitemServices {
    public class StartupChecker : StartupCheckerBase {
        public StartupChecker(IEventManager eventManager, IDependencyInjector dependencyInjector) : base(eventManager, dependencyInjector) { }

        protected override IEnumerable<IValidationStep> CreateValidators() {
            // TODO we have two V1 connection checks: here and in the "client" service (like JIRA). Remove ASAP
            return new List<IValidationStep> {
                new ValidationSimpleStep(new V1ConnectionValidator(), null),
            };            
        }
    }
}