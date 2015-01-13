using VersionOne.ServiceHost.ConfigurationTool.BZ;
using VersionOne.ServiceHost.ConfigurationTool.Entities;
using VersionOne.ServiceHost.ConfigurationTool.UI.Interfaces;

namespace VersionOne.ServiceHost.ConfigurationTool.UI.Controllers {
    public class DefectsController : BasePageController<WorkitemWriterEntity, IWorkitemsPageView> {
        public DefectsController(WorkitemWriterEntity model, IFacade facade) : base(model, facade) { }

        public override void PrepareView() {
            base.PrepareView();

            try {
                View.ReferenceFieldList = Facade.GetDefectReferenceFieldList();
            } catch(BusinessException ex) {
                View.DisplayError(ex.Message);
            }
        }
    }
}