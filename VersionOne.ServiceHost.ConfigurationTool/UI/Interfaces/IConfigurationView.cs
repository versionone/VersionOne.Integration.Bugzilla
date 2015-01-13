using System.Collections.Generic;
using System.Windows.Forms;

namespace VersionOne.ServiceHost.ConfigurationTool.UI.Interfaces {
    public interface IConfigurationView {
        bool NewFileMenuItemEnabled { get; set; }
        bool OpenFileMenuItemEnabled { get; set; }
        bool SaveFileMenuItemEnabled { get; set; }
        bool SaveFileAsMenuItemEnabled { get; set; }
        bool OptionsMenuItemEnabled { get; set; }
        bool GenerateSnapshotMenuItemEnabled { get; set; }

        void SetServiceNodesAndRedraw(IEnumerable<string> coreServiceNodes, IEnumerable<string> customServiceNodes);
        void SetCoreServiceNodesEnabled(bool enabled);
        void ShowErrorMessage(string message);

        string HeaderText { get; set; }
        Control CurrentControl { get; set; }

        IConfigurationController Controller { get; set; }
    }
}
