using System.Xml.Serialization;

namespace VersionOne.ServiceHost.ConfigurationTool.Entities {
    public class NullableBool {
        private bool boolValue;

        public const string StringValueProperty = "StringValue";
        public const string BoolValueProperty = "BoolValue";

        [XmlText]
        public string StringValue {
            get { return HasValue ? boolValue.ToString().ToLowerInvariant() : null; }
            set {
                bool boolVal;
                HasValue = bool.TryParse(value, out boolVal);
                boolValue = boolVal;
            }
        }

        [XmlIgnore]
        public bool BoolValue {
            get { return boolValue; }
            set {
                HasValue = true;
                boolValue = value;
            }
        }

        [XmlIgnore]
        public bool HasValue { get; private set; }

        public NullableBool() { }

        public NullableBool(bool value) {
            BoolValue = value;
        }

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType()) {
                return false;
            }

            var other = (NullableBool) obj; 
            return BoolValue == other.BoolValue && HasValue == other.HasValue;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public override string ToString() {
            return StringValue;
        }

        public static bool Equals (NullableBool a, NullableBool b) {
            if(a == null || b == null) {
                return false;
            }

            return a.BoolValue == b.BoolValue && a.HasValue == b.HasValue;
        }
    }
}