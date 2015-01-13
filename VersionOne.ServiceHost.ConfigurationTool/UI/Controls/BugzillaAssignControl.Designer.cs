namespace VersionOne.ServiceHost.ConfigurationTool.UI.Controls {
    partial class BugzillaAssignControl {
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
            this.lblCloseReassignValue = new System.Windows.Forms.Label();
            this.lblCreateReassignValue = new System.Windows.Forms.Label();
            this.txtCloseReassignValue = new System.Windows.Forms.TextBox();
            this.txtCreateReassignValue = new System.Windows.Forms.TextBox();
            this.grpAssign = new System.Windows.Forms.GroupBox();
            this.grpAssign.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCloseReassignValue
            // 
            this.lblCloseReassignValue.AutoSize = true;
            this.lblCloseReassignValue.Location = new System.Drawing.Point(7, 46);
            this.lblCloseReassignValue.Name = "lblCloseReassignValue";
            this.lblCloseReassignValue.Size = new System.Drawing.Size(110, 13);
            this.lblCloseReassignValue.TabIndex = 14;
            this.lblCloseReassignValue.Text = "Close Reassign Value";
            // 
            // lblCreateReassignValue
            // 
            this.lblCreateReassignValue.AutoSize = true;
            this.lblCreateReassignValue.Location = new System.Drawing.Point(7, 20);
            this.lblCreateReassignValue.Name = "lblCreateReassignValue";
            this.lblCreateReassignValue.Size = new System.Drawing.Size(115, 13);
            this.lblCreateReassignValue.TabIndex = 12;
            this.lblCreateReassignValue.Text = "Create Reassign Value";
            // 
            // txtCloseReassignValue
            // 
            this.txtCloseReassignValue.Location = new System.Drawing.Point(144, 43);
            this.txtCloseReassignValue.Name = "txtCloseReassignValue";
            this.txtCloseReassignValue.Size = new System.Drawing.Size(301, 20);
            this.txtCloseReassignValue.TabIndex = 10;
            // 
            // txtCreateReassignValue
            // 
            this.txtCreateReassignValue.Location = new System.Drawing.Point(144, 17);
            this.txtCreateReassignValue.Name = "txtCreateReassignValue";
            this.txtCreateReassignValue.Size = new System.Drawing.Size(301, 20);
            this.txtCreateReassignValue.TabIndex = 8;
            // 
            // grpAssign
            // 
            this.grpAssign.Controls.Add(this.lblCreateReassignValue);
            this.grpAssign.Controls.Add(this.txtCreateReassignValue);
            this.grpAssign.Controls.Add(this.lblCloseReassignValue);
            this.grpAssign.Controls.Add(this.txtCloseReassignValue);
            this.grpAssign.Location = new System.Drawing.Point(0, -2);
            this.grpAssign.Name = "grpAssign";
            this.grpAssign.Size = new System.Drawing.Size(490, 79);
            this.grpAssign.TabIndex = 16;
            this.grpAssign.TabStop = false;
            this.grpAssign.Text = "Assign";
            // 
            // BugzillaAssignControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpAssign);
            this.Name = "BugzillaAssignControl";
            this.Size = new System.Drawing.Size(502, 89);
            this.grpAssign.ResumeLayout(false);
            this.grpAssign.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblCloseReassignValue;
        private System.Windows.Forms.Label lblCreateReassignValue;
        private System.Windows.Forms.TextBox txtCloseReassignValue;
        private System.Windows.Forms.TextBox txtCreateReassignValue;
        private System.Windows.Forms.GroupBox grpAssign;
    }
}
