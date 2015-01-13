namespace VersionOne.ServiceHost.ConfigurationTool.UI.Controls {
    partial class TestServicePageControl {
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
            this.lblCreateDefect = new System.Windows.Forms.Label();
            this.cboCreateDefect = new System.Windows.Forms.ComboBox();
            this.txtDescriptionSuffix = new System.Windows.Forms.TextBox();
            this.lblDescriptionSuffix = new System.Windows.Forms.Label();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.lblChangeComment = new System.Windows.Forms.Label();
            this.lblReferenceFieldName = new System.Windows.Forms.Label();
            this.cboReferenceField = new System.Windows.Forms.ComboBox();
            this.lblPassedTestStatus = new System.Windows.Forms.Label();
            this.cboPassedTestStatus = new System.Windows.Forms.ComboBox();
            this.lblFailedTestStatus = new System.Windows.Forms.Label();
            this.cboFailedTestStatus = new System.Windows.Forms.ComboBox();
            this.chkDisabled = new System.Windows.Forms.CheckBox();
            this.txtBaseQueryFilter = new System.Windows.Forms.TextBox();
            this.lblBaseQueryFilter = new System.Windows.Forms.Label();
            this.grpProjectMap = new System.Windows.Forms.GroupBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.grdProjectMap = new System.Windows.Forms.DataGridView();
            this.bsProjectMapping = new System.Windows.Forms.BindingSource(this.components);
            this.lblMin = new System.Windows.Forms.Label();
            this.lblTimerInterval = new System.Windows.Forms.Label();
            this.nmdInterval = new System.Windows.Forms.NumericUpDown();
            this.colProjectId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIncludeChildren = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colV1Project = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.grpProjectMap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdProjectMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsProjectMapping)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmdInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCreateDefect
            // 
            this.lblCreateDefect.AutoSize = true;
            this.lblCreateDefect.Location = new System.Drawing.Point(29, 158);
            this.lblCreateDefect.Name = "lblCreateDefect";
            this.lblCreateDefect.Size = new System.Drawing.Size(73, 13);
            this.lblCreateDefect.TabIndex = 11;
            this.lblCreateDefect.Text = "Create Defect";
            // 
            // cboCreateDefect
            // 
            this.cboCreateDefect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCreateDefect.FormattingEnabled = true;
            this.cboCreateDefect.Location = new System.Drawing.Point(148, 155);
            this.cboCreateDefect.Name = "cboCreateDefect";
            this.cboCreateDefect.Size = new System.Drawing.Size(324, 21);
            this.cboCreateDefect.TabIndex = 12;
            // 
            // txtDescriptionSuffix
            // 
            this.txtDescriptionSuffix.Location = new System.Drawing.Point(148, 127);
            this.txtDescriptionSuffix.Name = "txtDescriptionSuffix";
            this.txtDescriptionSuffix.Size = new System.Drawing.Size(324, 20);
            this.txtDescriptionSuffix.TabIndex = 10;
            // 
            // lblDescriptionSuffix
            // 
            this.lblDescriptionSuffix.AutoSize = true;
            this.lblDescriptionSuffix.Location = new System.Drawing.Point(29, 130);
            this.lblDescriptionSuffix.Name = "lblDescriptionSuffix";
            this.lblDescriptionSuffix.Size = new System.Drawing.Size(87, 13);
            this.lblDescriptionSuffix.TabIndex = 9;
            this.lblDescriptionSuffix.Text = "Description suffix";
            // 
            // txtComment
            // 
            this.txtComment.Location = new System.Drawing.Point(148, 99);
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(324, 20);
            this.txtComment.TabIndex = 8;
            // 
            // lblChangeComment
            // 
            this.lblChangeComment.AutoSize = true;
            this.lblChangeComment.Location = new System.Drawing.Point(29, 102);
            this.lblChangeComment.Name = "lblChangeComment";
            this.lblChangeComment.Size = new System.Drawing.Size(90, 13);
            this.lblChangeComment.TabIndex = 7;
            this.lblChangeComment.Text = "Change comment";
            // 
            // lblReferenceFieldName
            // 
            this.lblReferenceFieldName.AutoSize = true;
            this.lblReferenceFieldName.Location = new System.Drawing.Point(29, 44);
            this.lblReferenceFieldName.Name = "lblReferenceFieldName";
            this.lblReferenceFieldName.Size = new System.Drawing.Size(113, 13);
            this.lblReferenceFieldName.TabIndex = 1;
            this.lblReferenceFieldName.Text = "Reference Field Name";
            // 
            // cboReferenceField
            // 
            this.cboReferenceField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboReferenceField.FormattingEnabled = true;
            this.cboReferenceField.Location = new System.Drawing.Point(148, 41);
            this.cboReferenceField.Name = "cboReferenceField";
            this.cboReferenceField.Size = new System.Drawing.Size(324, 21);
            this.cboReferenceField.TabIndex = 2;
            // 
            // lblPassedTestStatus
            // 
            this.lblPassedTestStatus.AutoSize = true;
            this.lblPassedTestStatus.Location = new System.Drawing.Point(29, 73);
            this.lblPassedTestStatus.Name = "lblPassedTestStatus";
            this.lblPassedTestStatus.Size = new System.Drawing.Size(99, 13);
            this.lblPassedTestStatus.TabIndex = 3;
            this.lblPassedTestStatus.Text = "Passed Test Status";
            // 
            // cboPassedTestStatus
            // 
            this.cboPassedTestStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPassedTestStatus.FormattingEnabled = true;
            this.cboPassedTestStatus.Location = new System.Drawing.Point(148, 70);
            this.cboPassedTestStatus.Name = "cboPassedTestStatus";
            this.cboPassedTestStatus.Size = new System.Drawing.Size(95, 21);
            this.cboPassedTestStatus.TabIndex = 4;
            // 
            // lblFailedTestStatus
            // 
            this.lblFailedTestStatus.AutoSize = true;
            this.lblFailedTestStatus.Location = new System.Drawing.Point(279, 73);
            this.lblFailedTestStatus.Name = "lblFailedTestStatus";
            this.lblFailedTestStatus.Size = new System.Drawing.Size(92, 13);
            this.lblFailedTestStatus.TabIndex = 5;
            this.lblFailedTestStatus.Text = "Failed Test Status";
            // 
            // cboFailedTestStatus
            // 
            this.cboFailedTestStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFailedTestStatus.FormattingEnabled = true;
            this.cboFailedTestStatus.Location = new System.Drawing.Point(377, 70);
            this.cboFailedTestStatus.Name = "cboFailedTestStatus";
            this.cboFailedTestStatus.Size = new System.Drawing.Size(95, 21);
            this.cboFailedTestStatus.TabIndex = 6;
            // 
            // chkDisabled
            // 
            this.chkDisabled.AutoSize = true;
            this.chkDisabled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkDisabled.Location = new System.Drawing.Point(405, 18);
            this.chkDisabled.Name = "chkDisabled";
            this.chkDisabled.Size = new System.Drawing.Size(67, 17);
            this.chkDisabled.TabIndex = 0;
            this.chkDisabled.Text = "Disabled";
            this.chkDisabled.UseVisualStyleBackColor = true;
            // 
            // txtBaseQueryFilter
            // 
            this.txtBaseQueryFilter.Location = new System.Drawing.Point(148, 184);
            this.txtBaseQueryFilter.Name = "txtBaseQueryFilter";
            this.txtBaseQueryFilter.Size = new System.Drawing.Size(324, 20);
            this.txtBaseQueryFilter.TabIndex = 14;
            // 
            // lblBaseQueryFilter
            // 
            this.lblBaseQueryFilter.AutoSize = true;
            this.lblBaseQueryFilter.Location = new System.Drawing.Point(29, 187);
            this.lblBaseQueryFilter.Name = "lblBaseQueryFilter";
            this.lblBaseQueryFilter.Size = new System.Drawing.Size(82, 13);
            this.lblBaseQueryFilter.TabIndex = 13;
            this.lblBaseQueryFilter.Text = "Base query filter";
            // 
            // grpProjectMap
            // 
            this.grpProjectMap.Controls.Add(this.btnDelete);
            this.grpProjectMap.Controls.Add(this.grdProjectMap);
            this.grpProjectMap.Location = new System.Drawing.Point(12, 255);
            this.grpProjectMap.Name = "grpProjectMap";
            this.grpProjectMap.Size = new System.Drawing.Size(502, 181);
            this.grpProjectMap.TabIndex = 18;
            this.grpProjectMap.TabStop = false;
            this.grpProjectMap.Text = "Project mapping";
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::VersionOne.ServiceHost.ConfigurationTool.Resources.DeleteIcon;
            this.btnDelete.Location = new System.Drawing.Point(372, 145);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(88, 27);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "Delete";
            this.btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // grdProjectMap
            // 
            this.grdProjectMap.AutoGenerateColumns = false;
            this.grdProjectMap.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grdProjectMap.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdProjectMap.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colProjectId,
            this.colIncludeChildren,
            this.colV1Project});
            this.grdProjectMap.DataSource = this.bsProjectMapping;
            this.grdProjectMap.Location = new System.Drawing.Point(16, 20);
            this.grdProjectMap.MultiSelect = false;
            this.grdProjectMap.Name = "grdProjectMap";
            this.grdProjectMap.Size = new System.Drawing.Size(444, 115);
            this.grdProjectMap.TabIndex = 0;
            // 
            // lblMin
            // 
            this.lblMin.AutoSize = true;
            this.lblMin.Location = new System.Drawing.Point(213, 213);
            this.lblMin.Name = "lblMin";
            this.lblMin.Size = new System.Drawing.Size(43, 13);
            this.lblMin.TabIndex = 17;
            this.lblMin.Text = "minutes";
            // 
            // lblTimerInterval
            // 
            this.lblTimerInterval.AutoSize = true;
            this.lblTimerInterval.Location = new System.Drawing.Point(29, 213);
            this.lblTimerInterval.Name = "lblTimerInterval";
            this.lblTimerInterval.Size = new System.Drawing.Size(62, 13);
            this.lblTimerInterval.TabIndex = 15;
            this.lblTimerInterval.Text = "Poll Interval";
            // 
            // nmdInterval
            // 
            this.nmdInterval.Location = new System.Drawing.Point(148, 212);
        	this.nmdInterval.Minimum = 1;
			//new decimal(new int[] {
			//1,
			//0,
			//0,
			//0});
            this.nmdInterval.Name = "nmdInterval";
            this.nmdInterval.Size = new System.Drawing.Size(52, 20);
            this.nmdInterval.TabIndex = 16;
            this.nmdInterval.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // colProjectId
            // 
            this.colProjectId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colProjectId.DataPropertyName = "DestinationProject";
            this.colProjectId.HeaderText = "QualityCenter Project";
            this.colProjectId.MinimumWidth = 100;
            this.colProjectId.Name = "colProjectId";
            // 
            // colIncludeChildren
            // 
            this.colIncludeChildren.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colIncludeChildren.DataPropertyName = "IncludeChildren";
            this.colIncludeChildren.FillWeight = 50F;
            this.colIncludeChildren.HeaderText = "Incl. Child Projects";
            this.colIncludeChildren.MinimumWidth = 50;
            this.colIncludeChildren.Name = "colIncludeChildren";
            // 
            // colV1Project
            // 
            this.colV1Project.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colV1Project.DataPropertyName = "Name";
            this.colV1Project.HeaderText = "VersionOne Project";
            this.colV1Project.MinimumWidth = 100;
            this.colV1Project.Name = "colV1Project";
            // 
            // TestServicePageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblMin);
            this.Controls.Add(this.lblTimerInterval);
            this.Controls.Add(this.nmdInterval);
            this.Controls.Add(this.grpProjectMap);
            this.Controls.Add(this.txtBaseQueryFilter);
            this.Controls.Add(this.lblBaseQueryFilter);
            this.Controls.Add(this.lblCreateDefect);
            this.Controls.Add(this.cboCreateDefect);
            this.Controls.Add(this.txtDescriptionSuffix);
            this.Controls.Add(this.lblDescriptionSuffix);
            this.Controls.Add(this.txtComment);
            this.Controls.Add(this.lblChangeComment);
            this.Controls.Add(this.lblReferenceFieldName);
            this.Controls.Add(this.cboReferenceField);
            this.Controls.Add(this.lblPassedTestStatus);
            this.Controls.Add(this.cboPassedTestStatus);
            this.Controls.Add(this.lblFailedTestStatus);
            this.Controls.Add(this.cboFailedTestStatus);
            this.Controls.Add(this.chkDisabled);
            this.Name = "TestServicePageControl";
            this.Size = new System.Drawing.Size(540, 445);
            this.grpProjectMap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdProjectMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsProjectMapping)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmdInterval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCreateDefect;
        private System.Windows.Forms.ComboBox cboCreateDefect;
        private System.Windows.Forms.TextBox txtDescriptionSuffix;
        private System.Windows.Forms.Label lblDescriptionSuffix;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.Label lblChangeComment;
        private System.Windows.Forms.Label lblReferenceFieldName;
        private System.Windows.Forms.ComboBox cboReferenceField;
        private System.Windows.Forms.Label lblPassedTestStatus;
        private System.Windows.Forms.ComboBox cboPassedTestStatus;
        private System.Windows.Forms.Label lblFailedTestStatus;
        private System.Windows.Forms.ComboBox cboFailedTestStatus;
        private System.Windows.Forms.CheckBox chkDisabled;
        private System.Windows.Forms.TextBox txtBaseQueryFilter;
        private System.Windows.Forms.Label lblBaseQueryFilter;
        private System.Windows.Forms.GroupBox grpProjectMap;
        private System.Windows.Forms.DataGridView grdProjectMap;
        private System.Windows.Forms.BindingSource bsProjectMapping;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label lblMin;
        private System.Windows.Forms.Label lblTimerInterval;
        private System.Windows.Forms.NumericUpDown nmdInterval;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProjectId;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colIncludeChildren;
        private System.Windows.Forms.DataGridViewComboBoxColumn colV1Project;
    }
}