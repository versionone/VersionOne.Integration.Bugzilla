using VersionOne.ServiceHost.ConfigurationTool.BZ;
using VersionOne.ServiceHost.ConfigurationTool.Entities;
using VersionOne.ServiceHost.ConfigurationTool.UI.Interfaces;

namespace VersionOne.ServiceHost.ConfigurationTool.UI.Controllers {
    public class ChangesetsController : BasePageController<ChangesetWriterEntity, IChangesetsPageView> {
        public ChangesetsController(ChangesetWriterEntity model, IFacade facade) : base(model, facade) { }
    }
}