using VersionOne.ServiceHost.ConfigurationTool.Entities;

namespace VersionOne.ServiceHost.ConfigurationTool.UI.Controls {
    public partial class BugzillaAssignControl : BaseDataBindingControl {
        public BugzillaAssignControl() {
            InitializeComponent();
        }

        public void DataBind(BugzillaServiceEntity source) {
            AddControlBinding(txtCreateReassignValue, source, BugzillaServiceEntity.CreateReassignValueProperty);
            AddControlBinding(txtCloseReassignValue, source, BugzillaServiceEntity.CloseReassignValueProperty);

            AddHelpSupport(txtCreateReassignValue, source, BugzillaServiceEntity.CreateReassignValueProperty);
            AddHelpSupport(txtCloseReassignValue, source, BugzillaServiceEntity.CloseReassignValueProperty);
        }
    }
}