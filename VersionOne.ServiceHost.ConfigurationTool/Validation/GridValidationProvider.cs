using System;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration;

namespace VersionOne.ServiceHost.ConfigurationTool.Validation {
    // TODO impl IExtenderProvider & expose EnableValidation property
    [ToolboxItemFilter("System.Windows.Forms")]
    public class GridValidationProvider : Component {
        private string ruleset = string.Empty;
        private ValidationSpecificationSource specificationSource = ValidationSpecificationSource.Both;
        private DataGridView grid;
        private bool performValidation = true;

        private IValidationInterceptor validationInterceptor;

        public string Ruleset {
            get { return ruleset; }
            set { ruleset = value; }
        }

        public ValidationSpecificationSource SpecificationSource {
            get { return specificationSource; }
            set { specificationSource = value; }
        }

        public DataGridView Grid {
            get { return grid; }
            set {
                if(grid != null) {
                    grid.CellValidating -= OnCellValidating;
                    grid.CellValidated -= OnCellValidated;
                } 

                grid = value;
                grid.CellValidating += OnCellValidating;
                grid.CellValidated += OnCellValidated;
            }
        }

        public Type ValidatedType { get; set; }

        public bool PerformValidation {
            get { return performValidation; }
            set { performValidation = value; }
        }

        public void RegisterValidationInterceptor(IValidationInterceptor interceptor) {
            validationInterceptor = interceptor;
        }

        private void NotifyValidationInterceptor() {
            if(validationInterceptor != null) {
                validationInterceptor.NotifyOnValidation();
            }
        }

        private void OnCellValidating(object source, DataGridViewCellValidatingEventArgs e) {
            var row = Grid.Rows[e.RowIndex];
            
            if (row.IsNewRow) {
                return;
            }

            var validationProxy = new GridValidationIntegrationProxy(Grid.Columns[e.ColumnIndex].DataPropertyName, e.FormattedValue, this);

            ValidationIntegrationHelper helper;

            try {
                helper = new ValidationIntegrationHelper(validationProxy);
            } catch (InvalidOperationException) {
                // ignore missing property
                return;
            }

            var validator = helper.GetValidator();
            
            if(validator == null) {
                // this property should not be checked
                return;
            }

            var validationResults = validator.Validate(validationProxy);

            if (!validationResults.IsValid) {
                var builder = new StringBuilder();
                
                foreach (ValidationResult result in validationResults) {
                    builder.AppendLine(result.Message);
                }

                row.Cells[e.ColumnIndex].ErrorText = builder.ToString();
            } else {
                row.Cells[e.ColumnIndex].ErrorText = string.Empty;
            }
        }

        private void OnCellValidated(object sender, DataGridViewCellEventArgs e) {
            NotifyValidationInterceptor();
        }

        protected override void Dispose(bool disposing) {
            grid.CellValidating -= OnCellValidating;
            grid.CellValidated -= OnCellValidated;
            base.Dispose(disposing);
        }
    }
}