namespace VersionOne.ServiceHost.WorkitemServices {
    public class Defect : Workitem {
        public Defect(string title, string description, string project, string owners) : base(title, description, project, owners) {}

        public Defect() {}

        public override string Type { get { return "Defect"; } }
    }
}
