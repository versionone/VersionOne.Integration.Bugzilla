namespace VersionOne.ServiceHost.Core.StartupValidation {
    public interface ISimpleResolver : IBaseValidationEntity {
        bool Resolve();
    }
}