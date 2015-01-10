using System;

namespace VersionOne.ServiceHost.Core.StartupValidation {
    public class ValidationSimpleStep : IValidationStep {
        [HasDependencies]
        public ISimpleResolver Resolver { get; set; }

        [HasDependencies]
        public ISimpleValidator Validator { get; set; }

        public ValidationSimpleStep(ISimpleValidator validator) : this(validator, null) {}

        public ValidationSimpleStep(ISimpleValidator validator, ISimpleResolver resolver) {
            Validator = validator;
            Resolver = resolver;
        }

        public void Run() {
            if(Validator == null) {
                throw new InvalidOperationException("Cannot run the step without a validator.");
            }

            var isValid = Validator.Validate();

            if(!isValid && (Resolver == null || Resolver != null && !Resolver.Resolve())) {
                throw new ValidationException("Validation error during service initialization.");
            }
        }
    }
}