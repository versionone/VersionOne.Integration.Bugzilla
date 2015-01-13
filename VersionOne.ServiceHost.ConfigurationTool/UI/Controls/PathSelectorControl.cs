using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms;

namespace VersionOne.ServiceHost.ConfigurationTool.UI.Controls {
    public partial class PathSelectorControl : UserControl {
        public DialogTypes DialogType { get ; set ;}

        public string SelectedPath {
            get { return txtWatchPath.Text.Trim(); }
            set { txtWatchPath.Text = value; }
        }

        public int TextBoxLeft {
            get { return txtWatchPath.Left; }
            set {
                txtWatchPath.Width = txtWatchPath.Width + txtWatchPath.Left - value;
                txtWatchPath.Left = value;
            }
        }

        public PathSelectorControl() {
            InitializeComponent();

            btnBrowse.Click += btnBrowse_Click;

            DialogType = DialogTypes.File;
        }

        public void AddControlBinding(object dataSource, string dataMember) {
            txtWatchPath.DataBindings.Add("Text", dataSource, dataMember, false, DataSourceUpdateMode.OnPropertyChanged);
        }

        public void AddControlValidation(string entityPropertyName, ValidationProvider validationProvider) {
            validationProvider.SetSourcePropertyName(txtWatchPath, entityPropertyName);
            validationProvider.SetValidatedProperty(txtWatchPath, "Text");
            validationProvider.SetPerformValidation(txtWatchPath, true);
        }

        private void btnBrowse_Click(object sender, EventArgs e) {
            switch(DialogType) {
                case DialogTypes.Folder: 
                    SelectFolder();
                    break;
                case DialogTypes.File:
                    SelectFile();
                    break;
            }
        }

        private void SelectFile() {
            var dialog = new OpenFileDialog();

            if(!string.IsNullOrEmpty(txtWatchPath.Text) && Directory.Exists(txtWatchPath.Text)) {
                dialog.FileName = txtWatchPath.Text;
            }

            if(dialog.ShowDialog(this) == DialogResult.OK) {
                txtWatchPath.Text = dialog.FileName;
                ValidateChildren();
            }            
        }

        private void SelectFolder() {
            var dialog = new FolderBrowserDialog { Description = "Please select a folder" };

            if(!string.IsNullOrEmpty(txtWatchPath.Text) && Directory.Exists(txtWatchPath.Text)) {
                dialog.SelectedPath = txtWatchPath.Text;
            }

            if(dialog.ShowDialog(this) == DialogResult.OK) {
                txtWatchPath.Text = dialog.SelectedPath;
                ValidateChildren();
            }
        }

        public enum DialogTypes {
            Folder,
            File,
        }
    }
}
