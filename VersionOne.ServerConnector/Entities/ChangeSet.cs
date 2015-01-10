using VersionOne.SDK.APIClient;

namespace VersionOne.ServerConnector.Entities {
    public class ChangeSet : Entity {
        public const string PrimaryWorkitemsProperty = "PrimaryWorkitems";

        public override string TypeToken {
            get { return VersionOneProcessor.ChangeSetType; }
        }

        internal ChangeSet(Asset asset) : base(asset, null) { }

        public string Reference {
            get { return GetProperty<string>(ReferenceProperty); }
            set { SetProperty(ReferenceProperty, value);}
        }

        public string Description {
            get { return GetProperty<string>(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value);}
        }

        public ValueId[] PrimaryWorkitems {
            get { return GetMultiValueProperty(PrimaryWorkitemsProperty); }
            set { SetMultiValueProperty(PrimaryWorkitemsProperty, value);}
        }
    }
}
