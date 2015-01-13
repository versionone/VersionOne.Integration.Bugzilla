namespace VersionOne.ServiceHost.ConfigurationTool.UI.Interfaces {
    public interface IPageController {
        void RegisterView(object view);
        void RegisterFormController(IFormController formController);
        void UnregisterFormController(IFormController formController);
        void PrepareView();
    }
}