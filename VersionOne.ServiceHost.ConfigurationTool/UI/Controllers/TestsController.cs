using VersionOne.ServiceHost.ConfigurationTool.BZ;
using VersionOne.ServiceHost.ConfigurationTool.Entities;
using VersionOne.ServiceHost.ConfigurationTool.UI.Interfaces;

namespace VersionOne.ServiceHost.ConfigurationTool.UI.Controllers {
    public class TestsController : BasePageController<TestWriterEntity, ITestsPageView> {
        public TestsController(TestWriterEntity model, IFacade facade) : base(model, facade) { }

        public override void PrepareView() {
            View.CreateDefectChoiceList = new string[] { "All", "CurrentIteration", "None" };

            try {
                View.FailedTestStatusList = Facade.GetTestStatuses();
                View.PassedTestStatusList = Facade.GetTestStatuses();
                View.ReferenceFieldList = Facade.GetTestReferenceFieldList();
                base.PrepareView();
            } catch(BusinessException ex) {
                View.DisplayError(ex.Message);
            }
        }
    }
}