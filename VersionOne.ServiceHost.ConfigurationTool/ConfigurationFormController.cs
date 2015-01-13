using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using VersionOne.ServiceHost.ConfigurationTool.BZ;
using VersionOne.ServiceHost.ConfigurationTool.Entities;
using VersionOne.ServiceHost.ConfigurationTool.UI;
using VersionOne.ServiceHost.ConfigurationTool.UI.Interfaces;

namespace VersionOne.ServiceHost.ConfigurationTool {
    public class ConfigurationFormController : IConfigurationController, IFormController {
        private IConfigurationView view;
        private ServiceHostConfiguration settings;
        private string currentFileNameValue = string.Empty;
        
        private readonly IFacade facadeImpl;
        private readonly IUIFactory uiFactory;

        #region IConfigurationController Members

        public IConfigurationView View {
            get { return view; }
        }

        public ServiceHostConfiguration Settings {
            get { return settings; }
        }

        public string ApplicationVersion {
            get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
        }

        public string CurrentFileName {
            get { return currentFileNameValue; }
        }

        public ConfigurationFormController(IFacade facade, IUIFactory uiFactory) {
            facadeImpl = facade;
            this.uiFactory = uiFactory;
            settings = facade.CreateConfiguration();
        }

        public void ShowPage(string pageKey) {
            try {
                var newView = uiFactory.GetNextPage(pageKey, Settings, this);
                View.HeaderText = pageKey;
                View.CurrentControl = newView;
            } catch(DependencyFailureException ex) {
                view.ShowErrorMessage(ex.Message);
            } catch(V1ConnectionRequiredException) {
                view.ShowErrorMessage("To open the page, valid V1 connection is required");
            }
        }

        public void RegisterView (IConfigurationView configurationView) {
            if (configurationView == null) {
                throw new InvalidOperationException("View must be initialized");
            }

            view = configurationView;
            configurationView.Controller = this;
        }

        public void PrepareView() {
            view.GenerateSnapshotMenuItemEnabled = false;
            view.NewFileMenuItemEnabled = false;
            view.OpenFileMenuItemEnabled = true;
            view.OptionsMenuItemEnabled = false;
            view.SaveFileAsMenuItemEnabled = true;
            view.SaveFileMenuItemEnabled = true;

            if (facadeImpl.AnyFileExists(Facade.ConfigurationFileNames)) {
                LoadFromAnyDefaultFile();
            } else {
                view.SetServiceNodesAndRedraw(null, null);
                view.SetCoreServiceNodesEnabled(false);
            }
        }

        private void LoadFromAnyDefaultFile() {
            foreach (var fileName in Facade.ConfigurationFileNames) {
                if (facadeImpl.FileExists(fileName)) {
                    LoadFromFile(fileName);
                    return;
                }
            }
        }

        public void SaveToFile(string fileName) {
            if (string.IsNullOrEmpty(fileName)) {
                fileName = Facade.ConfigurationFileNames[0];
            }

            InvokeBeforeSave();

            var result = facadeImpl.SaveConfigurationToFile(Settings, fileName);

            if(result.IsValid) {
                return;
            }

            var sb = new StringBuilder("Cannot save settings to file. The following errors were encountered: ");

            foreach (var error in result.GeneralErrors) {
                AppendErrorString(sb, error);
            }

            foreach (var entity in result.InvalidEntities.Keys) {
                AppendErrorString(sb, GetInvalidPageErrorMessage(entity, result.InvalidEntities[entity]));
            }

            view.ShowErrorMessage(sb.ToString());
        }

        private static void AppendErrorString(StringBuilder sb, string message) {
            sb.AppendFormat("{0} {1}", Environment.NewLine, message);
        }

        private string GetInvalidPageErrorMessage(BaseServiceEntity entity, IEnumerable<string> messages) {
            var pageName = uiFactory.ResolvePageNameByEntity(entity);

            return string.Format("{0} page contains invalid data:{1}{2}{3}", pageName, Environment.NewLine, "-", string.Join(Environment.NewLine + "-", messages.ToArray()));
        }

        public void LoadFromFile(string fileName) {
            try {
                settings = facadeImpl.LoadConfigurationFromFile(fileName);

                new DependencyValidator(facadeImpl).CheckServiceDependencies(settings);
                
                facadeImpl.ResetConnection();
                ShowPage("General");
                view.SetServiceNodesAndRedraw(uiFactory.GetCoreServiceNames(settings), uiFactory.GetCustomServiceNames(settings));
                view.SetCoreServiceNodesEnabled(false);
                currentFileNameValue = fileName;
            } catch(InvalidFilenameException ex) {
                view.ShowErrorMessage(ex.Message);
            } catch(DependencyFailureException) {
                settings = facadeImpl.CreateConfiguration();
                view.SetServiceNodesAndRedraw(null, null);
                view.ShowErrorMessage(Resources.ServiceDependenciesInFileInvalid);
            }
        }
        
        public bool ValidatePageAvailability(string pageKey) {
            var model = uiFactory.ResolveModel(pageKey, settings);

            try {
                if (model is BaseServiceEntity) {
                    new DependencyValidator(facadeImpl).CheckVersionOneDependency((BaseServiceEntity) model);
                }
            } catch(V1ConnectionRequiredException) {
                View.ShowErrorMessage(Resources.V1ConnectionRequiredForPage);
                return false;
            }

            return true;
        }

        #endregion

        #region IFormController Members

        public void SetCoreServiceNodesEnabled(bool enabled) {
            View.SetCoreServiceNodesEnabled(enabled);
        }

        public void FailApplication(string message) {
            view.ShowErrorMessage("Could not load application dependency. Application will be closed.");
            facadeImpl.LogMessage(message);
            Environment.Exit(-1);
        }

        public event EventHandler BeforeSave;

        private void InvokeBeforeSave() {
            if(BeforeSave != null) {
                BeforeSave(this, EventArgs.Empty);
            }
        }

        #endregion
    }
}