using VersionOne.ServiceHost.ConfigurationTool.Entities;

namespace VersionOne.ServiceHost.ConfigurationTool.UI.Interfaces {
    public interface IVersionOneSettingsConsumer {
        VersionOneSettings Settings { get; set; }
    }
}
