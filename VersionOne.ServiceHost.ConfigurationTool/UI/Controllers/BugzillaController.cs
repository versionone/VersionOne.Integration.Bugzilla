using System;
using VersionOne.ServiceHost.ConfigurationTool.Entities;
using VersionOne.ServiceHost.ConfigurationTool.UI.Interfaces;
using VersionOne.ServiceHost.ConfigurationTool.BZ;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace VersionOne.ServiceHost.ConfigurationTool.UI.Controllers {
    public class BugzillaController : BasePageController<BugzillaServiceEntity, IBugzillaPageView> {
        public BugzillaController(BugzillaServiceEntity model, IFacade facade) : base(model, facade) { }

        public override void PrepareView() {
            View.ValidationRequested += View_ValidationRequested;
            View.ControlValidationTriggered += View_ControlValidationTriggered;

            try {
                View.SourceList = Facade.GetSourceList();
                View.VersionOneProjects = Facade.GetProjectWrapperList();
                View.VersionOnePriorities = Facade.GetVersionOnePriorities();
                base.PrepareView();
            } catch (BusinessException ex) {
                View.DisplayError(ex.Message);
            } 
        }

        private void View_ControlValidationTriggered(object sender, EventArgs e) {
            ValidationResults results = Facade.ValidateEntity(Model);
            bool generalTabValid = true;
            bool mappingTabValid = true;

            foreach(ValidationResult result in results) {
                Type targetType = result.Target.GetType();

                if(targetType == typeof(BugzillaServiceEntity)) {
                    generalTabValid = false;
                }

                if(targetType == typeof(BugzillaProjectMapping) || targetType == typeof(BugzillaPriorityMapping)) {
                    mappingTabValid = false;
                }
            }

            View.SetGeneralTabValid(generalTabValid);
            View.SetMappingTabValid(mappingTabValid);
        }

        private void View_ValidationRequested(object sender, EventArgs e) {
            try {
                bool result = Facade.ValidateConnection(Model);
                View.SetValidationResult(result);
            } catch (AssemblyLoadException ex) {
                FormController.FailApplication(ex.Message);
            }
        }
    }
}
