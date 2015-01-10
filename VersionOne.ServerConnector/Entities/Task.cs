using System.Collections.Generic;
using VersionOne.SDK.APIClient;

namespace VersionOne.ServerConnector.Entities {
    public class Task : Workitem {
        public override string TypeToken {
            get { return VersionOneProcessor.TaskType; }
        }

        internal Task(Asset asset, IDictionary<string, PropertyValues> listValues, IList<Member> owners, IEntityFieldTypeResolver typeResolver) 
            : base(asset, listValues, owners, typeResolver) { }
    }
}