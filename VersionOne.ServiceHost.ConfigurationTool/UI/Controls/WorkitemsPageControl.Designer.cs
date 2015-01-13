namespace VersionOne.ServiceHost.ConfigurationTool.UI.Controls {
    partial class WorkitemsPageControl {
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
            this.lblExternalId = new System.Windows.Forms.Label();
            this.cboReferenceField = new System.Windows.Forms.ComboBox();
            this.chkDisabled = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblExternalId
            // 
            this.lblExternalId.AutoSize = true;
            this.lblExternalId.Location = new System.Drawing.Point(20, 53);
            this.lblExternalId.Name = "lblExternalId";
            this.lblExternalId.Size = new System.Drawing.Size(113, 13);
            this.lblExternalId.TabIndex = 1;
            this.lblExternalId.Text = "Reference Field Name";
            // 
            // cboReferenceField
            // 
            this.cboReferenceField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboReferenceField.FormattingEnabled = true;
            this.cboReferenceField.Location = new System.Drawing.Point(146, 50);
            this.cboReferenceField.Name = "cboReferenceField";
            this.cboReferenceField.Size = new System.Drawing.Size(334, 21);
            this.cboReferenceField.TabIndex = 2;
            // 
            // chkDisabled
            // 
            this.chkDisabled.AutoSize = true;
            this.chkDisabled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkDisabled.Location = new System.Drawing.Point(413, 17);
            this.chkDisabled.Name = "chkDisabled";
            this.chkDisabled.Size = new System.Drawing.Size(67, 17);
            this.chkDisabled.TabIndex = 0;
            this.chkDisabled.Text = "Disabled";
            this.chkDisabled.UseVisualStyleBackColor = true;
            // 
            // WorkitemsPageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblExternalId);
            this.Controls.Add(this.cboReferenceField);
            this.Controls.Add(this.chkDisabled);
            this.Name = "WorkitemsPageControl";
            this.Size = new System.Drawing.Size(540, 91);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblExternalId;
        private System.Windows.Forms.ComboBox cboReferenceField;
        private System.Windows.Forms.CheckBox chkDisabled;
    }
}
