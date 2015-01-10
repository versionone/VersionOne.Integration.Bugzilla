namespace VersionOne.ServiceHost.WorkitemServices {
    public class WorkitemStateChangeResult : WorkitemUpdateResult {
        public WorkitemStateChangeResult(string externalId, string workitemId) {
            ExternalId = externalId;
            WorkitemId = workitemId;
        }

        public string ExternalId { get; private set; }
        public bool ChangesProcessed { get; set; }

        public override string ToString() {
            return string.Format("{0}\n\tExternal ID: {1}", base.ToString(), ExternalId);
        }
    }
}