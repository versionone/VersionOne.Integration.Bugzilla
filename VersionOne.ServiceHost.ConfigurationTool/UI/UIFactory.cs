using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VersionOne.ServiceHost.ConfigurationTool.BZ;
using VersionOne.ServiceHost.ConfigurationTool.Entities;
using VersionOne.ServiceHost.ConfigurationTool.UI.Controllers;
using VersionOne.ServiceHost.ConfigurationTool.UI.Controls;
using VersionOne.ServiceHost.ConfigurationTool.UI.Interfaces;

namespace VersionOne.ServiceHost.ConfigurationTool.UI {
    public class UIFactory : IUIFactory {
        private static UIFactory instance;

        private IPageController currentController;

        private readonly IDictionary<string, StateDefinition> mappings = new Dictionary<string, StateDefinition>();

        private UIFactory() {
            mappings.Add(
                "General",
                new StateDefinition(typeof(GeneralController),
                    typeof(V1SettingsPageControl),
                    typeof(ServiceHostConfiguration),
                    ServicesGroup.General)
                );
            mappings.Add(
                "Tests",
                new StateDefinition(typeof(TestsController),
                    typeof(TestsPageControl),
                    typeof(TestWriterEntity),
                    ServicesGroup.Core)
                );
            mappings.Add(
                "Workitems",
                new StateDefinition(typeof(DefectsController),
                    typeof(WorkitemsPageControl),
                    typeof(WorkitemWriterEntity),
                    ServicesGroup.Core)
                );
            mappings.Add(
                "Changesets",
                new StateDefinition(typeof(ChangesetsController),
                    typeof(ChangesetsPageControl),
                    typeof(ChangesetWriterEntity),
                    ServicesGroup.Core)
                );
            mappings.Add(
                "Bugzilla",
                new StateDefinition(typeof(BugzillaController),
                    typeof(BugzillaPageControl),
                    typeof(BugzillaServiceEntity),
                    ServicesGroup.Custom)
                );
            mappings.Add(
                "Test service",
                new StateDefinition(typeof(TestServiceController),
                    typeof(TestServicePageControl),
                    typeof(TestServiceEntity),
                    ServicesGroup.Core)
                );
        }

        private StateDefinition RequestStateDef(string key) {
            if(!mappings.ContainsKey(key)) {
                throw new NotSupportedException("This state is not supported by application.");
            }

            return mappings[key];
        }

        public object ResolveModel(string pageKey, ServiceHostConfiguration parentModel) {
            var state = mappings[pageKey];

            if(pageKey == "General") {
                return parentModel;
            }

            return parentModel[state.EntityType];
        }

        public static UIFactory Instance {
            get { return instance ?? (instance = new UIFactory()); }
        }

        public TController CreateController<TController, TModel>(string key, TModel model) where TController : class, IPageController where TModel : class {
            var stateDefinition = RequestStateDef(key);
            var controller = Activator.CreateInstance(stateDefinition.ControllerType, new object[] {model, Facade.Instance}) as TController;
            currentController = controller;
            return controller;
        }

        public TView CreateView<TView>(string key) where TView : class {
            var stateDefinition = RequestStateDef(key);
            var view = Activator.CreateInstance(stateDefinition.ViewType) as TView;
            return view;
        }

        /// <summary>
        /// Create UI page, usually to configure a single service
        /// </summary>
        /// <param name="pageKey">Page key, the same as in <see cref="mappings"/></param>
        /// <param name="model">Settings container</param>
        /// <param name="formController">Reference to form controller</param>
        /// <returns>Representation of a configuration page</returns>
        public Control GetNextPage(string pageKey, ServiceHostConfiguration model, IFormController formController) {
            if(currentController != null) {
                currentController.UnregisterFormController(formController);
            }

            var stateDef = RequestStateDef(pageKey);
            var modelResolved = ResolveModel(pageKey, model);

            var view = Activator.CreateInstance(stateDef.ViewType);
            var controller = (IPageController)Activator.CreateInstance(stateDef.ControllerType, new[] { modelResolved, Facade.Instance });

            currentController = controller;

            controller.RegisterView(view);
            controller.RegisterFormController(formController);
            controller.PrepareView();

            return (Control)view;
        }

        /// <summary>
        /// Enumerate core service nodes to bind in UI tree
        /// </summary>
        /// <param name="config">Settings container</param>
        public IEnumerable<string> GetCoreServiceNames(ServiceHostConfiguration config) {
            IList<string> names = new List<string>();

            foreach(var mapping in mappings) {
                var entity = config[mapping.Value.EntityType];
                
                if(entity != null && mapping.Value.Group == ServicesGroup.Core) {
                    names.Add(mapping.Key);
                }
            }

            return names;
        }

        /// <summary>
        /// Enumerate custom service nodes
        /// </summary>
        /// <param name="config">Settings container</param>
        public IEnumerable<string> GetCustomServiceNames(ServiceHostConfiguration config) {
            IList<string> names = new List<string>();

            foreach(var mapping in mappings) {
                var entity = config[mapping.Value.EntityType];
                
                if(entity != null && mapping.Value.Group == ServicesGroup.Custom) {
                    names.Add(mapping.Key);
                }
            }

            return names;
        }

        /// <summary>
        /// Resolve page name mapped for entity type
        /// </summary>
        /// <param name="entity">Entity to resolve page name</param>
        /// <returns>Page name, or null, if there is no mapping</returns>
        public string ResolvePageNameByEntity(BaseServiceEntity entity) {
            return mappings.Where(mapping => mapping.Value.EntityType.Equals(entity.GetType())).Select(item => item.Key).FirstOrDefault();
        }
    }
}