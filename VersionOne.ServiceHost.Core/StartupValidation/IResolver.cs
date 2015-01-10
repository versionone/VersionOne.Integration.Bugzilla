using System.Collections.Generic;

namespace VersionOne.ServiceHost.Core.StartupValidation {
    public interface IResolver<T> {
        bool Resolve(IList<T> target);
    }
}