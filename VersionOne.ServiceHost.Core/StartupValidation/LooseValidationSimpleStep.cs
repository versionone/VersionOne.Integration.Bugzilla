using System;

namespace VersionOne.ServiceHost.Core.StartupValidation {
    public class LooseValidationSimpleStep: IValidationStep {
        [HasDependencies]
        public ISimpleResolver Resolver { get; set; }

        [HasDependencies]
        public ISimpleValidator Validator { get; set; }

        public LooseValidationSimpleStep(ISimpleValidator validator, ISimpleResolver resolver) {
            Validator = validator;
            Resolver = resolver;
        }

        public void Run() {
            if(Validator == null) {
                throw new InvalidOperationException("Cannot run the step without a validator.");
            }

            Validator.TreatErrorsAsWarnings = true;

            if(Resolver != null) {
                Resolver.TreatErrorsAsWarnings = true;
            }

            var isValid = Validator.Validate();

            if(!isValid && Resolver != null){
                Resolver.Resolve();
            }
        }
    }
}