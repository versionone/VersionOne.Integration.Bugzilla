using System.Collections.Generic;
using System.Text;

namespace VersionOne.ServiceHost.WorkitemServices {
    public class WorkitemUpdateResult {
        protected WorkitemUpdateResult() {
            Warnings = new List<string>();
            Messages = new List<string>();
        }

        public List<string> Warnings { get; private set; }
        public List<string> Messages { get; private set; }
        public string WorkitemId { get; set; }

        public override string ToString() {
            var warningBuffer = new StringBuilder();

            foreach(var warningValue in Warnings) {
                warningBuffer.AppendLine(warningValue);
            }
            
            var messageBuffer = new StringBuilder();

            foreach(var messageValue in Messages) {
                messageBuffer.AppendLine(messageValue);
            }
            
            return string.Format("{0}\n\tWorkitem Id: {1}\n\tWarnings: {2}\n\tMessages: {3}", base.ToString(), WorkitemId, warningBuffer, messageBuffer);
        }
    }
}