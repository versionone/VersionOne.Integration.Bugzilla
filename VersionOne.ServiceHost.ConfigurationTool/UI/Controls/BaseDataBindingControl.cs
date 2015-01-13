using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using VersionOne.ServiceHost.ConfigurationTool.Attributes;
using VersionOne.ServiceHost.ConfigurationTool.BZ;

namespace VersionOne.ServiceHost.ConfigurationTool.UI.Controls {
    public class BaseDataBindingControl : UserControl {        
        private const DataSourceUpdateMode UpdateMode = DataSourceUpdateMode.OnPropertyChanged;
        
        private HelpProvider helpProvider;
        private const int HelpButtonOffset = 15;

        protected internal readonly List<DataBindingPoint> DataBindingPoints = new List<DataBindingPoint>();

        protected IEnumerable<Control> DataBoundControls {
            get { return DataBindingPoints.Select(item => item.Control).ToList(); }
        }

        protected BaseDataBindingControl() {
            InitializeComponent();
        }

        protected void AddControlBinding(Control control, object dataSource, string dataMember) {
            string controlProperty;

            if(control.GetType() == typeof(TextBox)) {
                controlProperty = "Text";
            } else if(control.GetType() == typeof(CheckBox)) {
                controlProperty = "Checked";
            } else if(control.GetType() == typeof(NumericUpDown)) {
                controlProperty = "Value";
            } else if(control.GetType() == typeof(ComboBox)) {
                controlProperty = "SelectedValue";
            } else {
                throw new NotSupportedException("This type of control is not supported.");
            }

            control.DataBindings.Add(controlProperty, dataSource, dataMember, false, UpdateMode);
            DataBindingPoints.Add(new DataBindingPoint(control, dataSource, dataMember));
        }

        protected void AddSimpleComboboxBinding(ComboBox comboBox, object dataSource, string dataMember) {
            comboBox.DataBindings.Add("SelectedItem", dataSource, dataMember, false, UpdateMode);
            DataBindingPoints.Add(new DataBindingPoint(comboBox, dataSource, dataMember));
        }
 
        protected static void FillComboBoxWithStrings(ComboBox comboBox, IEnumerable<string> values) {
            comboBox.Items.Clear();

            foreach (var value in values) {
                comboBox.Items.Add(value);
            }
        }

        protected static void FillComboBoxWithListValues(ComboBox comboBox, IEnumerable<ListValue> values) {
            comboBox.DisplayMember = ListValue.NameProperty;
            comboBox.ValueMember = ListValue.ValueProperty;
            comboBox.DataSource = values;
        }

        private void InitializeComponent() {
            helpProvider = new HelpProvider();
            SuspendLayout();
            // 
            // BaseDataBindingControl
            // 
            Name = "BaseDataBindingControl";
            ResumeLayout(false);
        }

        #region Help

        protected void AddHelpSupport(Control control, object actualModel, string propertyName) {
            AddHelpSupport(control, actualModel, propertyName, HelpButtonOffset);
        }

        protected void AddHelpSupport(Control control, object actualModel, string propertyName, int offset) {
            var properties = TypeDescriptor.GetProperties(actualModel);
            var attributes = properties[propertyName].Attributes;
            var helpStringAttribute = (HelpStringAttribute) attributes[typeof(HelpStringAttribute)];

            if(helpStringAttribute == null) {
                return;
            }

            var content = helpStringAttribute.Content;

            if(!string.IsNullOrEmpty(helpStringAttribute.HelpResourceKey)) {
                content = HelpResources.ResourceManager.GetString(helpStringAttribute.HelpResourceKey);
            }

            if(string.IsNullOrEmpty(content)) {
                return;
            }

            helpProvider.SetHelpString(control, content);
            helpProvider.SetShowHelp(control, true);
            
            var helpButton = new HelpButton(control, helpProvider);
            helpButton.Location = CalculateHelpButtonLocation(control, helpButton, offset);
            control.Parent.Controls.Add(helpButton);
            control.Parent.Controls.SetChildIndex(helpButton, control.Parent.Controls.IndexOf(control) + 1);
            helpButton.TabIndex = control.TabIndex + 1;
        }

        private static Point CalculateHelpButtonLocation(Control control, Control helpButton, int offset) {
            return new Point(control.Right + offset, control.Top + control.Height / 2 - helpButton.Height / 2);
        }

        #endregion

        protected internal class DataBindingPoint {
            internal readonly Control Control;
            internal readonly object DataSource;
            internal readonly string PropertyName;

            public DataBindingPoint(Control control, object dataSource, string propertyName) {
                Control = control;
                DataSource = dataSource;
                PropertyName = propertyName;
            }
        }
    }
}