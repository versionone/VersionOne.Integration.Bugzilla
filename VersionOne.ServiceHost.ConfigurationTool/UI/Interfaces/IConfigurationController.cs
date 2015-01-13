namespace VersionOne.ServiceHost.ConfigurationTool.UI.Interfaces {
    public interface IConfigurationController : ISettingsProvider {
        string ApplicationVersion { get; }

        void RegisterView(IConfigurationView configurationView);

        IConfigurationView View { get; }

        string CurrentFileName { get;}

        void PrepareView();
        void ShowPage(string pageKey);

        void SaveToFile(string fileName);
        void LoadFromFile(string fileName);

        bool ValidatePageAvailability(string pageKey);
    }
}