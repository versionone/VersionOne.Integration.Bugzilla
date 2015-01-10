using System.Diagnostics;

namespace VersionOne.ServerConnector.Entities {
    [DebuggerDisplay("[{Name}, {Value}]")]
    public class ListValue {
        public const string NameProperty = "Name";
        public const string ValueProperty = "Value";

        public string Name { get; private set; }

        public string Value { get; private set; }

        /// <summary>
        /// Create list value entity.
        /// </summary>
        /// <param name="name">Text representation usually displayed on VersionOne UI. For example, it could be &quot;Passed&quot; for TestStatus</param>
        /// <param name="value">Token (OID) of corresponding value used internally, e. g. TestStatus:138.</param>
        public ListValue(string name, string value) {
            Name = name;
            Value = value;
        }
    }
}