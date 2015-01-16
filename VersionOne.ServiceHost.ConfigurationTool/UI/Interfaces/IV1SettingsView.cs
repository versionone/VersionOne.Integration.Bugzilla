using VersionOne.ServiceHost.ConfigurationTool.Entities;

namespace VersionOne.ServiceHost.ConfigurationTool.UI.Interfaces {
    public interface IV1SettingsView {
        bool TestsEnabled { get; set; }
        bool DefectsEnabled { get; set; }
        bool ChangesetsEnabled { get; set; }

        bool IsValid { get; }

        VersionOneConfiguration ConfigurationEntity { get; set; }

        void DataBind();
    }
}
