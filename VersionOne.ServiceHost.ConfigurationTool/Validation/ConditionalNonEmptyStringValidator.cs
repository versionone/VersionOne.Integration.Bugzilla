using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms;

namespace VersionOne.ServiceHost.ConfigurationTool.Validation {
    public class ConditionalNonEmptyStringValidator : NonEmptyStringValidator {
        private readonly string conditionalPropertyName;
        private readonly bool skipOnFalse;

        public ConditionalNonEmptyStringValidator(string messageTemplate, string conditionalPropertyName, bool skipOnFalse) : base(messageTemplate) {
            this.conditionalPropertyName = conditionalPropertyName;
            this.skipOnFalse = skipOnFalse;
        }

        /// <summary>
        /// Execute conditional validation. 
        /// If this method is called via WinForms integration layer, <paramref name="currentTarget"/> is ValidatedControlItem instance.
        /// When we explicitly validate entities using EL Validation facade, <paramref name="currentTarget"/> contains entity instance.
        /// </summary>
        protected override void DoValidate(string objectToValidate, object currentTarget, string key, ValidationResults validationResults) {
            var modelResolved = currentTarget;

            if(currentTarget is ValidatedControlItem) {
                var validatedItem = (ValidatedControlItem)currentTarget;
                modelResolved = validatedItem.Control.DataBindings[0].DataSource;
            }

            var propertyInfo = modelResolved.GetType().GetProperty(conditionalPropertyName, typeof(bool));
            var propertyValue = (bool) propertyInfo.GetValue(modelResolved, null);

            if(propertyValue == skipOnFalse) {
                base.DoValidate(objectToValidate, currentTarget, key, validationResults);
            }
        }
    }
}