namespace VersionOne.ServiceHost.ConfigurationTool {
    partial class ConfigurationForm {
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.msMenu = new System.Windows.Forms.MenuStrip();
            this.miFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miNewFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miOpenFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.miSaveFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miSaveFileAs = new System.Windows.Forms.ToolStripMenuItem();
            this.miSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.miExit = new System.Windows.Forms.ToolStripMenuItem();
            this.miTools = new System.Windows.Forms.ToolStripMenuItem();
            this.miGenerateSnapshot = new System.Windows.Forms.ToolStripMenuItem();
            this.miSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.miOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.miHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.miAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tsMenu = new System.Windows.Forms.ToolStrip();
            this.tsbOpen = new System.Windows.Forms.ToolStripButton();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.ctlSplitContainer = new System.Windows.Forms.SplitContainer();
            this.tvServices = new System.Windows.Forms.TreeView();
            this.pnlControlHolder = new System.Windows.Forms.Panel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblHeader = new System.Windows.Forms.Label();
            this.msMenu.SuspendLayout();
            this.tsMenu.SuspendLayout();
            this.ctlSplitContainer.Panel1.SuspendLayout();
            this.ctlSplitContainer.Panel2.SuspendLayout();
            this.ctlSplitContainer.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // msMenu
            // 
            this.msMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFile,
            this.miTools,
            this.miHelp});
            this.msMenu.Location = new System.Drawing.Point(0, 0);
            this.msMenu.Name = "msMenu";
            this.msMenu.Size = new System.Drawing.Size(750, 24);
            this.msMenu.TabIndex = 0;
            this.msMenu.Text = "menuStrip1";
            // 
            // miFile
            // 
            this.miFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miNewFile,
            this.miOpenFile,
            this.miSeparator1,
            this.miSaveFile,
            this.miSaveFileAs,
            this.miSeparator2,
            this.miExit});
            this.miFile.Name = "miFile";
            this.miFile.Size = new System.Drawing.Size(35, 20);
            this.miFile.Text = "&File";
            // 
            // miNewFile
            // 
            this.miNewFile.Image = global::VersionOne.ServiceHost.ConfigurationTool.Resources.NewFileImage;
            this.miNewFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.miNewFile.Name = "miNewFile";
            this.miNewFile.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.miNewFile.Size = new System.Drawing.Size(151, 22);
            this.miNewFile.Text = "&New";
            // 
            // miOpenFile
            // 
            this.miOpenFile.Image = global::VersionOne.ServiceHost.ConfigurationTool.Resources.OpenFileImage;
            this.miOpenFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.miOpenFile.Name = "miOpenFile";
            this.miOpenFile.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.miOpenFile.Size = new System.Drawing.Size(151, 22);
            this.miOpenFile.Text = "&Open";
            // 
            // miSeparator1
            // 
            this.miSeparator1.Name = "miSeparator1";
            this.miSeparator1.Size = new System.Drawing.Size(148, 6);
            // 
            // miSaveFile
            // 
            this.miSaveFile.Image = global::VersionOne.ServiceHost.ConfigurationTool.Resources.SaveFileImage;
            this.miSaveFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.miSaveFile.Name = "miSaveFile";
            this.miSaveFile.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.miSaveFile.Size = new System.Drawing.Size(151, 22);
            this.miSaveFile.Text = "&Save";
            // 
            // miSaveFileAs
            // 
            this.miSaveFileAs.Name = "miSaveFileAs";
            this.miSaveFileAs.Size = new System.Drawing.Size(151, 22);
            this.miSaveFileAs.Text = "Save &As";
            // 
            // miSeparator2
            // 
            this.miSeparator2.Name = "miSeparator2";
            this.miSeparator2.Size = new System.Drawing.Size(148, 6);
            // 
            // miExit
            // 
            this.miExit.Name = "miExit";
            this.miExit.Size = new System.Drawing.Size(151, 22);
            this.miExit.Text = "E&xit";
            // 
            // miTools
            // 
            this.miTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miGenerateSnapshot,
            this.miSeparator3,
            this.miOptions});
            this.miTools.Name = "miTools";
            this.miTools.Size = new System.Drawing.Size(44, 20);
            this.miTools.Text = "&Tools";
            // 
            // miGenerateSnapshot
            // 
            this.miGenerateSnapshot.Name = "miGenerateSnapshot";
            this.miGenerateSnapshot.Size = new System.Drawing.Size(206, 22);
            this.miGenerateSnapshot.Text = "Create settings snapshot";
            // 
            // miSeparator3
            // 
            this.miSeparator3.Name = "miSeparator3";
            this.miSeparator3.Size = new System.Drawing.Size(203, 6);
            // 
            // miOptions
            // 
            this.miOptions.Name = "miOptions";
            this.miOptions.Size = new System.Drawing.Size(206, 22);
            this.miOptions.Text = "&Options";
            // 
            // miHelp
            // 
            this.miHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miAbout});
            this.miHelp.Name = "miHelp";
            this.miHelp.Size = new System.Drawing.Size(40, 20);
            this.miHelp.Text = "&Help";
            // 
            // miAbout
            // 
            this.miAbout.Name = "miAbout";
            this.miAbout.Size = new System.Drawing.Size(126, 22);
            this.miAbout.Text = "&About...";
            // 
            // tsMenu
            // 
            this.tsMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbOpen,
            this.tsbSave});
            this.tsMenu.Location = new System.Drawing.Point(0, 24);
            this.tsMenu.Name = "tsMenu";
            this.tsMenu.Size = new System.Drawing.Size(750, 25);
            this.tsMenu.TabIndex = 1;
            this.tsMenu.Text = "toolStrip1";
            // 
            // tsbOpen
            // 
            this.tsbOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbOpen.Image = global::VersionOne.ServiceHost.ConfigurationTool.Resources.OpenFileImage;
            this.tsbOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOpen.Name = "tsbOpen";
            this.tsbOpen.Size = new System.Drawing.Size(23, 22);
            this.tsbOpen.Text = "Open configuration";
            // 
            // tsbSave
            // 
            this.tsbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSave.Image = global::VersionOne.ServiceHost.ConfigurationTool.Resources.SaveFileImage;
            this.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Size = new System.Drawing.Size(23, 22);
            this.tsbSave.Text = "Save changes";
            // 
            // ctlSplitContainer
            // 
            this.ctlSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctlSplitContainer.Location = new System.Drawing.Point(0, 49);
            this.ctlSplitContainer.Name = "ctlSplitContainer";
            // 
            // ctlSplitContainer.Panel1
            // 
            this.ctlSplitContainer.Panel1.Controls.Add(this.tvServices);
            // 
            // ctlSplitContainer.Panel2
            // 
            this.ctlSplitContainer.Panel2.Controls.Add(this.pnlControlHolder);
            this.ctlSplitContainer.Panel2.Controls.Add(this.pnlHeader);
            this.ctlSplitContainer.Size = new System.Drawing.Size(750, 671);
            this.ctlSplitContainer.SplitterDistance = 249;
            this.ctlSplitContainer.TabIndex = 2;
            // 
            // tvServices
            // 
            this.tvServices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvServices.Location = new System.Drawing.Point(0, 0);
            this.tvServices.Name = "tvServices";
            this.tvServices.Size = new System.Drawing.Size(249, 671);
            this.tvServices.TabIndex = 0;
            // 
            // pnlControlHolder
            // 
            this.pnlControlHolder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlControlHolder.Location = new System.Drawing.Point(0, 29);
            this.pnlControlHolder.Name = "pnlControlHolder";
            this.pnlControlHolder.Size = new System.Drawing.Size(501, 747);
            this.pnlControlHolder.TabIndex = 1;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.Gainsboro;
            this.pnlHeader.Controls.Add(this.lblHeader);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(497, 29);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblHeader.Location = new System.Drawing.Point(4, 2);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(193, 24);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Current Service name";
            // 
            // ConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 720);
            this.Controls.Add(this.ctlSplitContainer);
            this.Controls.Add(this.tsMenu);
            this.Controls.Add(this.msMenu);
            this.Icon = global::VersionOne.ServiceHost.ConfigurationTool.Resources.V1Logo;
            this.MainMenuStrip = this.msMenu;
            this.MinimumSize = new System.Drawing.Size(750, 720);
            this.Name = "ConfigurationForm";
            this.Text = "ServiceHost Settings";
            this.msMenu.ResumeLayout(false);
            this.msMenu.PerformLayout();
            this.tsMenu.ResumeLayout(false);
            this.tsMenu.PerformLayout();
            this.ctlSplitContainer.Panel1.ResumeLayout(false);
            this.ctlSplitContainer.Panel2.ResumeLayout(false);
            this.ctlSplitContainer.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        // There is known bug in code generation for SplitContainer, the order of generated statements makes it fail.
        // That's why this part should be extracted to separate method to be called just after InitializeComponent.
        private void PostInitializeComponent() {
            ctlSplitContainer.Panel1MinSize = 100; 
            ctlSplitContainer.Panel2MinSize = 450;
            ctlSplitContainer.SplitterDistance = 208;
        }

        private System.Windows.Forms.MenuStrip msMenu;
        private System.Windows.Forms.ToolStripMenuItem miFile;
        private System.Windows.Forms.ToolStripMenuItem miNewFile;
        private System.Windows.Forms.ToolStripMenuItem miOpenFile;
        private System.Windows.Forms.ToolStripSeparator miSeparator1;
        private System.Windows.Forms.ToolStripMenuItem miSaveFile;
        private System.Windows.Forms.ToolStripMenuItem miSaveFileAs;
        private System.Windows.Forms.ToolStripSeparator miSeparator2;
        private System.Windows.Forms.ToolStripMenuItem miExit;
        private System.Windows.Forms.ToolStripMenuItem miTools;
        private System.Windows.Forms.ToolStripMenuItem miOptions;
        private System.Windows.Forms.ToolStripMenuItem miHelp;
        private System.Windows.Forms.ToolStripMenuItem miAbout;
        private System.Windows.Forms.ToolStrip tsMenu;
        private System.Windows.Forms.ToolStripMenuItem miGenerateSnapshot;
        private System.Windows.Forms.ToolStripSeparator miSeparator3;
        private System.Windows.Forms.ToolStripButton tsbOpen;
        private System.Windows.Forms.ToolStripButton tsbSave;
        private System.Windows.Forms.SplitContainer ctlSplitContainer;
        private System.Windows.Forms.TreeView tvServices;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Panel pnlControlHolder;
    }
}

