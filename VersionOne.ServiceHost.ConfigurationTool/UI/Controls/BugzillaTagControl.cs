using VersionOne.ServiceHost.ConfigurationTool.Entities;

namespace VersionOne.ServiceHost.ConfigurationTool.UI.Controls {
    public partial class BugzillaTagControl : BaseDataBindingControl {
        public BugzillaTagControl () {
            InitializeComponent();
        }

        public void DataBind (BugzillaServiceEntity source) {
            AddControlBinding(txtCreateFieldValue, source, BugzillaServiceEntity.CreateFieldValueProperty);
            AddControlBinding(txtCreateFieldId, source, BugzillaServiceEntity.CreateFieldIdProperty);
            AddControlBinding(txtCloseFieldValue, source, BugzillaServiceEntity.CloseFieldValueProperty);
            AddControlBinding(txtCloseFieldId, source, BugzillaServiceEntity.CloseFieldIdProperty);

            AddHelpSupport(txtCreateFieldId, source, BugzillaServiceEntity.CreateFieldIdProperty);
            AddHelpSupport(txtCreateFieldValue, source, BugzillaServiceEntity.CreateFieldValueProperty);
            AddHelpSupport(txtCloseFieldId, source, BugzillaServiceEntity.CloseFieldIdProperty);
            AddHelpSupport(txtCloseFieldValue, source, BugzillaServiceEntity.CloseFieldValueProperty);
        }
    }
}