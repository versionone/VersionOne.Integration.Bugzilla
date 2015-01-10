using System.Collections.Generic;
using System.Diagnostics;
using VersionOne.SDK.APIClient;

namespace VersionOne.ServerConnector.Entities {
    [DebuggerDisplay("{TypeName} {Name}, Id={Id}, Number={Number}")]
    public class Story : PrimaryWorkitem {
        public const string BenefitsProperty = "Benefits";

        public override string TypeToken {
            get { return VersionOneProcessor.StoryType; }
        }

        protected internal Story(Asset asset, IDictionary<string, PropertyValues> listValues, IEntityFieldTypeResolver typeResolver, IList<Member> owners = null) : base(asset, listValues, typeResolver, owners) { }

        protected Story() { }

        public string Benefits {
            get { return GetProperty<string>(BenefitsProperty); }
            set { SetProperty(BenefitsProperty, value); }
        }
    }
}