using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using VersionOne.ServiceHost.ConfigurationTool.BZ;
using VersionOne.ServiceHost.ConfigurationTool.UI.Interfaces;

namespace VersionOne.ServiceHost.ConfigurationTool {
    public partial class ConfigurationForm : Form, IConfigurationView {
        private IConfigurationController controller;
        
        private readonly List<string> coreServiceNodes = new List<string>(new[] { "Tests", "Defects", "Changesets", "Test service" });
        private readonly List<string> nonSelectableNodes = new List<string>(new[] {"Services" });

        private readonly List<string> coreServices = new List<string>();
        private readonly List<string> customServices = new List<string>();
        
        private bool coreServicesEnabled = true;

        private const string DefaultFilter = "XML Config file (VersionOne.ServiceHost.exe.config) | VersionOne.ServiceHost.exe.config; VersionOne.ServiceExecutor.exe.config";
        private static readonly string DefaultFileName = Facade.ConfigurationFileNames[0];

        public string HeaderText {
            get { return lblHeader.Text; }
            set { lblHeader.Text = value; }
        }

        public Control CurrentControl {
            get { return pnlControlHolder.Controls.Count > 0 ? pnlControlHolder.Controls[0] : null; }
            set {
                pnlControlHolder.Controls.Clear();
                pnlControlHolder.Controls.Add(value);
                value.Dock = DockStyle.Fill;
            }
        }

        public IConfigurationController Controller {
            get { return controller; }
            set { controller = value; }
        }

        public ConfigurationForm() {
            InitializeComponent();
            // This method must exist to separate part of control init from InitializeComponent().
            // Code generation for SplitContainer contains known defect, and things moved to this one should not be generated.
            PostInitializeComponent();

            Closing += FormClosingHandler;

            tvServices.BeforeSelect += tvServices_BeforeSelect;
            tvServices.AfterSelect += tvServices_AfterSelect;

            miExit.Click += miExit_Click;
            miAbout.Click += miAbout_Click;

            tsbSave.Click += tsbSave_Click;
            miSaveFile.Click += miSaveFile_Click;
            miSaveFileAs.Click += miSaveFileAs_Click;

            miOpenFile.Click += OpenFileClick;
            tsbOpen.Click += OpenFileClick;
        }

        private bool IsCoreService(string serviceKey) {
            return coreServiceNodes.Contains(serviceKey);
        }

        private bool NodeNotSelectable(TreeNode node) {
            return (!coreServicesEnabled && IsCoreService(node.Text)) || nonSelectableNodes.Contains(node.Text);
        }

        private void ExecuteSaveFileAs() {
            using(var saveFileDialog = new SaveFileDialog()) {
                saveFileDialog.Filter = DefaultFilter;
                saveFileDialog.FileName = controller.CurrentFileName;
                saveFileDialog.SupportMultiDottedExtensions = true;
                saveFileDialog.AddExtension = true;
                
                if(saveFileDialog.ShowDialog(this) == DialogResult.OK) {
                    controller.SaveToFile(saveFileDialog.FileName);
                }
            }
        }

        #region Event handlers

        private void tvServices_BeforeSelect(object sender, TreeViewCancelEventArgs e) {
            if (NodeNotSelectable(e.Node)) {
                e.Cancel = true;
                return;
            }

            var isAvailable = controller.ValidatePageAvailability(e.Node.Text);
            e.Cancel = !isAvailable;
        }

        private void tvServices_AfterSelect(object sender, TreeViewEventArgs e) {
            controller.ShowPage(e.Node.Text);
        }

        private void tsbSave_Click(object sender, EventArgs e) {
            controller.SaveToFile(controller.CurrentFileName);
        }

        private void miSaveFile_Click(object sender, EventArgs e) {
            controller.SaveToFile(controller.CurrentFileName);
        }

        //TODO move to controller
        private void miSaveFileAs_Click(object sender, EventArgs e) {
            ExecuteSaveFileAs();
        }

        //TODO move to controller
        private void OpenFileClick(object sender, EventArgs e) {
            if(CheckChanges()) {
                using (var dialog = new OpenFileDialog()) {
                    dialog.Filter = DefaultFilter;
                    dialog.FileName = DefaultFileName;
                    dialog.SupportMultiDottedExtensions = true;
                    dialog.AddExtension = true;
                    if (dialog.ShowDialog(this) == DialogResult.OK) {
                        controller.LoadFromFile(dialog.FileName);
                    }
                }
            }
        }

        /// <summary>
        /// Check changes and if user wanted save them.
        /// </summary>
        /// <returns>False if user pressed Cancel, true otherwise.</returns>
        // TODO refactor
        private bool CheckChanges() {
            if(controller.Settings.HasChanged) {
                var result = MessageBox.Show("Do you want to save changes?", "ServiceHost Settings", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                
                if (result == DialogResult.Cancel) {
                    return false;
                }
  
                if (result == DialogResult.Yes) {
                    ExecuteSaveFileAs();
                }
            }
            return true;
        }

        private void FormClosingHandler(object sender, CancelEventArgs e) {
            if (!CheckChanges()) {
                e.Cancel = true;
            }
        }

        private void miExit_Click(object sender, EventArgs e) {
            Close();
        }

        private void miAbout_Click(object sender, EventArgs e) {
            var assembly = Assembly.GetEntryAssembly();
            var copyrightAttribute = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>();
            var description = string.Format("VersionOne ServiceHost configuration utility, version {0}.\n{1}", controller.ApplicationVersion, copyrightAttribute.Copyright);
            MessageBox.Show(description);
        }
        #endregion

        #region IConfigurationView Members

        public bool NewFileMenuItemEnabled {
            get { return miNewFile.Enabled; }
            set { miNewFile.Enabled = value; }
        }

        public bool OpenFileMenuItemEnabled {
            get { return miOpenFile.Enabled; }
            set { miOpenFile.Enabled = tsbOpen.Enabled = value; }
        }

        public bool SaveFileMenuItemEnabled {
            get { return miSaveFile.Enabled; }
            set { miSaveFile.Enabled = tsbSave.Enabled = value; }
        }

        public bool SaveFileAsMenuItemEnabled {
            get { return miSaveFileAs.Enabled; }
            set { miSaveFileAs.Enabled = value; }
        }

        public bool OptionsMenuItemEnabled {
            get { return miOptions.Enabled; }
            set { miOptions.Enabled = value; }
        }

        public bool GenerateSnapshotMenuItemEnabled {
            get { return miGenerateSnapshot.Enabled; }
            set { miGenerateSnapshot.Enabled = value; }
        }

        public void SetServiceNodesAndRedraw(IEnumerable<string> coreServiceNodes, IEnumerable<string> customServiceNodes) {
            SetCoreServiceNodes(coreServiceNodes);
            SetCustomServiceNodes(customServiceNodes);
            DrawNodes();
        }

        private void SetCoreServiceNodes(IEnumerable<string> nodes) {
            coreServices.Clear();
            if (nodes != null) {
                coreServices.AddRange(nodes);
            }
        }

        private void SetCustomServiceNodes(IEnumerable<string> nodes) {
            customServices.Clear();
            if (nodes != null) {
                customServices.AddRange(nodes);
            }
        }

        private void DrawNodes() {
            tvServices.BeginUpdate();
            
            tvServices.Nodes.Clear();

            var generalNode = new TreeNode("General");
            generalNode.Nodes.AddRange(coreServices.Select(coreService => new TreeNode(coreService)).ToArray());

            var servicesNode = new TreeNode("Services");
            servicesNode.Nodes.AddRange(customServices.Select(customService => new TreeNode(customService)).ToArray());

            tvServices.Nodes.Add(generalNode);
            tvServices.Nodes.Add(servicesNode);
            tvServices.EndUpdate();
            tvServices.ExpandAll();
        }

        public void SetCoreServiceNodesEnabled(bool enabled) {
            coreServicesEnabled = enabled;
            
            foreach (var node in tvServices.Nodes[0].Nodes.Cast<TreeNode>().Where(node => IsCoreService(node.Text))) {
                node.ForeColor = enabled ? SystemColors.ControlText : SystemColors.GrayText;
            }
        }

        public void ShowErrorMessage(string message) {
            MessageBox.Show(this, message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion
    }
}