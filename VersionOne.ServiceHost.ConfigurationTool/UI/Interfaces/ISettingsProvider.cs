using VersionOne.ServiceHost.ConfigurationTool.Entities;

namespace VersionOne.ServiceHost.ConfigurationTool.UI.Interfaces {
    public interface ISettingsProvider {
        ServiceHostConfiguration Settings { get; }
    }
}
