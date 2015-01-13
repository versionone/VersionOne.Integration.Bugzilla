namespace VersionOne.ServiceHost.ConfigurationTool.UI.Controls {
    partial class PathSelectorControl {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
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
        private void InitializeComponent() {
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lblWatchPath = new System.Windows.Forms.Label();
            this.txtWatchPath = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(311, 30);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(74, 23);
            this.btnBrowse.TabIndex = 25;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            // 
            // lblWatchPath
            // 
            this.lblWatchPath.AutoSize = true;
            this.lblWatchPath.Location = new System.Drawing.Point(0, 6);
            this.lblWatchPath.Name = "lblWatchPath";
            this.lblWatchPath.Size = new System.Drawing.Size(63, 13);
            this.lblWatchPath.TabIndex = 23;
            this.lblWatchPath.Text = "Watch path";
            // 
            // txtWatchPath
            // 
            this.txtWatchPath.AcceptsTab = true;
            this.txtWatchPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWatchPath.Location = new System.Drawing.Point(70, 3);
            this.txtWatchPath.MinimumSize = new System.Drawing.Size(50, 4);
            this.txtWatchPath.Name = "txtWatchPath";
            this.txtWatchPath.Size = new System.Drawing.Size(315, 20);
            this.txtWatchPath.TabIndex = 24;
            // 
            // PathSelectorControl
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblWatchPath);
            this.Controls.Add(this.txtWatchPath);
            this.Controls.Add(this.btnBrowse);
            this.Name = "PathSelectorControl";
            this.Size = new System.Drawing.Size(400, 60);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label lblWatchPath;
        private System.Windows.Forms.TextBox txtWatchPath;
    }
}
