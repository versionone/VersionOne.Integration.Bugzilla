using Ninject;

namespace VersionOne.ServiceHost.Core.Services {
    public interface IComponentProvider {
        void RegisterComponents(IKernel container);
    }
}