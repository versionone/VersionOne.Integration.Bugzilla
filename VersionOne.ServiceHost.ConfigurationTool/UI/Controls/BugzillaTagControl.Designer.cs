namespace VersionOne.ServiceHost.ConfigurationTool.UI.Controls {
    partial class BugzillaTagControl {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent () {
            this.txtCreateFieldId = new System.Windows.Forms.TextBox();
            this.txtCreateFieldValue = new System.Windows.Forms.TextBox();
            this.txtCloseFieldId = new System.Windows.Forms.TextBox();
            this.txtCloseFieldValue = new System.Windows.Forms.TextBox();
            this.lblCreateFieldId = new System.Windows.Forms.Label();
            this.lblCloseFieldValue = new System.Windows.Forms.Label();
            this.lblCloseFieldId = new System.Windows.Forms.Label();
            this.lblCreateFieldValue = new System.Windows.Forms.Label();
            this.grpTag = new System.Windows.Forms.GroupBox();
            this.grpTag.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtCreateFieldId
            // 
            this.txtCreateFieldId.Location = new System.Drawing.Point(144, 17);
            this.txtCreateFieldId.Name = "txtCreateFieldId";
            this.txtCreateFieldId.Size = new System.Drawing.Size(302, 20);
            this.txtCreateFieldId.TabIndex = 0;
            // 
            // txtCreateFieldValue
            // 
            this.txtCreateFieldValue.Location = new System.Drawing.Point(144, 43);
            this.txtCreateFieldValue.Name = "txtCreateFieldValue";
            this.txtCreateFieldValue.Size = new System.Drawing.Size(302, 20);
            this.txtCreateFieldValue.TabIndex = 1;
            // 
            // txtCloseFieldId
            // 
            this.txtCloseFieldId.Location = new System.Drawing.Point(144, 69);
            this.txtCloseFieldId.Name = "txtCloseFieldId";
            this.txtCloseFieldId.Size = new System.Drawing.Size(302, 20);
            this.txtCloseFieldId.TabIndex = 2;
            // 
            // txtCloseFieldValue
            // 
            this.txtCloseFieldValue.Location = new System.Drawing.Point(144, 95);
            this.txtCloseFieldValue.Name = "txtCloseFieldValue";
            this.txtCloseFieldValue.Size = new System.Drawing.Size(302, 20);
            this.txtCloseFieldValue.TabIndex = 3;
            // 
            // lblCreateFieldId
            // 
            this.lblCreateFieldId.AutoSize = true;
            this.lblCreateFieldId.Location = new System.Drawing.Point(10, 20);
            this.lblCreateFieldId.Name = "lblCreateFieldId";
            this.lblCreateFieldId.Size = new System.Drawing.Size(77, 13);
            this.lblCreateFieldId.TabIndex = 4;
            this.lblCreateFieldId.Text = "Create Field ID";
            // 
            // lblCloseFieldValue
            // 
            this.lblCloseFieldValue.AutoSize = true;
            this.lblCloseFieldValue.Location = new System.Drawing.Point(10, 98);
            this.lblCloseFieldValue.Name = "lblCloseFieldValue";
            this.lblCloseFieldValue.Size = new System.Drawing.Size(88, 13);
            this.lblCloseFieldValue.TabIndex = 5;
            this.lblCloseFieldValue.Text = "Close Field Value";
            // 
            // lblCloseFieldId
            // 
            this.lblCloseFieldId.AutoSize = true;
            this.lblCloseFieldId.Location = new System.Drawing.Point(10, 72);
            this.lblCloseFieldId.Name = "lblCloseFieldId";
            this.lblCloseFieldId.Size = new System.Drawing.Size(72, 13);
            this.lblCloseFieldId.TabIndex = 6;
            this.lblCloseFieldId.Text = "Close Field ID";
            // 
            // lblCreateFieldValue
            // 
            this.lblCreateFieldValue.AutoSize = true;
            this.lblCreateFieldValue.Location = new System.Drawing.Point(10, 46);
            this.lblCreateFieldValue.Name = "lblCreateFieldValue";
            this.lblCreateFieldValue.Size = new System.Drawing.Size(93, 13);
            this.lblCreateFieldValue.TabIndex = 7;
            this.lblCreateFieldValue.Text = "Create Field Value";
            // 
            // grpTag
            // 
            this.grpTag.Controls.Add(this.lblCreateFieldId);
            this.grpTag.Controls.Add(this.lblCreateFieldValue);
            this.grpTag.Controls.Add(this.txtCreateFieldId);
            this.grpTag.Controls.Add(this.lblCloseFieldId);
            this.grpTag.Controls.Add(this.txtCreateFieldValue);
            this.grpTag.Controls.Add(this.lblCloseFieldValue);
            this.grpTag.Controls.Add(this.txtCloseFieldId);
            this.grpTag.Controls.Add(this.txtCloseFieldValue);
            this.grpTag.Location = new System.Drawing.Point(0, -1);
            this.grpTag.Name = "grpTag";
            this.grpTag.Size = new System.Drawing.Size(490, 123);
            this.grpTag.TabIndex = 16;
            this.grpTag.TabStop = false;
            this.grpTag.Text = "Tag";
            // 
            // BugzillaTagControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpTag);
            this.Name = "BugzillaTagControl";
            this.Size = new System.Drawing.Size(502, 127);
            this.grpTag.ResumeLayout(false);
            this.grpTag.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtCreateFieldId;
        private System.Windows.Forms.TextBox txtCreateFieldValue;
        private System.Windows.Forms.TextBox txtCloseFieldId;
        private System.Windows.Forms.TextBox txtCloseFieldValue;
        private System.Windows.Forms.Label lblCreateFieldId;
        private System.Windows.Forms.Label lblCloseFieldValue;
        private System.Windows.Forms.Label lblCloseFieldId;
        private System.Windows.Forms.Label lblCreateFieldValue;
        private System.Windows.Forms.GroupBox grpTag;
    }
}
