using System.Text;
using VersionOne.SDK.APIClient;

namespace VersionOne.ServiceHost.ConfigurationTool.DL {
    public class ProjectWrapper {
        private readonly Asset asset;
        private readonly string projectName;
        private readonly int offset;

        public ProjectWrapper(Asset asset, string projectName, int offset) {
            this.asset = asset;
            this.projectName = projectName;
            this.offset = offset;
        }

        public string ProjectName {
            get { return projectName; }
        }

        public string Token {
            get { return asset.Oid.Momentless.Token; }
        }

        public string DisplayName {
            get { return new StringBuilder().Append(' ', offset * 2).Append(ProjectName).ToString(); }
        }

        public override string ToString() {
            return ProjectName;
        }
    }
}