namespace VersionOne.ServiceHost.ConfigurationTool.UI.Controls {
    partial class ChangesetsPageControl {
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
            this.grpLinkProperties = new System.Windows.Forms.GroupBox();
            this.chkLinkOnMenu = new System.Windows.Forms.CheckBox();
            this.txtLinkUrl = new System.Windows.Forms.TextBox();
            this.lblLinkName = new System.Windows.Forms.Label();
            this.txtLinkName = new System.Windows.Forms.TextBox();
            this.lblLinkUrl = new System.Windows.Forms.Label();
            this.chkAlwaysCreateChangeset = new System.Windows.Forms.CheckBox();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.lblComment = new System.Windows.Forms.Label();
            this.chkDisabled = new System.Windows.Forms.CheckBox();
            this.grpLinkProperties.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpLinkProperties
            // 
            this.grpLinkProperties.Controls.Add(this.chkLinkOnMenu);
            this.grpLinkProperties.Controls.Add(this.txtLinkUrl);
            this.grpLinkProperties.Controls.Add(this.lblLinkName);
            this.grpLinkProperties.Controls.Add(this.txtLinkName);
            this.grpLinkProperties.Controls.Add(this.lblLinkUrl);
            this.grpLinkProperties.Location = new System.Drawing.Point(12, 118);
            this.grpLinkProperties.Name = "grpLinkProperties";
            this.grpLinkProperties.Size = new System.Drawing.Size(502, 144);
            this.grpLinkProperties.TabIndex = 13;
            this.grpLinkProperties.TabStop = false;
            this.grpLinkProperties.Text = "Link properties";
            // 
            // chkLinkOnMenu
            // 
            this.chkLinkOnMenu.AutoSize = true;
            this.chkLinkOnMenu.Location = new System.Drawing.Point(23, 113);
            this.chkLinkOnMenu.Name = "chkLinkOnMenu";
            this.chkLinkOnMenu.Size = new System.Drawing.Size(86, 17);
            this.chkLinkOnMenu.TabIndex = 0;
            this.chkLinkOnMenu.Text = "Add to menu";
            this.chkLinkOnMenu.UseVisualStyleBackColor = true;
            // 
            // txtLinkUrl
            // 
            this.txtLinkUrl.Location = new System.Drawing.Point(136, 68);
            this.txtLinkUrl.Name = "txtLinkUrl";
            this.txtLinkUrl.Size = new System.Drawing.Size(326, 20);
            this.txtLinkUrl.TabIndex = 4;
            // 
            // lblLinkName
            // 
            this.lblLinkName.AutoSize = true;
            this.lblLinkName.Location = new System.Drawing.Point(20, 28);
            this.lblLinkName.Name = "lblLinkName";
            this.lblLinkName.Size = new System.Drawing.Size(35, 13);
            this.lblLinkName.TabIndex = 1;
            this.lblLinkName.Text = "Name";
            // 
            // txtLinkName
            // 
            this.txtLinkName.Location = new System.Drawing.Point(136, 19);
            this.txtLinkName.Name = "txtLinkName";
            this.txtLinkName.Size = new System.Drawing.Size(326, 20);
            this.txtLinkName.TabIndex = 2;
            // 
            // lblLinkUrl
            // 
            this.lblLinkUrl.AutoSize = true;
            this.lblLinkUrl.Location = new System.Drawing.Point(20, 75);
            this.lblLinkUrl.Name = "lblLinkUrl";
            this.lblLinkUrl.Size = new System.Drawing.Size(29, 13);
            this.lblLinkUrl.TabIndex = 3;
            this.lblLinkUrl.Text = "URL";
            // 
            // chkAlwaysCreateChangeset
            // 
            this.chkAlwaysCreateChangeset.AutoSize = true;
            this.chkAlwaysCreateChangeset.Location = new System.Drawing.Point(33, 86);
            this.chkAlwaysCreateChangeset.Name = "chkAlwaysCreateChangeset";
            this.chkAlwaysCreateChangeset.Size = new System.Drawing.Size(150, 17);
            this.chkAlwaysCreateChangeset.TabIndex = 12;
            this.chkAlwaysCreateChangeset.Text = "Always create changesets";
            this.chkAlwaysCreateChangeset.UseVisualStyleBackColor = true;
            // 
            // txtComment
            // 
            this.txtComment.Location = new System.Drawing.Point(148, 50);
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(326, 20);
            this.txtComment.TabIndex = 11;
            // 
            // lblComment
            // 
            this.lblComment.AutoSize = true;
            this.lblComment.Location = new System.Drawing.Point(31, 53);
            this.lblComment.Name = "lblComment";
            this.lblComment.Size = new System.Drawing.Size(90, 13);
            this.lblComment.TabIndex = 10;
            this.lblComment.Text = "Change comment";
            // 
            // chkDisabled
            // 
            this.chkDisabled.AutoSize = true;
            this.chkDisabled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkDisabled.Location = new System.Drawing.Point(407, 18);
            this.chkDisabled.Name = "chkDisabled";
            this.chkDisabled.Size = new System.Drawing.Size(67, 17);
            this.chkDisabled.TabIndex = 7;
            this.chkDisabled.Text = "Disabled";
            this.chkDisabled.UseVisualStyleBackColor = true;
            // 
            // ChangesetsPageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpLinkProperties);
            this.Controls.Add(this.chkAlwaysCreateChangeset);
            this.Controls.Add(this.txtComment);
            this.Controls.Add(this.lblComment);
            this.Controls.Add(this.chkDisabled);
            this.Name = "ChangesetsPageControl";
            this.Size = new System.Drawing.Size(540, 283);
            this.grpLinkProperties.ResumeLayout(false);
            this.grpLinkProperties.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpLinkProperties;
        private System.Windows.Forms.CheckBox chkLinkOnMenu;
        private System.Windows.Forms.TextBox txtLinkUrl;
        private System.Windows.Forms.Label lblLinkName;
        private System.Windows.Forms.TextBox txtLinkName;
        private System.Windows.Forms.Label lblLinkUrl;
        private System.Windows.Forms.CheckBox chkAlwaysCreateChangeset;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.Label lblComment;
        private System.Windows.Forms.CheckBox chkDisabled;
    }
}
