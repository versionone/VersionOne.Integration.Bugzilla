using System;

namespace VersionOne.ServiceHost.ConfigurationTool.UI.Interfaces {
    public interface IPageView<TModel> where TModel : class {
        TModel Model { get; set; }
        void DataBind();

        void CommitPendingChanges();

        void DisplayError(string message);

        event EventHandler ControlValidationTriggered;
    }
}