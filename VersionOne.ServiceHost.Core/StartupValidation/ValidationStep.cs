using System;
using System.Linq;

namespace VersionOne.ServiceHost.Core.StartupValidation {
    public class ValidationStep<TValidator, TValidationResult, TResolver> : IValidationStep
            where TValidator : class, IValidator<TValidationResult>
            where TResolver : class, IResolver<TValidationResult> {
        [HasDependencies]
        public TValidator Validator { get; set; }

        [HasDependencies]
        public TResolver Resolver { get; set; }

        public ValidationStep(TValidator validator, TResolver resolver) {
            Validator = validator;
            Resolver = resolver;
        }

        public void Run() {
            if(Validator == null) {
                throw new InvalidOperationException("Cannot run the step without a validator");
            }

            var results = Validator.Validate();

            if(!results.IsValid && Resolver == null || (Resolver != null && !Resolver.Resolve(results.Items.Select(x => x.Target).ToList()))) {
                throw new ValidationException("Validation error during service initialization");
            }
        }
    }
}