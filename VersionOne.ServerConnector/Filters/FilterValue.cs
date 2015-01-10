namespace VersionOne.ServerConnector.Filters {
    public class FilterValue {
        public object Value { get; set;}
        public FilterValuesActions Action { get; set; }

        public FilterValue(object value, FilterValuesActions action) {
            Value = value;
            Action = action;
        }
    }
}