using VersionOne.ServiceHost.ConfigurationTool.BZ;
using VersionOne.ServiceHost.ConfigurationTool.Entities;
using VersionOne.ServiceHost.ConfigurationTool.UI.Interfaces;
using System;

namespace VersionOne.ServiceHost.ConfigurationTool.UI.Controllers {
    public class GeneralController : BasePageController<ServiceHostConfiguration, IGeneralPageView> {
        public GeneralController(ServiceHostConfiguration model, IFacade facade) : base(model, facade) { }

        public override void PrepareView() {
            base.PrepareView();

            View.ValidationRequested += View_ValidationRequested;
        }

        private void View_ValidationRequested(object sender, ConnectionValidationEventArgs e) {
            var isProxyUrlSyntaxCorrect = IsProxyUriCorrect(e.VersionOneSettings.ProxySettings);            
            var result = false;
            
            if (isProxyUrlSyntaxCorrect) {
                result = Facade.IsVersionOneConnectionValid(e.VersionOneSettings);
                View.SetValidationResult(result);
            }            
            
            FormController.SetCoreServiceNodesEnabled(result);
        }

        private bool IsProxyUriCorrect(ProxyConnectionSettings proxySettings) {
            if (proxySettings == null || !proxySettings.Enabled) {
                return true;
            }
            
            var isProxyUrlSyntaxCorrect = true;
            
            try {
                var uri = new Uri(proxySettings.Uri);
            } catch (Exception) {
                isProxyUrlSyntaxCorrect = false;
                Facade.ResetConnection();
            }
            
            View.SetProxyUrlValidationFault(isProxyUrlSyntaxCorrect);
            return isProxyUrlSyntaxCorrect;
        }
    }
}