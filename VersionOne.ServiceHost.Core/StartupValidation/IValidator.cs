namespace VersionOne.ServiceHost.Core.StartupValidation {
    public interface IValidator<T> {
        ValidationResults<T> Validate();
    }
}