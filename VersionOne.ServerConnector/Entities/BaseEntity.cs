using System.Collections;
using System.Linq;
using VersionOne.SDK.APIClient;

namespace VersionOne.ServerConnector.Entities {
    public abstract class BaseEntity {
        public const string ReferenceProperty = "Reference";

        internal readonly Asset Asset;

        public virtual string TypeToken { get { return Asset.AssetType.Token; } }

        internal BaseEntity(Asset asset) {
            Asset = asset;
        }

        protected BaseEntity() { }

        public virtual T GetProperty<T>(string name) {
            var attributeDefinition = Asset.AssetType.GetAttributeDefinition(name);
            return (T) (Asset.GetAttribute(attributeDefinition) != null ? Asset.GetAttribute(attributeDefinition).Value : null);
        }

        protected virtual void SetProperty<T>(string name, T value) {
            var attributeDefinition = Asset.AssetType.GetAttributeDefinition(name);

            if(value is BaseEntity) {
                var entity = value as BaseEntity;
                Asset.SetAttributeValue(attributeDefinition, entity.Asset.Oid.Momentless);
            } else {
                Asset.SetAttributeValue(attributeDefinition, value);
            }
        }

        protected virtual ValueId[] GetMultiValueProperty(string name) {
            var attributeDefinition = Asset.AssetType.GetAttributeDefinition(name);
            var attribute = Asset.GetAttribute(attributeDefinition);
            return attribute == null  || attribute.Values == null ? new ValueId[0] : ConvertEnumerable(attribute.Values);
        }

        protected virtual void SetMultiValueProperty(string name, ValueId[] values) {
            var attributeDefinition = Asset.AssetType.GetAttributeDefinition(name);
            var attribute = Asset.GetAttribute(attributeDefinition);

            foreach (Oid value in attribute.Values) {
                Asset.RemoveAttributeValue(attributeDefinition, value);
            }

            (values ?? new ValueId[0]).ToList().ForEach(x => Asset.AddAttributeValue(attributeDefinition, x.Oid.Momentless));
        }

        private static ValueId[] ConvertEnumerable(IEnumerable source) {
            return source.Cast<Oid>().Select(x => new ValueId(x, null)).ToArray();
        }
    }
}