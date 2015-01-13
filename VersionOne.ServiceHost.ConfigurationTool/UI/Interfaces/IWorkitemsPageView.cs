using VersionOne.ServiceHost.ConfigurationTool.Entities;

namespace VersionOne.ServiceHost.ConfigurationTool.UI.Interfaces {
    public interface IWorkitemsPageView : IPageView<WorkitemWriterEntity> {
        string[] ReferenceFieldList { get; set; }
    }
}