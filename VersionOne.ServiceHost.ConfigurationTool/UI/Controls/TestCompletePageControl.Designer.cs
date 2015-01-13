namespace VersionOne.ServiceHost.ConfigurationTool.UI.Controls {
    partial class TestCompletePageControl {
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
            this.lblPollIntervalSuffix = new System.Windows.Forms.Label();
            this.lblPollIntervalPrefix = new System.Windows.Forms.Label();
            this.numIntervalMinutes = new System.Windows.Forms.NumericUpDown();
            this.lblRetryTimeout = new System.Windows.Forms.Label();
            this.chkDisabled = new System.Windows.Forms.CheckBox();
            this.lblRetryAttempts = new System.Windows.Forms.Label();
            this.lblTimeoutIntervalSuffix = new System.Windows.Forms.Label();
            this.numTimeoutInterval = new System.Windows.Forms.NumericUpDown();
            this.lblAttemptsSuffix = new System.Windows.Forms.Label();
            this.numAttemps = new System.Windows.Forms.NumericUpDown();
            this.grbRetry = new System.Windows.Forms.GroupBox();
            this.grpGeneral = new System.Windows.Forms.GroupBox();
            this.pscWatchSuite = new VersionOne.ServiceHost.ConfigurationTool.UI.Controls.PathSelectorControl();
            ((System.ComponentModel.ISupportInitialize)(this.numIntervalMinutes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeoutInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAttemps)).BeginInit();
            this.grbRetry.SuspendLayout();
            this.grpGeneral.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblPollIntervalSuffix
            // 
            this.lblPollIntervalSuffix.AutoSize = true;
            this.lblPollIntervalSuffix.Location = new System.Drawing.Point(169, 91);
            this.lblPollIntervalSuffix.Name = "lblPollIntervalSuffix";
            this.lblPollIntervalSuffix.Size = new System.Drawing.Size(43, 13);
            this.lblPollIntervalSuffix.TabIndex = 16;
            this.lblPollIntervalSuffix.Text = "minutes";
            // 
            // lblPollIntervalPrefix
            // 
            this.lblPollIntervalPrefix.AutoSize = true;
            this.lblPollIntervalPrefix.Location = new System.Drawing.Point(13, 91);
            this.lblPollIntervalPrefix.Name = "lblPollIntervalPrefix";
            this.lblPollIntervalPrefix.Size = new System.Drawing.Size(61, 13);
            this.lblPollIntervalPrefix.TabIndex = 14;
            this.lblPollIntervalPrefix.Text = "Poll interval";
            // 
            // numIntervalMinutes
            // 
            this.numIntervalMinutes.Location = new System.Drawing.Point(111, 89);
            this.numIntervalMinutes.Maximum = new decimal(new int[] {
            1440,
            0,
            0,
            0});
            this.numIntervalMinutes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numIntervalMinutes.Name = "numIntervalMinutes";
            this.numIntervalMinutes.Size = new System.Drawing.Size(52, 20);
            this.numIntervalMinutes.TabIndex = 15;
            this.numIntervalMinutes.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // lblRetryTimeout
            // 
            this.lblRetryTimeout.AutoSize = true;
            this.lblRetryTimeout.Location = new System.Drawing.Point(13, 26);
            this.lblRetryTimeout.Name = "lblRetryTimeout";
            this.lblRetryTimeout.Size = new System.Drawing.Size(69, 13);
            this.lblRetryTimeout.TabIndex = 12;
            this.lblRetryTimeout.Text = "Retry timeout";
            // 
            // chkDisabled
            // 
            this.chkDisabled.AutoSize = true;
            this.chkDisabled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkDisabled.Location = new System.Drawing.Point(409, 12);
            this.chkDisabled.Name = "chkDisabled";
            this.chkDisabled.Size = new System.Drawing.Size(67, 17);
            this.chkDisabled.TabIndex = 9;
            this.chkDisabled.Text = "Disabled";
            this.chkDisabled.UseVisualStyleBackColor = true;
            // 
            // lblRetryAttempts
            // 
            this.lblRetryAttempts.AutoSize = true;
            this.lblRetryAttempts.Location = new System.Drawing.Point(13, 57);
            this.lblRetryAttempts.Name = "lblRetryAttempts";
            this.lblRetryAttempts.Size = new System.Drawing.Size(75, 13);
            this.lblRetryAttempts.TabIndex = 17;
            this.lblRetryAttempts.Text = "Retry attempts";
            // 
            // lblTimeoutIntervalSuffix
            // 
            this.lblTimeoutIntervalSuffix.AutoSize = true;
            this.lblTimeoutIntervalSuffix.Location = new System.Drawing.Point(169, 26);
            this.lblTimeoutIntervalSuffix.Name = "lblTimeoutIntervalSuffix";
            this.lblTimeoutIntervalSuffix.Size = new System.Drawing.Size(47, 13);
            this.lblTimeoutIntervalSuffix.TabIndex = 19;
            this.lblTimeoutIntervalSuffix.Text = "seconds";
            // 
            // numTimeoutInterval
            // 
            this.numTimeoutInterval.Location = new System.Drawing.Point(111, 24);
            this.numTimeoutInterval.Maximum = new decimal(new int[] {
            1440,
            0,
            0,
            0});
            this.numTimeoutInterval.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numTimeoutInterval.Name = "numTimeoutInterval";
            this.numTimeoutInterval.Size = new System.Drawing.Size(52, 20);
            this.numTimeoutInterval.TabIndex = 18;
            this.numTimeoutInterval.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // lblAttemptsSuffix
            // 
            this.lblAttemptsSuffix.AutoSize = true;
            this.lblAttemptsSuffix.Location = new System.Drawing.Point(169, 57);
            this.lblAttemptsSuffix.Name = "lblAttemptsSuffix";
            this.lblAttemptsSuffix.Size = new System.Drawing.Size(31, 13);
            this.lblAttemptsSuffix.TabIndex = 21;
            this.lblAttemptsSuffix.Text = "times";
            // 
            // numAttemps
            // 
            this.numAttemps.Location = new System.Drawing.Point(111, 55);
            this.numAttemps.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numAttemps.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numAttemps.Name = "numAttemps";
            this.numAttemps.Size = new System.Drawing.Size(52, 20);
            this.numAttemps.TabIndex = 20;
            this.numAttemps.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // grbRetry
            // 
            this.grbRetry.Controls.Add(this.lblRetryTimeout);
            this.grbRetry.Controls.Add(this.lblRetryAttempts);
            this.grbRetry.Controls.Add(this.lblAttemptsSuffix);
            this.grbRetry.Controls.Add(this.numTimeoutInterval);
            this.grbRetry.Controls.Add(this.numAttemps);
            this.grbRetry.Controls.Add(this.lblTimeoutIntervalSuffix);
            this.grbRetry.Location = new System.Drawing.Point(12, 174);
            this.grbRetry.Name = "grbRetry";
            this.grbRetry.Size = new System.Drawing.Size(502, 96);
            this.grbRetry.TabIndex = 23;
            this.grbRetry.TabStop = false;
            this.grbRetry.Text = "Retry";
            // 
            // grpGeneral
            // 
            this.grpGeneral.Controls.Add(this.pscWatchSuite);
            this.grpGeneral.Controls.Add(this.lblPollIntervalPrefix);
            this.grpGeneral.Controls.Add(this.lblPollIntervalSuffix);
            this.grpGeneral.Controls.Add(this.numIntervalMinutes);
            this.grpGeneral.Location = new System.Drawing.Point(12, 35);
            this.grpGeneral.Name = "grpGeneral";
            this.grpGeneral.Size = new System.Drawing.Size(502, 121);
            this.grpGeneral.TabIndex = 24;
            this.grpGeneral.TabStop = false;
            this.grpGeneral.Text = "General";
            // 
            // pscWatchSuite
            // 
            this.pscWatchSuite.AllowDrop = true;
            this.pscWatchSuite.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pscWatchSuite.DialogType = VersionOne.ServiceHost.ConfigurationTool.UI.Controls.PathSelectorControl.DialogTypes.File;
            this.pscWatchSuite.Location = new System.Drawing.Point(16, 19);
            this.pscWatchSuite.Name = "pscWatchSuite";
            this.pscWatchSuite.SelectedPath = "";
            this.pscWatchSuite.Size = new System.Drawing.Size(461, 60);
            this.pscWatchSuite.TabIndex = 17;
            this.pscWatchSuite.TextBoxLeft = 70;
            // 
            // TestCompletePageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpGeneral);
            this.Controls.Add(this.chkDisabled);
            this.Controls.Add(this.grbRetry);
            this.Name = "TestCompletePageControl";
            this.Size = new System.Drawing.Size(540, 293);
            ((System.ComponentModel.ISupportInitialize)(this.numIntervalMinutes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeoutInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAttemps)).EndInit();
            this.grbRetry.ResumeLayout(false);
            this.grbRetry.PerformLayout();
            this.grpGeneral.ResumeLayout(false);
            this.grpGeneral.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPollIntervalSuffix;
        private System.Windows.Forms.Label lblPollIntervalPrefix;
        private System.Windows.Forms.NumericUpDown numIntervalMinutes;
        private System.Windows.Forms.Label lblRetryTimeout;
        private System.Windows.Forms.CheckBox chkDisabled;
        private System.Windows.Forms.Label lblRetryAttempts;
        private System.Windows.Forms.Label lblTimeoutIntervalSuffix;
        private System.Windows.Forms.NumericUpDown numTimeoutInterval;
        private System.Windows.Forms.Label lblAttemptsSuffix;
        private System.Windows.Forms.NumericUpDown numAttemps;
        private System.Windows.Forms.GroupBox grbRetry;
        private System.Windows.Forms.GroupBox grpGeneral;
        private PathSelectorControl pscWatchSuite;

    }
}
