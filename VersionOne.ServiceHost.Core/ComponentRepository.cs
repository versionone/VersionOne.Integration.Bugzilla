using System;
using System.Collections.Generic;
using System.Linq;

namespace VersionOne.ServiceHost.Core {
    [Obsolete("Replace with NInject ASAP")]
    public class ComponentRepository {
        private static ComponentRepository instance;
        private readonly IList<object> components = new List<object>();

        private ComponentRepository() { }

        public static ComponentRepository Instance {
            get { return instance ?? (instance = new ComponentRepository()); }
        }

        public T Resolve<T>() where T : class {
            return (T) components.FirstOrDefault(item => item is T);
        }

        public void Register<T>(T component) where T : class {
            if(component == null) {
                throw new ArgumentNullException("component");
            }

            var existing = components.FirstOrDefault(item => item is T);

            if(existing != null) {
                var index = components.IndexOf(existing);
                components[index] = component;
            } else {
                components.Add(component);
            }
        }

        public void Unregister<T>(T component) where T : class {
            if(components.Any(item => item == component)) {
                components.Remove(component);
            }
        }
    }
}