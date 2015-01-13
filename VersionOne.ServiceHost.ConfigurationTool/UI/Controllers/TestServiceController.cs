using VersionOne.ServiceHost.ConfigurationTool.BZ;
using VersionOne.ServiceHost.ConfigurationTool.Entities;
using VersionOne.ServiceHost.ConfigurationTool.UI.Interfaces;

namespace VersionOne.ServiceHost.ConfigurationTool.UI.Controllers {
    public class TestServiceController : BasePageController<TestServiceEntity, ITestServicePageView> {
        public TestServiceController(TestServiceEntity model, IFacade facade) : base(model, facade) { }

        public override void PrepareView() {
            View.CreateDefectChoiceList = new string[] { "All", "CurrentIteration", "None" };
            View.ProjectMapRowsChanged += View_ProjectMapRowsChanged; 

            try {
                View.FailedTestStatusList = Facade.GetTestStatuses();
                View.PassedTestStatusList = Facade.GetTestStatuses();
                View.ReferenceFieldList = Facade.GetTestReferenceFieldList();
                View.Projects = Facade.GetProjectList();
                base.PrepareView();
            } catch(BusinessException ex) {
                View.DisplayError(ex.Message);
            }
        }

        private void View_ProjectMapRowsChanged(object sender, TestProjectEventArgs e) {
            Facade.AddTestServiceMappingIfRequired(FormController.Settings, e.Mapping);
        }
    }
}