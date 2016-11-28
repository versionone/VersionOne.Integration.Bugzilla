using System;
using System.Drawing;
using System.Windows.Forms;
using VersionOne.ServiceHost.ConfigurationTool.Entities;
using VersionOne.ServiceHost.ConfigurationTool.UI.Interfaces;
using System.Collections.Generic;
using VersionOne.ServiceHost.ConfigurationTool.DL;
using VersionOne.ServiceHost.ConfigurationTool.BZ;

namespace VersionOne.ServiceHost.ConfigurationTool.UI.Controls {
    // To be able to access Design View on this control you need to extend from UserControl and comment all lines with errors
    public partial class BugzillaPageControl : BasePageControl<BugzillaServiceEntity>, IBugzillaPageView {
        public event EventHandler ValidationRequested;

        private readonly BugzillaTagControl ctlBugTag;
        private readonly BugzillaAssignControl ctlBugAssign;

        public BugzillaPageControl() {
            InitializeComponent();
            AddTabHighlightingSupport(tcBugzillaData);

            grdProjectMappings.AutoGenerateColumns = false;
            grdPriorityMappings.AutoGenerateColumns = false;

            btnVerify.Click += btnVerifyBugzillaConnection_Click;
            rbTag.Click += delegate { SwitchControls(); };
            rbAssign.Click += delegate { SwitchControls(); };

            btnDeleteProjectMapping.Click += btnDeleteProjectMapping_Click;
            btnDeletePriorityMapping.Click += btnDeletePriorityMapping_Click;

            grdProjectMappings.UserDeletingRow += (sender, e) => { e.Cancel = !ConfirmDelete(); };
            grdProjectMappings.DataError += grdProjectMappings_DataError;
            
            grdPriorityMappings.UserDeletingRow += (sender, e) => { e.Cancel = !ConfirmDelete(); };
            grdPriorityMappings.DataError += grdPriorityMappings_DataError;

            AddControlValidation<BugzillaServiceEntity>(txtUrl, BugzillaServiceEntity.UrlProperty, "Text");
            AddControlValidation<BugzillaServiceEntity>(txtPassword, BugzillaServiceEntity.PasswordProperty, "Text");
            AddControlValidation<BugzillaServiceEntity>(txtUserName, BugzillaServiceEntity.UserNameProperty, "Text");
            AddControlValidation<BugzillaServiceEntity>(txtSearchName, BugzillaServiceEntity.SearchNameProperty, "Text");
            AddControlValidation<BugzillaServiceEntity>(txtDefectLinkFieldId, BugzillaServiceEntity.LinkFieldProperty, "Text");
            AddControlValidation<BugzillaServiceEntity>(txtUrlTempl, BugzillaServiceEntity.UrlTemplateProperty, "Text");
            AddControlValidation<BugzillaServiceEntity>(txtUrlTitle, BugzillaServiceEntity.UrlTitleProperty, "Text");
            AddControlValidation<BugzillaServiceEntity>(cboSourceFieldValue, BugzillaServiceEntity.SourceNameProperty, "Text");

            AddGridValidationProvider(typeof(BugzillaProjectMapping), grdProjectMappings);
            AddGridValidationProvider(typeof(BugzillaPriorityMapping), grdPriorityMappings);

            ctlBugTag = new BugzillaTagControl();
            ctlBugAssign = new BugzillaAssignControl();
        }

        public override void DataBind() {
            AddControlBinding(txtUrl, Model, BugzillaServiceEntity.UrlProperty);
            AddControlBinding(chkDisable, Model, BaseEntity.DisabledProperty);
            AddControlBinding(txtPassword, Model, BugzillaServiceEntity.PasswordProperty);
            AddControlBinding(txtUserName, Model, BugzillaServiceEntity.UserNameProperty);
            AddControlBinding(chkIgnoreCertificate, Model.IgnoreCertificate, NullableBool.BoolValueProperty);
            AddControlBinding(txtUrlTitle, Model, BugzillaServiceEntity.UrlTitleProperty);
            AddControlBinding(txtUrlTempl, Model, BugzillaServiceEntity.UrlTemplateProperty);
            AddControlBinding(txtSearchName, Model, BugzillaServiceEntity.SearchNameProperty);
            AddSimpleComboboxBinding(cboSourceFieldValue, Model, BugzillaServiceEntity.SourceNameProperty);
            AddControlBinding(txtDefectLinkFieldId, Model, BugzillaServiceEntity.LinkFieldProperty);
            AddControlBinding(nmdInterval, Model.Timer, TimerEntity.TimerProperty);
            AddControlBinding(txtCreateResolveValue, Model, BugzillaServiceEntity.CreateResolveValueProperty);
            AddControlBinding(chkCloseAccept, Model.CloseAccept, NullableBool.BoolValueProperty);
            AddControlBinding(txtCloseResolveValue, Model, BugzillaServiceEntity.CloseResolveValueProperty);

            ctlBugTag.DataBind(Model);            
            ctlBugAssign.DataBind(Model);

            if(Model.AssignButtonPressed) {
                LoadControl(ctlBugAssign);
                rbAssign.Checked = true;
            } else {
                LoadControl(ctlBugTag);
                rbTag.Checked = true;
            }

            BindProjectMappingsGrid();
            BindPriorityMappingsGrid();

            FillComboBoxWithStrings(cboSourceFieldValue, SourceList);

            BindHelpStrings();

            InvokeValidationTriggered();
        }

        private void BindPriorityMappingsGrid() {
            BindPriorityColumn();
            bsPriorityMappings.DataSource = Model.PriorityMappings;
            grdPriorityMappings.DataSource = bsPriorityMappings;
        }

        private void BindProjectMappingsGrid() {
            BindProjectColumn();
            bsProjectMappings.DataSource = Model.ProjectMappings;
            grdProjectMappings.DataSource = bsProjectMappings;
        }

        private void BindHelpStrings() {
            AddHelpSupport(chkDisable, Model, BaseEntity.DisabledProperty);
            AddHelpSupport(txtUrl, Model, BugzillaServiceEntity.UrlProperty);
            AddHelpSupport(cboSourceFieldValue, Model, BugzillaServiceEntity.SourceNameProperty);
            AddHelpSupport(txtUrlTempl, Model, BugzillaServiceEntity.UrlTemplateProperty);
            AddHelpSupport(txtSearchName, Model, BugzillaServiceEntity.SearchNameProperty);
            AddHelpSupport(txtDefectLinkFieldId, Model, BugzillaServiceEntity.LinkFieldProperty);
            AddHelpSupport(chkCloseAccept, Model, BugzillaServiceEntity.CloseAcceptProperty);
            AddHelpSupport(txtCreateResolveValue, Model, BugzillaServiceEntity.CreateResolveValueProperty);
            AddHelpSupport(txtCloseResolveValue, Model, BugzillaServiceEntity.CloseResolveValueProperty);
            AddHelpSupport(grdProjectMappings, Model, BugzillaServiceEntity.ProjectMappingsProperty);
            AddHelpSupport(grdPriorityMappings, Model, BugzillaServiceEntity.PriorityMappingsProperty);
            AddHelpSupport(lblMin, Model.Timer, TimerEntity.TimerProperty);
        }

        private void BindProjectColumn() {
            colVersionOneProject.DisplayMember = "DisplayName";
            colVersionOneProject.ValueMember = "Token";
            colVersionOneProject.DataSource = VersionOneProjects;
        }

        private void BindPriorityColumn() {
            colVersionOnePriority.DisplayMember = "Name";
            colVersionOnePriority.ValueMember = "Value";
            colVersionOnePriority.DataSource = VersionOnePriorities;
        }

        private void btnVerifyBugzillaConnection_Click(object sender, EventArgs e) {
            if(ValidationRequested != null) {
                lblConnectionValidation.ForeColor = Color.Black;
                ValidationRequested(this, EventArgs.Empty);
            }
        }

        private void btnDeletePriorityMapping_Click(object sender, EventArgs e) {
            if(grdPriorityMappings.SelectedRows.Count > 0 && ConfirmDelete()) {
                bsPriorityMappings.Remove(grdPriorityMappings.SelectedRows[0].DataBoundItem);
            }
        }

        private void btnDeleteProjectMapping_Click(object sender, EventArgs e) {
            if(grdProjectMappings.SelectedRows.Count > 0 && ConfirmDelete()) {
                bsProjectMappings.Remove(grdProjectMappings.SelectedRows[0].DataBoundItem);
            }
        }

        private void grdProjectMappings_DataError(object sender, DataGridViewDataErrorEventArgs e) {
            if(VersionOneProjects != null && VersionOneProjects.Count > 0) {
                grdProjectMappings.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = VersionOneProjects[0].Token;
            } 
            
            e.ThrowException = false;
        }

        private void grdPriorityMappings_DataError(object sender, DataGridViewDataErrorEventArgs e) {
            if(VersionOnePriorities != null && VersionOnePriorities.Count > 0) {
                grdPriorityMappings.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = VersionOnePriorities[0].Value;
            } 
            
            e.ThrowException = false;
        }

        private void SwitchControls() {
            pnlTagOrAssign.Controls.Clear();

            if(rbTag.Checked) {
                LoadControl(ctlBugTag);
            } else if(rbAssign.Checked) {
                LoadControl(ctlBugAssign);
            } else {
                throw new InvalidOperationException();
            }
        }

        private void LoadControl(Control ctl) {
            pnlTagOrAssign.Controls.Add(ctl);
            ctl.Dock = DockStyle.Fill;
        }

        #region IBugzillaPageView Members

        public string[] SourceList { get; set; }

        public IList<ProjectWrapper> VersionOneProjects { get; set; }

        public IList<ListValue> VersionOnePriorities { get; set; }

        public void SetValidationResult(bool validationSuccessful) {
            lblConnectionValidation.Visible = true;
            if (validationSuccessful) {
                lblConnectionValidation.ForeColor = Color.Green;
                lblConnectionValidation.Text = Resources.ConnectionValidMessage;
            } else {
                lblConnectionValidation.ForeColor = Color.Red;
                lblConnectionValidation.Text = Resources.ConnectionInvalidMessage;
            }
        }

        public void SetGeneralTabValid(bool isValid) {
            TabHighlighter.SetTabPageValidationMark(tpGeneral, isValid);
        }

        public void SetMappingTabValid(bool isValid) {
            TabHighlighter.SetTabPageValidationMark(tpMappings, isValid);
        }

        #endregion
    }
}