using VersionOne.ServiceHost.ConfigurationTool.Entities;
using VersionOne.ServiceHost.ConfigurationTool.UI.Interfaces;

namespace VersionOne.ServiceHost.ConfigurationTool.UI.Controls {
    public partial class WorkitemsPageControl : BasePageControl<WorkitemWriterEntity>, IWorkitemsPageView {
        private string[] referenceFieldList;
        
        public WorkitemsPageControl() {
            InitializeComponent();
            AddControlValidation<WorkitemWriterEntity>(cboReferenceField, WorkitemWriterEntity.ExternalIdFieldNameProperty, "Text");
        }

        public override void DataBind() {
            AddControlBinding(chkDisabled, Model, BaseEntity.DisabledProperty);
            AddSimpleComboboxBinding(cboReferenceField, Model, WorkitemWriterEntity.ExternalIdFieldNameProperty);

            BindHelpStrings();
        }

        private void BindHelpStrings() {
            AddHelpSupport(chkDisabled, Model, BaseEntity.DisabledProperty);
            AddHelpSupport(cboReferenceField, Model, WorkitemWriterEntity.ExternalIdFieldNameProperty);
        }

        public string[] ReferenceFieldList {
            get { return referenceFieldList; }
            set {
                referenceFieldList = value;
                cboReferenceField.Items.Clear();
                if (value != null) {
                    cboReferenceField.Items.AddRange(value);
                }
            }
        }
    }
}