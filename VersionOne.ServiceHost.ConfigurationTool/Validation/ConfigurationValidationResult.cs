using System.Collections.Generic;
using VersionOne.ServiceHost.ConfigurationTool.Entities;

namespace VersionOne.ServiceHost.ConfigurationTool.Validation {
    public class ConfigurationValidationResult {
        private readonly IDictionary<BaseServiceEntity, IList<string>> invalidEntities = new Dictionary<BaseServiceEntity, IList<string>>();
        private readonly ICollection<string> generalErrors = new List<string>();

        public IDictionary<BaseServiceEntity, IList<string>> InvalidEntities {
            get { return invalidEntities; }
        }

        public IEnumerable<string> GeneralErrors {
            get { return generalErrors; }
        }

        public int InvalidEntitiesCount {
            get { return invalidEntities.Count; }
        }

        public int GeneralErrorCount {
            get { return generalErrors.Count; }
        }

        public bool IsValid {
            get {
                return invalidEntities.Count == 0 && generalErrors.Count == 0;
            }
        }

        public void AddEntity(BaseServiceEntity entity, IEnumerable<string> messages) {
            if(!invalidEntities.ContainsKey(entity)) {
                invalidEntities.Add(entity, new List<string>());
            }
            
            foreach(var message in messages) {
                invalidEntities[entity].Add(message);    
            }
        }

        public void AddError(string message) {
            generalErrors.Add(message);
        }
    }
}