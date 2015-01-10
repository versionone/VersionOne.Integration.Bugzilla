namespace VersionOne.ServiceHost.WorkitemServices {
    public class Story : Workitem {
        public Story(string title, string description, string project, string owners): base(title, description, project, owners) {}

        public Story() {}

        public override string Type { get { return "Story"; } }
    }
}