using System.Linq;
using System.Reflection;
using Ninject;

namespace VersionOne.ServiceHost.Core {
    public class DependencyInjector : IDependencyInjector {
        private readonly IKernel container;

        public DependencyInjector(IKernel container) {
            this.container = container;
        }

        public void Inject(object consumer) {
            DoInject(consumer);

            var properties = consumer.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            properties.Where(property => property.GetCustomAttributes(typeof (HasDependenciesAttribute), true).Any())
                .Select(x => x.GetValue(consumer, new object[0]))
                .Where(x => x != null)
                .ToList()
                .ForEach(DoInject);
        }

        private void DoInject(object consumer) {
            container.Inject(consumer);
        }
    }
}