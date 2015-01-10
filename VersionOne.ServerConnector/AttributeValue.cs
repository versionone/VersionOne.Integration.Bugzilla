namespace VersionOne.ServerConnector {
    public abstract class AttributeValue {
        internal readonly string Name;

        protected AttributeValue(string name) {
            Name = name;
        }

        internal static AttributeValue Single(string name, object value) {
            return new SingleAttributeValue(name, value);
        }

        internal static AttributeValue Multi(string name, params object[] values) {
            return new MultipleAttributeValue(name, values);
        }
    }
}