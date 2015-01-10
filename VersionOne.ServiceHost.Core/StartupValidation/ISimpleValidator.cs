namespace VersionOne.ServiceHost.Core.StartupValidation {
    public interface ISimpleValidator : IBaseValidationEntity {
        bool Validate();
    }
}