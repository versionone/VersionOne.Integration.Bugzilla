using System;
using VersionOne.ServiceHost.ConfigurationTool.BZ;
using VersionOne.ServiceHost.ConfigurationTool.UI.Interfaces;

namespace VersionOne.ServiceHost.ConfigurationTool.UI.Controllers {
    public abstract class BasePageController<TModel, TIView> : IPageController
        where TModel : class
        where TIView : class, IPageView<TModel> {
        private IFormController formController;

        protected readonly IFacade Facade;

        public IFormController FormController {
            get { return formController; }
        }

        protected BasePageController(TModel model, IFacade facade) {
            Model = model;
            Facade = facade;
        }

        public TModel Model { get; set; }

        public TIView View { get; protected set; }

        public void RegisterView(object view) {
            if(!(view is TIView)) {
                throw new ArgumentException("Unexpected view type: " + view.GetType() + "; should be " + typeof(TIView));
            }

            RegisterView((TIView)view);
        }

        public void RegisterView(TIView view) {
            if(view == null) {
                throw new ArgumentNullException("view", "View should not be null.");
            }

            View = view;
            view.Model = Model;
        }

        public void RegisterFormController(IFormController formController) {
            this.formController = formController;
            formController.BeforeSave += OnBeforeSave;
        }

        public void UnregisterFormController(IFormController formController) {
            formController.BeforeSave -= OnBeforeSave;
        }

        public virtual void PrepareView() {
            View.DataBind();
        }

        private void OnBeforeSave(object sender, EventArgs e) {
            View.CommitPendingChanges();
        }
    }
}