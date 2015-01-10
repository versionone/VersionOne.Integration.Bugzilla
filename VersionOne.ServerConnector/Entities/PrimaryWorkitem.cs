using System;
using System.Collections.Generic;
using VersionOne.SDK.APIClient;

namespace VersionOne.ServerConnector.Entities {
    public class PrimaryWorkitem : Workitem {        
        public const string TeamNameProperty = "Team.Name";
        public const string ParentNameProperty = "Parent.Name";
        public const string SprintNameProperty = "Timebox.Name";
        public const string OrderProperty = "Order";
        public const string CompletedInBuildRunsProperty = "CompletedInBuildRuns";

        public string FeatureGroupName {
            get { return GetProperty<string>(ParentNameProperty); }
        }

        public string Team {
            get { return GetProperty<string>(TeamNameProperty); }
        }

        public string SprintName {
            get { return GetProperty<string>(SprintNameProperty); }
        }

        public int Order {
            get {
                int order;
                int.TryParse(GetProperty<Rank>(OrderProperty).ToString(), out order);
                return order;
            }
        }

        public ValueId Status {
            get { return GetListValue(StatusProperty); }
            set { SetCustomListValue(StatusProperty, value.Token); }
        }

        public override string TypeToken {
            get { return VersionOneProcessor.PrimaryWorkitemType; }
        }

        public ValueId[] CompletedInBuildRuns {
            get { return GetMultiValueProperty(CompletedInBuildRunsProperty); }
            set { SetMultiValueProperty(CompletedInBuildRunsProperty, value); }
        }

        internal protected PrimaryWorkitem(Asset asset, IDictionary<string, PropertyValues> listValues, IEntityFieldTypeResolver typeResolver, IList<Member> owners = null) 
            : base(asset, listValues, owners, typeResolver) { }

        internal protected PrimaryWorkitem() { }

        internal static new PrimaryWorkitem Create(Asset asset, IDictionary<string, PropertyValues> listPropertyValues, IEntityFieldTypeResolver typeResolver, IList<Member> owners = null) {
            switch(asset.AssetType.Token) {
                case VersionOneProcessor.StoryType:
                    return new Story(asset, listPropertyValues, typeResolver, owners);
                case VersionOneProcessor.DefectType:
                    return new Defect(asset, listPropertyValues, typeResolver, owners);
                default:
                    throw new NotSupportedException("Type " + asset.AssetType.Token + " is not supported in factory method");
            }
        }
    }
}