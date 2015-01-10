using Ninject;
using VersionOne.ServiceHost.Core.Logging;

namespace VersionOne.ServiceHost.Core.StartupValidation {
    public interface IBaseValidationEntity {
        [Inject]
        ILogger Logger { get; set; }

        bool TreatErrorsAsWarnings { get; set; }
    }
}