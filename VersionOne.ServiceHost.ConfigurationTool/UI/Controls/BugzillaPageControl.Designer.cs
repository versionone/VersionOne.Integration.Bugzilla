namespace VersionOne.ServiceHost.ConfigurationTool.UI.Controls {
    partial class BugzillaPageControl {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.grpConnection = new System.Windows.Forms.GroupBox();
            this.chkIgnoreCertificate = new System.Windows.Forms.CheckBox();
            this.lblConnectionValidation = new System.Windows.Forms.Label();
            this.btnVerify = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.lblUrl = new System.Windows.Forms.Label();
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.lblMin = new System.Windows.Forms.Label();
            this.lblTimerInterval = new System.Windows.Forms.Label();
            this.nmdInterval = new System.Windows.Forms.NumericUpDown();
            this.txtSearchName = new System.Windows.Forms.TextBox();
            this.lblSearchName = new System.Windows.Forms.Label();
            this.txtDefectLinkFieldId = new System.Windows.Forms.TextBox();
            this.lblDefectLinkFieldId = new System.Windows.Forms.Label();
            this.chkDisable = new System.Windows.Forms.CheckBox();
            this.rbTag = new System.Windows.Forms.RadioButton();
            this.rbAssign = new System.Windows.Forms.RadioButton();
            this.pnlTagOrAssign = new System.Windows.Forms.Panel();
            this.grpUpdateBugs = new System.Windows.Forms.GroupBox();
            this.chkCloseAccept = new System.Windows.Forms.CheckBox();
            this.chkCreateAccept = new System.Windows.Forms.CheckBox();
            this.txtCreateResolveValue = new System.Windows.Forms.TextBox();
            this.lblCreateResolveValue = new System.Windows.Forms.Label();
            this.txtCloseResolveValue = new System.Windows.Forms.TextBox();
            this.lblCloseResolveValue = new System.Windows.Forms.Label();
            this.grpVersionOne = new System.Windows.Forms.GroupBox();
            this.cboSourceFieldValue = new System.Windows.Forms.ComboBox();
            this.txtUrlTitle = new System.Windows.Forms.TextBox();
            this.lblUrlTitle = new System.Windows.Forms.Label();
            this.txtUrlTempl = new System.Windows.Forms.TextBox();
            this.lblUrlTemplate = new System.Windows.Forms.Label();
            this.lblSource = new System.Windows.Forms.Label();
            this.tcBugzillaData = new System.Windows.Forms.TabControl();
            this.tpGeneral = new System.Windows.Forms.TabPage();
            this.tpMappings = new System.Windows.Forms.TabPage();
            this.grpPriorityMappings = new System.Windows.Forms.GroupBox();
            this.btnDeletePriorityMapping = new System.Windows.Forms.Button();
            this.grdPriorityMappings = new System.Windows.Forms.DataGridView();
            this.colVersionOnePriority = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colBugzillaPriority = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpProjectMappings = new System.Windows.Forms.GroupBox();
            this.btnDeleteProjectMapping = new System.Windows.Forms.Button();
            this.grdProjectMappings = new System.Windows.Forms.DataGridView();
            this.colVersionOneProject = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colBugzillaProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bsProjectMappings = new System.Windows.Forms.BindingSource(this.components);
            this.bsPriorityMappings = new System.Windows.Forms.BindingSource(this.components);
            this.grpConnection.SuspendLayout();
            this.grpSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmdInterval)).BeginInit();
            this.grpUpdateBugs.SuspendLayout();
            this.grpVersionOne.SuspendLayout();
            this.tcBugzillaData.SuspendLayout();
            this.tpGeneral.SuspendLayout();
            this.tpMappings.SuspendLayout();
            this.grpPriorityMappings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdPriorityMappings)).BeginInit();
            this.grpProjectMappings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdProjectMappings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsProjectMappings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsPriorityMappings)).BeginInit();
            this.SuspendLayout();
            // 
            // grpConnection
            // 
            this.grpConnection.Controls.Add(this.chkIgnoreCertificate);
            this.grpConnection.Controls.Add(this.lblConnectionValidation);
            this.grpConnection.Controls.Add(this.btnVerify);
            this.grpConnection.Controls.Add(this.txtPassword);
            this.grpConnection.Controls.Add(this.lblPassword);
            this.grpConnection.Controls.Add(this.txtUserName);
            this.grpConnection.Controls.Add(this.lblUserName);
            this.grpConnection.Controls.Add(this.txtUrl);
            this.grpConnection.Controls.Add(this.lblUrl);
            this.grpConnection.Location = new System.Drawing.Point(12, 10);
            this.grpConnection.Name = "grpConnection";
            this.grpConnection.Size = new System.Drawing.Size(502, 109);
            this.grpConnection.TabIndex = 0;
            this.grpConnection.TabStop = false;
            this.grpConnection.Text = "Connection";
            // 
            // chkIgnoreCertificate
            // 
            this.chkIgnoreCertificate.Location = new System.Drawing.Point(13, 75);
            this.chkIgnoreCertificate.Name = "chkIgnoreCertificate";
            this.chkIgnoreCertificate.Size = new System.Drawing.Size(106, 17);
            this.chkIgnoreCertificate.TabIndex = 0;
            this.chkIgnoreCertificate.Text = "Ignore Certificate";
            this.chkIgnoreCertificate.UseVisualStyleBackColor = true;
            // 
            // lblConnectionValidation
            // 
            this.lblConnectionValidation.AutoSize = true;
            this.lblConnectionValidation.Location = new System.Drawing.Point(175, 76);
            this.lblConnectionValidation.Name = "lblConnectionValidation";
            this.lblConnectionValidation.Size = new System.Drawing.Size(81, 13);
            this.lblConnectionValidation.TabIndex = 6;
            this.lblConnectionValidation.Text = "Validation result";
            this.lblConnectionValidation.Visible = false;
            // 
            // btnVerify
            // 
            this.btnVerify.Location = new System.Drawing.Point(366, 69);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(87, 26);
            this.btnVerify.TabIndex = 7;
            this.btnVerify.Text = "Validate";
            this.btnVerify.UseVisualStyleBackColor = true;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(334, 43);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(119, 20);
            this.txtPassword.TabIndex = 5;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(265, 46);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 4;
            this.lblPassword.Text = "Password";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(89, 43);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(121, 20);
            this.txtUserName.TabIndex = 3;
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(10, 46);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(55, 13);
            this.lblUserName.TabIndex = 2;
            this.lblUserName.Text = "Username";
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(89, 17);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(364, 20);
            this.txtUrl.TabIndex = 1;
            // 
            // lblUrl
            // 
            this.lblUrl.AutoSize = true;
            this.lblUrl.Location = new System.Drawing.Point(10, 20);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(68, 13);
            this.lblUrl.TabIndex = 0;
            this.lblUrl.Text = "Bugzilla URL";
            // 
            // grpSearch
            // 
            this.grpSearch.Controls.Add(this.lblMin);
            this.grpSearch.Controls.Add(this.lblTimerInterval);
            this.grpSearch.Controls.Add(this.nmdInterval);
            this.grpSearch.Controls.Add(this.txtSearchName);
            this.grpSearch.Controls.Add(this.lblSearchName);
            this.grpSearch.Location = new System.Drawing.Point(12, 234);
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.Size = new System.Drawing.Size(502, 80);
            this.grpSearch.TabIndex = 2;
            this.grpSearch.TabStop = false;
            this.grpSearch.Text = "Find Bugzilla Bugs";
            // 
            // lblMin
            // 
            this.lblMin.AutoSize = true;
            this.lblMin.Location = new System.Drawing.Point(213, 55);
            this.lblMin.Name = "lblMin";
            this.lblMin.Size = new System.Drawing.Size(43, 13);
            this.lblMin.TabIndex = 4;
            this.lblMin.Text = "minutes";
            // 
            // lblTimerInterval
            // 
            this.lblTimerInterval.AutoSize = true;
            this.lblTimerInterval.Location = new System.Drawing.Point(10, 52);
            this.lblTimerInterval.Name = "lblTimerInterval";
            this.lblTimerInterval.Size = new System.Drawing.Size(62, 13);
            this.lblTimerInterval.TabIndex = 2;
            this.lblTimerInterval.Text = "Poll Interval";
            // 
            // nmdInterval
            // 
            this.nmdInterval.Location = new System.Drawing.Point(152, 52);
            this.nmdInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmdInterval.Name = "nmdInterval";
            this.nmdInterval.Size = new System.Drawing.Size(52, 20);
            this.nmdInterval.TabIndex = 3;
            this.nmdInterval.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // txtSearchName
            // 
            this.txtSearchName.Location = new System.Drawing.Point(152, 19);
            this.txtSearchName.Name = "txtSearchName";
            this.txtSearchName.Size = new System.Drawing.Size(301, 20);
            this.txtSearchName.TabIndex = 1;
            // 
            // lblSearchName
            // 
            this.lblSearchName.AutoSize = true;
            this.lblSearchName.Location = new System.Drawing.Point(10, 22);
            this.lblSearchName.Name = "lblSearchName";
            this.lblSearchName.Size = new System.Drawing.Size(72, 13);
            this.lblSearchName.TabIndex = 0;
            this.lblSearchName.Text = "Search Name";
            // 
            // txtDefectLinkFieldId
            // 
            this.txtDefectLinkFieldId.Location = new System.Drawing.Point(152, 19);
            this.txtDefectLinkFieldId.Name = "txtDefectLinkFieldId";
            this.txtDefectLinkFieldId.Size = new System.Drawing.Size(301, 20);
            this.txtDefectLinkFieldId.TabIndex = 1;
            // 
            // lblDefectLinkFieldId
            // 
            this.lblDefectLinkFieldId.AutoSize = true;
            this.lblDefectLinkFieldId.Location = new System.Drawing.Point(10, 22);
            this.lblDefectLinkFieldId.Name = "lblDefectLinkFieldId";
            this.lblDefectLinkFieldId.Size = new System.Drawing.Size(136, 13);
            this.lblDefectLinkFieldId.TabIndex = 0;
            this.lblDefectLinkFieldId.Text = "Link to VersionOne Field ID";
            // 
            // chkDisable
            // 
            this.chkDisable.AutoSize = true;
            this.chkDisable.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkDisable.Location = new System.Drawing.Point(402, 3);
            this.chkDisable.Name = "chkDisable";
            this.chkDisable.Size = new System.Drawing.Size(67, 17);
            this.chkDisable.TabIndex = 0;
            this.chkDisable.Text = "Disabled";
            this.chkDisable.UseVisualStyleBackColor = true;
            // 
            // rbTag
            // 
            this.rbTag.AutoSize = true;
            this.rbTag.Location = new System.Drawing.Point(152, 124);
            this.rbTag.Name = "rbTag";
            this.rbTag.Size = new System.Drawing.Size(152, 17);
            this.rbTag.TabIndex = 9;
            this.rbTag.TabStop = true;
            this.rbTag.Text = "Tag defects by field values";
            this.rbTag.UseVisualStyleBackColor = true;
            // 
            // rbAssign
            // 
            this.rbAssign.AutoSize = true;
            this.rbAssign.Location = new System.Drawing.Point(13, 124);
            this.rbAssign.Name = "rbAssign";
            this.rbAssign.Size = new System.Drawing.Size(129, 17);
            this.rbAssign.TabIndex = 8;
            this.rbAssign.TabStop = true;
            this.rbAssign.Text = "Assign defects to user";
            this.rbAssign.UseVisualStyleBackColor = true;
            // 
            // pnlTagOrAssign
            // 
            this.pnlTagOrAssign.Location = new System.Drawing.Point(6, 147);
            this.pnlTagOrAssign.Name = "pnlTagOrAssign";
            this.pnlTagOrAssign.Size = new System.Drawing.Size(496, 123);
            this.pnlTagOrAssign.TabIndex = 10;
            // 
            // grpUpdateBugs
            // 
            this.grpUpdateBugs.Controls.Add(this.chkCloseAccept);
            this.grpUpdateBugs.Controls.Add(this.chkCreateAccept);
            this.grpUpdateBugs.Controls.Add(this.rbAssign);
            this.grpUpdateBugs.Controls.Add(this.txtCreateResolveValue);
            this.grpUpdateBugs.Controls.Add(this.lblCreateResolveValue);
            this.grpUpdateBugs.Controls.Add(this.rbTag);
            this.grpUpdateBugs.Controls.Add(this.txtCloseResolveValue);
            this.grpUpdateBugs.Controls.Add(this.pnlTagOrAssign);
            this.grpUpdateBugs.Controls.Add(this.txtDefectLinkFieldId);
            this.grpUpdateBugs.Controls.Add(this.lblCloseResolveValue);
            this.grpUpdateBugs.Controls.Add(this.lblDefectLinkFieldId);
            this.grpUpdateBugs.Location = new System.Drawing.Point(12, 320);
            this.grpUpdateBugs.Name = "grpUpdateBugs";
            this.grpUpdateBugs.Size = new System.Drawing.Size(502, 275);
            this.grpUpdateBugs.TabIndex = 3;
            this.grpUpdateBugs.TabStop = false;
            this.grpUpdateBugs.Text = "Update Bugzilla Bugs";
            // 
            // chkCloseAccept
            // 
            this.chkCloseAccept.AutoSize = true;
            this.chkCloseAccept.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkCloseAccept.Location = new System.Drawing.Point(181, 47);
            this.chkCloseAccept.Name = "chkCloseAccept";
            this.chkCloseAccept.Size = new System.Drawing.Size(116, 17);
            this.chkCloseAccept.TabIndex = 3;
            this.chkCloseAccept.Text = "Close Accept         ";
            this.chkCloseAccept.UseVisualStyleBackColor = true;
            // 
            // chkCreateAccept
            // 
            this.chkCreateAccept.AutoSize = true;
            this.chkCreateAccept.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkCreateAccept.Location = new System.Drawing.Point(9, 47);
            this.chkCreateAccept.Name = "chkCreateAccept";
            this.chkCreateAccept.Size = new System.Drawing.Size(121, 17);
            this.chkCreateAccept.TabIndex = 2;
            this.chkCreateAccept.Text = "Create Accept         ";
            this.chkCreateAccept.UseVisualStyleBackColor = true;
            // 
            // txtCreateResolveValue
            // 
            this.txtCreateResolveValue.Location = new System.Drawing.Point(152, 70);
            this.txtCreateResolveValue.Name = "txtCreateResolveValue";
            this.txtCreateResolveValue.Size = new System.Drawing.Size(301, 20);
            this.txtCreateResolveValue.TabIndex = 5;
            // 
            // lblCreateResolveValue
            // 
            this.lblCreateResolveValue.AutoSize = true;
            this.lblCreateResolveValue.Location = new System.Drawing.Point(10, 73);
            this.lblCreateResolveValue.Name = "lblCreateResolveValue";
            this.lblCreateResolveValue.Size = new System.Drawing.Size(110, 13);
            this.lblCreateResolveValue.TabIndex = 4;
            this.lblCreateResolveValue.Text = "Create Resolve Value";
            // 
            // txtCloseResolveValue
            // 
            this.txtCloseResolveValue.Location = new System.Drawing.Point(152, 96);
            this.txtCloseResolveValue.Name = "txtCloseResolveValue";
            this.txtCloseResolveValue.Size = new System.Drawing.Size(301, 20);
            this.txtCloseResolveValue.TabIndex = 7;
            // 
            // lblCloseResolveValue
            // 
            this.lblCloseResolveValue.AutoSize = true;
            this.lblCloseResolveValue.Location = new System.Drawing.Point(10, 99);
            this.lblCloseResolveValue.Name = "lblCloseResolveValue";
            this.lblCloseResolveValue.Size = new System.Drawing.Size(105, 13);
            this.lblCloseResolveValue.TabIndex = 6;
            this.lblCloseResolveValue.Text = "Close Resolve Value";
            // 
            // grpVersionOne
            // 
            this.grpVersionOne.Controls.Add(this.cboSourceFieldValue);
            this.grpVersionOne.Controls.Add(this.txtUrlTitle);
            this.grpVersionOne.Controls.Add(this.lblUrlTitle);
            this.grpVersionOne.Controls.Add(this.txtUrlTempl);
            this.grpVersionOne.Controls.Add(this.lblUrlTemplate);
            this.grpVersionOne.Controls.Add(this.lblSource);
            this.grpVersionOne.Location = new System.Drawing.Point(12, 124);
            this.grpVersionOne.Name = "grpVersionOne";
            this.grpVersionOne.Size = new System.Drawing.Size(502, 105);
            this.grpVersionOne.TabIndex = 1;
            this.grpVersionOne.TabStop = false;
            this.grpVersionOne.Text = "VersionOne Defect Attributes";
            // 
            // cboSourceFieldValue
            // 
            this.cboSourceFieldValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSourceFieldValue.FormattingEnabled = true;
            this.cboSourceFieldValue.Location = new System.Drawing.Point(152, 19);
            this.cboSourceFieldValue.Name = "cboSourceFieldValue";
            this.cboSourceFieldValue.Size = new System.Drawing.Size(301, 21);
            this.cboSourceFieldValue.TabIndex = 1;
            // 
            // txtUrlTitle
            // 
            this.txtUrlTitle.Location = new System.Drawing.Point(152, 72);
            this.txtUrlTitle.Name = "txtUrlTitle";
            this.txtUrlTitle.Size = new System.Drawing.Size(301, 20);
            this.txtUrlTitle.TabIndex = 5;
            // 
            // lblUrlTitle
            // 
            this.lblUrlTitle.AutoSize = true;
            this.lblUrlTitle.Location = new System.Drawing.Point(10, 75);
            this.lblUrlTitle.Name = "lblUrlTitle";
            this.lblUrlTitle.Size = new System.Drawing.Size(52, 13);
            this.lblUrlTitle.TabIndex = 4;
            this.lblUrlTitle.Text = "URL Title";
            // 
            // txtUrlTempl
            // 
            this.txtUrlTempl.Location = new System.Drawing.Point(152, 46);
            this.txtUrlTempl.Name = "txtUrlTempl";
            this.txtUrlTempl.Size = new System.Drawing.Size(301, 20);
            this.txtUrlTempl.TabIndex = 3;
            // 
            // lblUrlTemplate
            // 
            this.lblUrlTemplate.AutoSize = true;
            this.lblUrlTemplate.Location = new System.Drawing.Point(10, 49);
            this.lblUrlTemplate.Name = "lblUrlTemplate";
            this.lblUrlTemplate.Size = new System.Drawing.Size(76, 13);
            this.lblUrlTemplate.TabIndex = 2;
            this.lblUrlTemplate.Text = "URL Template";
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Location = new System.Drawing.Point(10, 22);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(41, 13);
            this.lblSource.TabIndex = 0;
            this.lblSource.Text = "Source";
            // 
            // tcBugzillaData
            // 
            this.tcBugzillaData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcBugzillaData.Controls.Add(this.tpGeneral);
            this.tcBugzillaData.Controls.Add(this.tpMappings);
            this.tcBugzillaData.Location = new System.Drawing.Point(0, 20);
            this.tcBugzillaData.Name = "tcBugzillaData";
            this.tcBugzillaData.SelectedIndex = 0;
            this.tcBugzillaData.Size = new System.Drawing.Size(540, 643);
            this.tcBugzillaData.TabIndex = 1;
            // 
            // tpGeneral
            // 
            this.tpGeneral.Controls.Add(this.grpConnection);
            this.tpGeneral.Controls.Add(this.grpUpdateBugs);
            this.tpGeneral.Controls.Add(this.grpVersionOne);
            this.tpGeneral.Controls.Add(this.grpSearch);
            this.tpGeneral.Location = new System.Drawing.Point(4, 22);
            this.tpGeneral.Name = "tpGeneral";
            this.tpGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tpGeneral.Size = new System.Drawing.Size(532, 617);
            this.tpGeneral.TabIndex = 0;
            this.tpGeneral.Text = "Bugzilla Service Settings";
            this.tpGeneral.UseVisualStyleBackColor = true;
            // 
            // tpMappings
            // 
            this.tpMappings.Controls.Add(this.grpPriorityMappings);
            this.tpMappings.Controls.Add(this.grpProjectMappings);
            this.tpMappings.Location = new System.Drawing.Point(4, 22);
            this.tpMappings.Name = "tpMappings";
            this.tpMappings.Padding = new System.Windows.Forms.Padding(3);
            this.tpMappings.Size = new System.Drawing.Size(532, 617);
            this.tpMappings.TabIndex = 1;
            this.tpMappings.Text = "Project and Priority Mappings";
            this.tpMappings.UseVisualStyleBackColor = true;
            // 
            // grpPriorityMappings
            // 
            this.grpPriorityMappings.Controls.Add(this.btnDeletePriorityMapping);
            this.grpPriorityMappings.Controls.Add(this.grdPriorityMappings);
            this.grpPriorityMappings.Location = new System.Drawing.Point(12, 251);
            this.grpPriorityMappings.Name = "grpPriorityMappings";
            this.grpPriorityMappings.Size = new System.Drawing.Size(502, 228);
            this.grpPriorityMappings.TabIndex = 2;
            this.grpPriorityMappings.TabStop = false;
            this.grpPriorityMappings.Text = "Priority Mappings";
            // 
            // btnDeletePriorityMapping
            // 
            this.btnDeletePriorityMapping.Image = global::VersionOne.ServiceHost.ConfigurationTool.Resources.DeleteIcon;
            this.btnDeletePriorityMapping.Location = new System.Drawing.Point(334, 187);
            this.btnDeletePriorityMapping.Name = "btnDeletePriorityMapping";
            this.btnDeletePriorityMapping.Size = new System.Drawing.Size(119, 30);
            this.btnDeletePriorityMapping.TabIndex = 1;
            this.btnDeletePriorityMapping.Text = "Delete current row";
            this.btnDeletePriorityMapping.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDeletePriorityMapping.UseVisualStyleBackColor = true;
            // 
            // grdPriorityMappings
            // 
            this.grdPriorityMappings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdPriorityMappings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colVersionOnePriority,
            this.colBugzillaPriority});
            this.grdPriorityMappings.Location = new System.Drawing.Point(12, 20);
            this.grdPriorityMappings.Name = "grdPriorityMappings";
            this.grdPriorityMappings.Size = new System.Drawing.Size(441, 161);
            this.grdPriorityMappings.TabIndex = 0;
            // 
            // colVersionOnePriority
            // 
            this.colVersionOnePriority.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colVersionOnePriority.DataPropertyName = "VersionOnePriorityId";
            this.colVersionOnePriority.HeaderText = "VersionOne Priority";
            this.colVersionOnePriority.MinimumWidth = 100;
            this.colVersionOnePriority.Name = "colVersionOnePriority";
            // 
            // colBugzillaPriority
            // 
            this.colBugzillaPriority.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colBugzillaPriority.DataPropertyName = "BugzillaPriorityName";
            this.colBugzillaPriority.HeaderText = "Bugzilla Priority";
            this.colBugzillaPriority.MinimumWidth = 100;
            this.colBugzillaPriority.Name = "colBugzillaPriority";
            // 
            // grpProjectMappings
            // 
            this.grpProjectMappings.Controls.Add(this.btnDeleteProjectMapping);
            this.grpProjectMappings.Controls.Add(this.grdProjectMappings);
            this.grpProjectMappings.Location = new System.Drawing.Point(12, 17);
            this.grpProjectMappings.Name = "grpProjectMappings";
            this.grpProjectMappings.Size = new System.Drawing.Size(502, 228);
            this.grpProjectMappings.TabIndex = 0;
            this.grpProjectMappings.TabStop = false;
            this.grpProjectMappings.Text = "Project Mappings";
            // 
            // btnDeleteProjectMapping
            // 
            this.btnDeleteProjectMapping.Image = global::VersionOne.ServiceHost.ConfigurationTool.Resources.DeleteIcon;
            this.btnDeleteProjectMapping.Location = new System.Drawing.Point(334, 187);
            this.btnDeleteProjectMapping.Name = "btnDeleteProjectMapping";
            this.btnDeleteProjectMapping.Size = new System.Drawing.Size(119, 30);
            this.btnDeleteProjectMapping.TabIndex = 1;
            this.btnDeleteProjectMapping.Text = "Delete current row";
            this.btnDeleteProjectMapping.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDeleteProjectMapping.UseVisualStyleBackColor = true;
            // 
            // grdProjectMappings
            // 
            this.grdProjectMappings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdProjectMappings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colVersionOneProject,
            this.colBugzillaProductName});
            this.grdProjectMappings.Location = new System.Drawing.Point(15, 20);
            this.grdProjectMappings.Name = "grdProjectMappings";
            this.grdProjectMappings.Size = new System.Drawing.Size(438, 161);
            this.grdProjectMappings.TabIndex = 0;
            // 
            // colVersionOneProject
            // 
            this.colVersionOneProject.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colVersionOneProject.DataPropertyName = "VersionOneProjectToken";
            this.colVersionOneProject.HeaderText = "VersionOne Project";
            this.colVersionOneProject.MinimumWidth = 100;
            this.colVersionOneProject.Name = "colVersionOneProject";
            // 
            // colBugzillaProductName
            // 
            this.colBugzillaProductName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colBugzillaProductName.DataPropertyName = "BugzillaProjectName";
            this.colBugzillaProductName.HeaderText = "Bugzilla Product Name";
            this.colBugzillaProductName.MinimumWidth = 100;
            this.colBugzillaProductName.Name = "colBugzillaProductName";
            // 
            // BugzillaPageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.tcBugzillaData);
            this.Controls.Add(this.chkDisable);
            this.Name = "BugzillaPageControl";
            this.Size = new System.Drawing.Size(540, 663);
            this.grpConnection.ResumeLayout(false);
            this.grpConnection.PerformLayout();
            this.grpSearch.ResumeLayout(false);
            this.grpSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmdInterval)).EndInit();
            this.grpUpdateBugs.ResumeLayout(false);
            this.grpUpdateBugs.PerformLayout();
            this.grpVersionOne.ResumeLayout(false);
            this.grpVersionOne.PerformLayout();
            this.tcBugzillaData.ResumeLayout(false);
            this.tpGeneral.ResumeLayout(false);
            this.tpMappings.ResumeLayout(false);
            this.grpPriorityMappings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdPriorityMappings)).EndInit();
            this.grpProjectMappings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdProjectMappings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsProjectMappings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsPriorityMappings)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

}
        #endregion

private System.Windows.Forms.GroupBox grpConnection;
        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Label lblUrl;
        private System.Windows.Forms.GroupBox grpSearch;
        private System.Windows.Forms.TextBox txtSearchName;
        private System.Windows.Forms.Label lblSearchName;
        private System.Windows.Forms.TextBox txtDefectLinkFieldId;
        private System.Windows.Forms.Label lblDefectLinkFieldId;
        private System.Windows.Forms.CheckBox chkDisable;
        private System.Windows.Forms.Label lblTimerInterval;
        private System.Windows.Forms.NumericUpDown nmdInterval;
        private System.Windows.Forms.Label lblConnectionValidation;
        private System.Windows.Forms.Label lblMin;
        private System.Windows.Forms.RadioButton rbTag;
        private System.Windows.Forms.RadioButton rbAssign;
        private System.Windows.Forms.Panel pnlTagOrAssign;
        private System.Windows.Forms.GroupBox grpUpdateBugs;
        private System.Windows.Forms.TextBox txtCreateResolveValue;
        private System.Windows.Forms.Label lblCreateResolveValue;
        private System.Windows.Forms.TextBox txtCloseResolveValue;
        private System.Windows.Forms.Label lblCloseResolveValue;
        private System.Windows.Forms.CheckBox chkCloseAccept;
        private System.Windows.Forms.CheckBox chkCreateAccept;
        private System.Windows.Forms.GroupBox grpVersionOne;
        private System.Windows.Forms.ComboBox cboSourceFieldValue;
        private System.Windows.Forms.TextBox txtUrlTitle;
        private System.Windows.Forms.Label lblUrlTitle;
        private System.Windows.Forms.TextBox txtUrlTempl;
        private System.Windows.Forms.Label lblUrlTemplate;
        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.TabControl tcBugzillaData;
        private System.Windows.Forms.TabPage tpGeneral;
        private System.Windows.Forms.TabPage tpMappings;
        private System.Windows.Forms.GroupBox grpProjectMappings;
        private System.Windows.Forms.DataGridView grdProjectMappings;
        private System.Windows.Forms.Button btnDeleteProjectMapping;
        private System.Windows.Forms.BindingSource bsProjectMappings;
        private System.Windows.Forms.BindingSource bsPriorityMappings;
        private System.Windows.Forms.GroupBox grpPriorityMappings;
        private System.Windows.Forms.Button btnDeletePriorityMapping;
        private System.Windows.Forms.DataGridView grdPriorityMappings;
        private System.Windows.Forms.DataGridViewComboBoxColumn colVersionOnePriority;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBugzillaPriority;
        private System.Windows.Forms.DataGridViewComboBoxColumn colVersionOneProject;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBugzillaProductName;
        private System.Windows.Forms.CheckBox chkIgnoreCertificate;
    }
}
