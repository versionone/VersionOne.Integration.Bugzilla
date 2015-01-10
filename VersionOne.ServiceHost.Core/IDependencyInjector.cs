namespace VersionOne.ServiceHost.Core {
    public interface IDependencyInjector {
        void Inject(object consumer);
    }
}