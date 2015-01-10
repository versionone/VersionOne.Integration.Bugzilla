using System.Collections.Generic;
using System.Diagnostics;
using VersionOne.SDK.APIClient;

namespace VersionOne.ServerConnector.Entities {
    [DebuggerDisplay("{TypeName} {Name}, Id={Id}")]
    // TODO resolve TypeName and TypeToken clashes, if any
    public abstract class Entity : BaseEntity {
        public const string NameProperty = "Name";        
        public const string CustomPrefix = "Custom_";
        public const string SourceNameProperty = "Source.Name";        
        public const string StatusNameProperty = "Status.Name";
        public const string DescriptionProperty = "Description";
        public const string StatusProperty = "Status";

        //TODO decide where we need to keep these constants.
        public const string ParentProperty = "Parent";
        public const string ParentAndUpProperty = "ParentAndUp";
        public const string InactiveProperty = "Inactive";
        public const string ScopeParentAndUpProperty = "Scope.ParentMeAndUp";

        public string Id { get; protected set; }
        public string TypeName { get; protected set; }

        protected internal IDictionary<string, PropertyValues> ListValues { get; set; }

        public string Name {
            get { return GetProperty<string>(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }

        protected internal IEntityFieldTypeResolver TypeResolver;

        internal Entity(Asset asset, IEntityFieldTypeResolver typeResolver) : base(asset) {
            Id = asset.Oid.Momentless.ToString();
            TypeName = asset.AssetType.Token;
            TypeResolver = typeResolver;
        }

        protected Entity() { }

        public virtual bool HasChanged() {
            return Asset.HasChanged;
        }

        public string GetCustomFieldValue(string fieldName) {
            fieldName = NormalizeCustomFieldName(fieldName);
            var value = GetProperty<object>(fieldName);

            if (value != null && value is Oid && ((Oid)value).IsNull) {
                return null;
            }

            return value != null ? value.ToString() : null;
        }

        public ValueId GetListValue(string fieldName) {
            var value = GetProperty<Oid>(fieldName);
            var type = TypeResolver.Resolve(TypeToken, fieldName);
            return ListValues[type].Find(value.Token);            
        }

        public ValueId GetCustomListValue(string fieldName) {
            fieldName = NormalizeCustomFieldName(fieldName);
            return GetListValue(fieldName);
        }

        public void SetCustomListValue(string fieldName, string value) {
            var type = TypeResolver.Resolve(TypeToken, fieldName);
            var valueData = ListValues[type].Find(value);

            if (valueData != null) {
                SetProperty(fieldName, valueData.Oid);
            }
        }

        private static string NormalizeCustomFieldName(string fieldName) {
            return fieldName.StartsWith(CustomPrefix) ? fieldName : CustomPrefix + fieldName;
        }

        public void SetCustomNumericValue(string fieldName, double value) {
            SetProperty(fieldName, value);
        }

        public void SetCustomStringValue(string fieldName, string value) {
            SetProperty(fieldName, value);
        }

        public double? GetCustomNumericValue(string fieldName) {
            return GetProperty<double?>(fieldName);
        }

        public const string AssetTypeProperty = "AssetType";
    }
}