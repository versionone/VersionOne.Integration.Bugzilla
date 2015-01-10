using System;
using System.Collections.Generic;
using VersionOne.SDK.APIClient;

namespace VersionOne.ServerConnector.Entities {
    public class BuildRun : Entity {
        public const string ElapsedProperty = "Elapsed";
        public const string DateProperty = "Date";
        public const string BuildProjectProperty = "BuildProject";
        public const string ChangeSetsProperty = "ChangeSets";

        public override string TypeToken {
            get { return VersionOneProcessor.BuildRunType; }
        }

        internal BuildRun(Asset asset, IDictionary<string, PropertyValues> listValues, IEntityFieldTypeResolver typeResolver) : base(asset, typeResolver) {
            ListValues = listValues;
        }

        // TODO impl. getter properly
        public ValueId Status {
            get { return new ValueId(GetProperty<Oid>(StatusProperty), string.Empty); }
            set { SetProperty(StatusProperty, value.Oid); }
        }

        public double? Elapsed {
            get { return GetProperty<double?>(ElapsedProperty); }
            set { SetProperty(ElapsedProperty, value); }
        }

        public DateTime Date {
            get { return GetProperty<DateTime>(DateProperty); }
            set { SetProperty(DateProperty, value); }
        }

        public string Description {
            get { return GetProperty<string>(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        
        public ValueId[] ChangeSets {
            get { return GetMultiValueProperty(ChangeSetsProperty); }
            set { SetMultiValueProperty(ChangeSetsProperty, value); }
        }
    }
}