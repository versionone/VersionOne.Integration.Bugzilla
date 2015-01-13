using System;

namespace VersionOne.ServiceHost.ConfigurationTool.UI.Interfaces {
    // TODO remove this interface
    public interface IFormController : ISettingsProvider {
        void SetCoreServiceNodesEnabled(bool enabled);
        void FailApplication(string message);

        event EventHandler BeforeSave;
    }
}