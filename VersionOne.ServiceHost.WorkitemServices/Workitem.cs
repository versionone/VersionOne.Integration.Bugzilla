namespace VersionOne.ServiceHost.WorkitemServices {
//TODO combine with Workitem from ServerConnector
    public abstract class Workitem {
        protected Workitem(string title, string description, string project, string owners) {
            Title = title;
            Description = description;
            Project = project;
            Owners = owners;
        }

        protected Workitem() {}

        public string ExternalSystemName { get; set; }
        public string ExternalId { get; set; }

        ///<summary> Pemalink to the defect in an external system.</summary>
        public UrlToExternalSystem ExternalLink { get; set; }

        public string Number { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Project { get; set; }
        public string ProjectId { get; set; }

        ///<summary>A comma separated list of owner IDs.</summary>
        public string Owners { get; set; }

        public string Priority { get; set; }
        public abstract string Type { get; }

        public override string ToString() {
            return string.Format("[{0}/{1}] {2}", Number, ExternalId, Title);
        }
    }
}