using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms;

using VersionOne.ServiceHost.ConfigurationTool.Validation;

namespace VersionOne.ServiceHost.ConfigurationTool.UI.Controls {
    /// <summary>
    /// Base page control that all tab pages must inherit from. 
    /// Should be abstract class but Form Designer cannot handle it gracefully and won't open successor controls.
    /// </summary>
    public abstract class BasePageControl<TModel> : BaseDataBindingControl, IValidationInterceptor {
        private IContainer components;

        protected ErrorProvider ErrorProvider { get; private set; }
        protected TabHighlighter TabHighlighter { get; private set; }

        private readonly IDictionary<Type, ValidationProvider> validationProviders = new Dictionary<Type, ValidationProvider>();
        private readonly IDictionary<Type, GridValidationProvider> gridValidationProviders = new Dictionary<Type, GridValidationProvider>();

        private readonly ICollection<Control> ValidatedControls = new List<Control>();

        public event EventHandler ControlValidationTriggered;

        public TModel Model { get; set; }

        protected void InvokeValidationTriggered() {
            if(ControlValidationTriggered != null) {
                ControlValidationTriggered(this, EventArgs.Empty);
            }
        }

        public abstract void DataBind();

        protected BasePageControl() {
            InitializeComponent();
            AddValidationProvider(typeof(TModel));
        }

        protected void AddTabHighlightingSupport(TabControl tabControl) {
            TabHighlighter = new TabHighlighter(tabControl);
            components.Add(TabHighlighter);
        }

        /// <summary>
        /// Layout, controls and components initialization. Should not be modified.
        /// </summary>
        private void InitializeComponent() {
            components = new Container();
            ErrorProvider = new ErrorProvider(components);
            ((ISupportInitialize)(ErrorProvider)).BeginInit();
            SuspendLayout();
            // 
            // errorProvider
            // 
            ErrorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            ErrorProvider.ContainerControl = this;
            // 
            // BasePageControl
            // 
            AutoValidate = AutoValidate.EnableAllowFocusChange;
            Name = "BasePageControl";
            ((ISupportInitialize)(ErrorProvider)).EndInit();
            ResumeLayout(false);
        }

        public virtual void CommitPendingChanges() {
            foreach(var control in DataBoundControls) {
                control.SelectNextControl(control, true, false, false, true);
            }

            foreach(Control control in Controls) {
                CommitGridChangesRecursively(control);
            }
        }

        private static void CommitGridChangesRecursively(Control control) {
            if(control is DataGridView) {
                var grid = (DataGridView)control;
                grid.EndEdit();

                // NOTE this is a way to fix issue with saving empty rows if a cell in new line is selected (B-02082)
                grid.ClearSelection();
                grid.CurrentCell = null;

                if(grid.Rows.Count > 0) {
                    if(!grid.Rows[0].IsNewRow && grid.Rows[0].Cells.Count > 0) {
                        grid.CurrentCell = grid.Rows[0].Cells[0];
                    }
                }

                MoveFocus(grid);
                
                return;
            }

            foreach(Control child in control.Controls) {
                CommitGridChangesRecursively(child);
            }
        }

        private static void MoveFocus(DataGridView grid) {
            foreach(Control control in grid.Parent.Controls) {
                if(!control.CanFocus) {
                    continue;
                }

                if(control.GetType() == typeof(DataGridView)) {
                    continue;
                }

                control.Focus();
                break;
            }
        }

        public void DisplayError(string message) {
            MessageBox.Show(this, message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #region Validation

        protected void AddValidationProvider(Type entityType) {
            var provider = new ValidationProvider {
                ErrorProvider = ErrorProvider,
                RulesetName = string.Empty,
                SourceTypeName = entityType.AssemblyQualifiedName,
            };
            components.Add(provider);
            validationProviders.Add(entityType, provider);
        }

        protected void AddGridValidationProvider(Type entityType, DataGridView grid) {
            var provider = new GridValidationProvider {
                Grid = grid,
                ValidatedType = entityType,
                PerformValidation = true
            };
            provider.RegisterValidationInterceptor(this);
            gridValidationProviders.Add(entityType, provider);
        }

        protected void ValidateDataBoundControl(Control control) {
            if(control.DataBindings.Count < 1 || control.DataBindings[0].DataSource == null) {
                throw new InvalidOperationException("Control is not data bound, or its datasource is null");
            }

            var dataSource = control.DataBindings[0].DataSource;
            validationProviders[dataSource.GetType()].PerformValidation(control);
        }

        protected void AddControlTextValidation<TEntity>(Control control, string entityPropertyName) {
            AddControlValidation<TEntity>(control, entityPropertyName, "Text");
        }

        protected void AddControlValidation<TEntity>(Control control, string entityPropertyName, string controlPropertyName) {
            if(ValidatedControls.Contains(control)) {
                return;
            }

            var provider = validationProviders[typeof(TEntity)];

            ValidatedControls.Add(control);
            provider.SetSourcePropertyName(control, entityPropertyName);
            provider.SetValidatedProperty(control, controlPropertyName);
            provider.SetPerformValidation(control, true);
            RegisterControlForValidationNotification(control);
        }

        protected void RemoveControlValidation<TEntity>(Control control) {
            if(!ValidatedControls.Contains(control)) {
                return;
            }

            var provider = validationProviders[typeof(TEntity)];

            ValidatedControls.Remove(control);
            provider.SetPerformValidation(control, false);
            UnregisterControlValidationNotification(control);
        }

        private void HandleControlValidating(object sender, CancelEventArgs e) {
            NotifyOnValidation();
        }

        private void RegisterControlForValidationNotification(Control control) {
            control.Validating += HandleControlValidating;
        }

        private void UnregisterControlValidationNotification(Control control) {
            control.Validating += HandleControlValidating;
        }

        #endregion

        protected static bool ConfirmDelete() {
            var result = MessageBox.Show("Are you sure you want to delete item?", "Please confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            return result == DialogResult.OK;
        }

        public void NotifyOnValidation() {
            InvokeValidationTriggered();
        }

        protected ValidationProvider GetValidationProvider(Type entity) {
            return validationProviders[entity];
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            ValidateChildren();
        }
    }
}