namespace VersionOne.ServerConnector {
    internal class MultipleAttributeValue : AttributeValue {
        internal readonly object[] Values;
        
        public MultipleAttributeValue(string name, object[] values) : base(name) {
            Values = values;
        }
    }
}