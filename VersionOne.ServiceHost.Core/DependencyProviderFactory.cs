using Ninject;

namespace VersionOne.ServiceHost.Core {
    public abstract class DependencyProviderFactory {
        [Inject]
        public IDependencyInjector DependencyInjector { get; set; }

        protected T Prepare<T>(T item) {
            DependencyInjector.Inject(item);
            return item;
        }
    }
}