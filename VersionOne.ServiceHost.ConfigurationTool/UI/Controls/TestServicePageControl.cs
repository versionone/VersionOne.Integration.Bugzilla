using System;
using System.Collections.Generic;
using VersionOne.ServiceHost.ConfigurationTool.BZ;
using VersionOne.ServiceHost.ConfigurationTool.Entities;
using VersionOne.ServiceHost.ConfigurationTool.UI.Interfaces;
using System.Windows.Forms;

namespace VersionOne.ServiceHost.ConfigurationTool.UI.Controls {
    public partial class TestServicePageControl : BasePageControl<TestServiceEntity>, ITestServicePageView {
        public event EventHandler<TestProjectEventArgs> ProjectMapRowsChanged;
        private bool gridBindingComplete;

        public TestServicePageControl() {
            InitializeComponent();

            grdProjectMap.DataBindingComplete += grdProjectMap_DataBindingComplete;
            grdProjectMap.DataError += grdProjectMap_DataError;
            grdProjectMap.CellValueChanged += grdProjectMap_CellValueChanged;
            grdProjectMap.RowLeave += grdProjectMap_RowLeave;
            grdProjectMap.UserDeletingRow += grdProjectMap_UserDeletingRow;
            btnDelete.Click += btnDelete_Click;

            AddGridValidationProvider(typeof(TestPublishProjectMapping), grdProjectMap);

            AddControlTextValidation<TestServiceEntity>(txtComment, TestWriterEntity.ChangeCommentProperty);
            AddControlTextValidation<TestServiceEntity>(txtBaseQueryFilter, TestServiceEntity.BaseQueryFilterProperty);
            AddControlTextValidation<TestServiceEntity>(cboCreateDefect, TestWriterEntity.CreateDefectProperty);
            AddControlTextValidation<TestServiceEntity>(cboPassedTestStatus, TestWriterEntity.PassedOidProperty);
            AddControlTextValidation<TestServiceEntity>(cboFailedTestStatus, TestWriterEntity.FailedOidProperty);
            AddControlTextValidation<TestServiceEntity>(cboReferenceField, TestWriterEntity.ReferenceAttributeProperty);
        }

        public override void DataBind() {
            AddControlBinding(chkDisabled, Model, BaseEntity.DisabledProperty);
            AddSimpleComboboxBinding(cboReferenceField, Model, TestWriterEntity.ReferenceAttributeProperty);
            AddControlBinding(cboPassedTestStatus, Model, TestWriterEntity.PassedOidProperty);
            AddControlBinding(cboFailedTestStatus, Model, TestWriterEntity.FailedOidProperty);
            AddControlBinding(txtComment, Model, TestWriterEntity.ChangeCommentProperty);
            AddControlBinding(txtDescriptionSuffix, Model, TestWriterEntity.DescriptionSuffixProperty);
            AddControlBinding(txtBaseQueryFilter, Model, TestServiceEntity.BaseQueryFilterProperty);
            AddControlBinding(nmdInterval, Model.Timer, TimerEntity.TimerProperty);
            AddSimpleComboboxBinding(cboCreateDefect, Model, TestWriterEntity.CreateDefectProperty);

            BindProjectMappingsGrid();

            FillComboBoxWithStrings(cboReferenceField, ReferenceFieldList);
            FillComboBoxWithStrings(cboCreateDefect, CreateDefectChoiceList);
            FillComboBoxWithListValues(cboPassedTestStatus, PassedTestStatusList);
            FillComboBoxWithListValues(cboFailedTestStatus, FailedTestStatusList);

            BindHelpStrings();
        }

        private void BindProjectMappingsGrid() {
            BindProjectColumn();
            bsProjectMapping.DataSource = Model.Projects;
        }

        private void BindHelpStrings() {
            AddHelpSupport(chkDisabled, Model, BaseEntity.DisabledProperty);
            AddHelpSupport(cboReferenceField, Model, TestWriterEntity.ReferenceAttributeProperty);
            AddHelpSupport(cboPassedTestStatus, Model, TestWriterEntity.PassedOidProperty);
            AddHelpSupport(cboFailedTestStatus, Model, TestWriterEntity.FailedOidProperty);
            AddHelpSupport(txtComment, Model, TestWriterEntity.ChangeCommentProperty);
            AddHelpSupport(txtDescriptionSuffix, Model, TestWriterEntity.DescriptionSuffixProperty);
            AddHelpSupport(txtBaseQueryFilter, Model, TestServiceEntity.BaseQueryFilterProperty);
            AddHelpSupport(cboCreateDefect, Model, TestWriterEntity.CreateDefectProperty);
            AddHelpSupport(grdProjectMap, Model, TestServiceEntity.ProjectsProperty);
            AddHelpSupport(lblMin, Model.Timer, TimerEntity.TimerProperty);
        }

        private void BindProjectColumn() {
            colV1Project.Items.Clear();

            foreach(string project in Projects) {
                colV1Project.Items.Add(project);
            }
        }

        private void grdProjectMap_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e) {
            if(!ConfirmDelete()) {
                e.Cancel = true;
            }
        }

        private void grdProjectMap_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e) {
            gridBindingComplete = true;
        }

        private void grdProjectMap_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
            grdProjectMap.EndEdit();
            
            if(e.ColumnIndex == 0) {
                CommitProjectMapRowsChanges(this, grdProjectMap.Rows[e.RowIndex]);
            }
        }

        private void grdProjectMap_RowLeave(object sender, DataGridViewCellEventArgs e) {
            grdProjectMap.EndEdit();

            var currentRow = grdProjectMap.Rows[e.RowIndex];
            
            if(!currentRow.IsNewRow) {
                CommitProjectMapRowsChanges(this, currentRow);
            }
        }

        private void InvokeProjectMapRowsChanged(object sender, TestProjectEventArgs e) {
            if(ProjectMapRowsChanged == null) {
                return;
            }

            ProjectMapRowsChanged(sender, e);
        }

        private void CommitProjectMapRowsChanges(object sender, DataGridViewRow currentRow) {
            var currentProject = (TestPublishProjectMapping)currentRow.DataBoundItem;

            if(gridBindingComplete && currentProject.Name != null && currentProject.DestinationProject != null) {
                InvokeProjectMapRowsChanged(sender, new TestProjectEventArgs(currentProject));
            }
        }

        private void grdProjectMap_DataError(object sender, DataGridViewDataErrorEventArgs e) {
            if(Projects.Count != 0) {
                grdProjectMap.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Projects[0];
            }

            e.ThrowException = false;
        }

        private void btnDelete_Click(object sender, EventArgs e) {
            if(grdProjectMap.SelectedRows.Count == 0) {
                return;
            }

            if(ConfirmDelete()) {
                bsProjectMapping.Remove(grdProjectMap.SelectedRows[0].DataBoundItem);
            }
        }

        public IList<string> Projects { get; set; }
        public IEnumerable<string> ReferenceFieldList { get; set; }
        public IEnumerable<string> CreateDefectChoiceList { get; set; }
        public IList<ListValue> PassedTestStatusList { get; set; }
        public IList<ListValue> FailedTestStatusList { get; set; }
    }
}