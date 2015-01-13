using System;
using System.Windows.Forms;
using System.Drawing;

namespace VersionOne.ServiceHost.ConfigurationTool.UI.Controls {
    public partial class HelpButton : UserControl {
        private readonly Control relatedControl;
        private readonly HelpProvider helpProvider;

        public HelpButton(Control relatedControl, HelpProvider helpProvider) {
            this.relatedControl = relatedControl;
            this.helpProvider = helpProvider;

            InitializeComponent();

            btnHelp.Click += btnHelp_Click;
        }

        private void btnHelp_Click(object sender, EventArgs e) {
            var helpString = helpProvider.GetHelpString(relatedControl);
            var location = relatedControl.PointToScreen(Point.Empty);
            Help.ShowPopup(relatedControl, helpString, location);
        }
    }
}