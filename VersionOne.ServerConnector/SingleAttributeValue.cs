namespace VersionOne.ServerConnector {
    internal class SingleAttributeValue : AttributeValue {
        internal readonly object Value;

        internal SingleAttributeValue(string name, object value) : base(name) {
            Value = value;
        }
    }
}