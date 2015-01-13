using System;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration;

namespace VersionOne.ServiceHost.ConfigurationTool.Validation {
    /// <summary>
    /// A simple implementation of validation integration proxy. DataGridView control structure is dynamic and complex, so we cannot just map controls to properties.
    /// </summary>
    internal class GridValidationIntegrationProxy : IValidationIntegrationProxy {
        private readonly GridValidationProvider validationProvider;
        private readonly string validatedPropertyName;
        private readonly object valueToValidate;

        public GridValidationIntegrationProxy(string propertyName, object valueToValidate, GridValidationProvider provider) {
            validationProvider = provider;
            this.valueToValidate = valueToValidate;
            validatedPropertyName = propertyName;
        }
        
        public void PerformCustomValueConversion(ValueConvertEventArgs e) { }

        public object GetRawValue() {
            return valueToValidate;
        }

        public MemberValueAccessBuilder GetMemberValueAccessBuilder() {
            return new GridMemberValueAccessBuilder();
        }

        public string Ruleset {
            get { return validationProvider.Ruleset; }
        }

        public ValidationSpecificationSource SpecificationSource {
            get { return validationProvider.SpecificationSource; }
        }

        public bool ProvidesCustomValueConversion {
            get { return false; }
        }

        public string ValidatedPropertyName {
            get { return validatedPropertyName; }
        }

        public Type ValidatedType {
            get { return validationProvider.ValidatedType; }
        }
    }
}