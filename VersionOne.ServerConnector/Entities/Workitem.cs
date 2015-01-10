using System;
using System.Collections.Generic;
using VersionOne.SDK.APIClient;

namespace VersionOne.ServerConnector.Entities {
    public abstract class Workitem : Entity {
        public const string OwnersProperty = "Owners";
        public const string NumberProperty = "Number";
        public const string EstimateProperty = "Estimate";
        public const string PriorityProperty = "Priority";
        public const string ChangeDateUtcProperty = "ChangeDateUTC";
        public const string ScopeProperty = "Scope";
        public const string AssetStateProperty = "AssetState";
        public const string ScopeNameProperty = "Scope.Name";
        public const string IdProperty = "ID";

        public string Number { get { return GetProperty<string>(NumberProperty); } }

        public string Description {
            get { return GetProperty<string>(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }

        public string Reference {
            get { return GetProperty<string>(ReferenceProperty); }
            set { SetProperty(ReferenceProperty, value); }
        }

        public double? Estimate {
            get { return GetProperty<double?>(EstimateProperty); }
            set { SetProperty(EstimateProperty, value);}
        }

        public DateTime ChangeDateUtc {
            get { return GetProperty<DateTime>(ChangeDateUtcProperty); }
            set { SetProperty(ChangeDateUtcProperty, value); }
        }

        public bool IsClosed {
            get { return GetProperty<byte>(AssetStateProperty) == 128; }
        }
        
        public string PriorityToken {
            get {
                var oid = GetProperty<Oid>(PriorityProperty);
                return oid.IsNull ? null : oid.Momentless.Token;
            }
            set {
                var priority = ListValues[VersionOneProcessor.WorkitemPriorityType].Find(value);
                
                if (priority != null) {
                    SetProperty(PriorityProperty, priority.Oid);
                }
            }
        }

        public IList<Member> Owners { get; protected set; }

        public KeyValuePair<string, string> Project {
            get { return new KeyValuePair<string, string>(GetProperty<Oid>(ScopeProperty).Momentless.ToString(), GetProperty<string>(ScopeNameProperty)); }
        }

        internal Workitem(Asset asset, IDictionary<string, PropertyValues> listValues, IList<Member> owners, IEntityFieldTypeResolver typeResolver) 
                : this(asset, listValues, typeResolver) {
            Owners = owners;
        }

        internal Workitem(Asset asset, IDictionary<string, PropertyValues> listValues, IEntityFieldTypeResolver typeResolver) : this(asset, typeResolver) {
            ListValues = listValues;
        }

        private Workitem(Asset asset, IEntityFieldTypeResolver typeResolver) : base(asset, typeResolver) {}

        protected Workitem() { }

        internal static Workitem Create(Asset asset, IDictionary<string, PropertyValues> listPropertyValues, IEntityFieldTypeResolver typeResolver, IList<Member> owners = null) {
            switch(asset.AssetType.Token) {
                case VersionOneProcessor.StoryType:
                case VersionOneProcessor.DefectType:
                    return PrimaryWorkitem.Create(asset, listPropertyValues, typeResolver, owners);
                case VersionOneProcessor.TaskType:
                    return new Task(asset, listPropertyValues, owners, typeResolver);
                case VersionOneProcessor.TestType:
                    return new Test(asset, listPropertyValues, owners, typeResolver);
                default:
                    throw new NotSupportedException("Type " + asset.AssetType.Token + " is not supported in factory method");
            }
        }
    }
}