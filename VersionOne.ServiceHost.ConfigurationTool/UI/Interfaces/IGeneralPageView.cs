using System;
using VersionOne.ServiceHost.ConfigurationTool.Entities;

namespace VersionOne.ServiceHost.ConfigurationTool.UI.Interfaces {
    public interface IGeneralPageView : IPageView<ServiceHostConfiguration> {
        event EventHandler<ConnectionValidationEventArgs> ValidationRequested;

        void SetValidationResult(bool validationSuccessful);

        void SetProxyUrlValidationFault(bool validationSuccessful);
    }
}
