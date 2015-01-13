namespace VersionOne.ServiceHost.ConfigurationTool.BZ {
    public class PropertyValidationResult {
        public readonly string PropertyName;
        public readonly object Entity;

        public PropertyValidationResult(string propertyName, object entity) {
            PropertyName = propertyName;
            Entity = entity;
        }
    }
}