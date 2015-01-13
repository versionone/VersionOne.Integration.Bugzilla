using System.Collections.Generic;
using System.Windows.Forms;
using VersionOne.ServiceHost.ConfigurationTool.Entities;
using VersionOne.ServiceHost.ConfigurationTool.UI.Interfaces;

namespace VersionOne.ServiceHost.ConfigurationTool.UI {
    public interface IUIFactory {
        TController CreateController<TController, TModel>(string key, TModel model) where TController : class, IPageController where TModel : class;
        TView CreateView<TView>(string key) where TView : class;
        object ResolveModel(string pageKey, ServiceHostConfiguration settings);
        Control GetNextPage(string pageKey, ServiceHostConfiguration model, IFormController formController);
        IEnumerable<string> GetCoreServiceNames(ServiceHostConfiguration config);
        IEnumerable<string> GetCustomServiceNames(ServiceHostConfiguration config);
        string ResolvePageNameByEntity(BaseServiceEntity entity);
    }
}