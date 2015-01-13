using System;
using VersionOne.ServiceHost.ConfigurationTool.Entities;

namespace VersionOne.ServiceHost.ConfigurationTool.BZ {
    public class TestProjectEventArgs : EventArgs {
        public readonly TestPublishProjectMapping Mapping;

        public TestProjectEventArgs(TestPublishProjectMapping mapping) {
            Mapping = mapping;
        }
    }
}
