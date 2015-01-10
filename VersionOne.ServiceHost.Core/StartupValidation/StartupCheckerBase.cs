using System.Collections.Generic;
using VersionOne.ServiceHost.Eventing;

namespace VersionOne.ServiceHost.Core.StartupValidation {
    public abstract class StartupCheckerBase {
        private readonly IEventManager eventManager;
        private readonly IDependencyInjector dependencyInjector;

        protected StartupCheckerBase(IEventManager eventManager, IDependencyInjector dependencyInjector) {
            this.eventManager = eventManager;
            this.dependencyInjector = dependencyInjector;
        }

        public void Initialize() {
            eventManager.Subscribe(typeof(ServiceHostState), Run);
        }

        public void Run(object obj) {
            if(obj == null || !ServiceHostState.Validate.Equals(obj)) {
                return;
            }

            var steps = CreateValidators();

            foreach(var step in steps) {
                dependencyInjector.Inject(step);
                step.Run();
            }

            Complete();
        }

        protected abstract IEnumerable<IValidationStep> CreateValidators();

        protected virtual void Complete() {}
    }
}