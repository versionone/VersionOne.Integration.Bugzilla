namespace VersionOne.ServiceHost.Core.StartupValidation {
    public class ValidationResult<T> {
        public T Target { get; private set; }
        
        public ValidationResult(T target) {
            Target = target;
        }
    
        public ValidationResult() {
            Target = default(T);
        }
    }
}