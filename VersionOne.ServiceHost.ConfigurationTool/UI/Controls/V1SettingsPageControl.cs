using System;
using System.Drawing;
using VersionOne.ServiceHost.ConfigurationTool.Entities;
using VersionOne.ServiceHost.ConfigurationTool.UI.Interfaces;

namespace VersionOne.ServiceHost.ConfigurationTool.UI.Controls {
    public partial class V1SettingsPageControl : BasePageControl<ServiceHostConfiguration>, IGeneralPageView {
        public event EventHandler<ConnectionValidationEventArgs> ValidationRequested;

        public V1SettingsPageControl() {
            InitializeComponent();

            btnVerifyV1Connection.Click += btnVerifyV1Connection_Click;
            chkUseProxy.CheckedChanged += chkUseProxy_CheckedChanged;

            AddValidationProvider(typeof(VersionOneSettings));
            AddValidationProvider(typeof(ProxyConnectionSettings));

            AddControlTextValidation<VersionOneSettings>(txtServerUrl, VersionOneSettings.ApplicationUrlProperty);
            AddControlTextValidation<VersionOneSettings>(txtAccessToken, VersionOneSettings.AccessTokenProperty);

            CheckProxyForm();
        }

        public override void DataBind() {
            AddControlBinding(txtServerUrl, Model.Settings, VersionOneSettings.ApplicationUrlProperty);
            AddControlBinding(txtAccessToken, Model.Settings, VersionOneSettings.AccessTokenProperty);
            AddControlBinding(chkUseProxy, Model.ProxySettings, ProxyConnectionSettings.EnabledProperty);
            AddControlBinding(txtProxyUri, Model.ProxySettings, ProxyConnectionSettings.UriProperty);
            AddControlBinding(txtProxyUsername, Model.ProxySettings, ProxyConnectionSettings.UsernameProperty);
            AddControlBinding(txtProxyPassword, Model.ProxySettings, ProxyConnectionSettings.PasswordProperty);
            AddControlBinding(txtProxyDomain, Model.ProxySettings, ProxyConnectionSettings.DomainProperty);

            BindHelpStrings();
        }

        private void BindHelpStrings() {
            AddHelpSupport(txtServerUrl, Model.Settings, VersionOneSettings.ApplicationUrlProperty);
            AddHelpSupport(chkUseProxy, Model.Settings.ProxySettings, ProxyConnectionSettings.EnabledProperty);
        }

        #region IGeneralPageView members
        public void SetValidationResult(bool validationSuccessful) {
            lblV1ConnectionValidationResult.Visible = true;

            if(validationSuccessful) {
                lblV1ConnectionValidationResult.ForeColor = Color.Green;
                lblV1ConnectionValidationResult.Text = Resources.V1SettingsValidMessage;
            } else {
                lblV1ConnectionValidationResult.ForeColor = Color.Red;
                lblV1ConnectionValidationResult.Text = Resources.V1SettingsInvalidMessage;
            }
        }

        public void SetProxyUrlValidationFault(bool validationSuccessful) {
            lblV1ConnectionValidationResult.Visible = !validationSuccessful;

            if(!validationSuccessful) {
                lblV1ConnectionValidationResult.ForeColor = Color.Red;
                lblV1ConnectionValidationResult.Text = Resources.V1ProxyUrlSettingsIsNotCorrect;
            }
        }
        #endregion

        #region Event handlers

        private void btnVerifyV1Connection_Click(object sender, EventArgs e) {
            if(ValidationRequested == null) {
                return;
            }

            var settings = new VersionOneSettings {
                ApplicationUrl = txtServerUrl.Text,
                AccessToken = txtAccessToken.Text,
                ProxySettings = new ProxyConnectionSettings {
                    Enabled = chkUseProxy.Checked,
                    Domain = txtProxyDomain.Text,
                    UserName = txtProxyUsername.Text,
                    Password = txtProxyPassword.Text,
                    Uri = txtProxyUri.Text
                }
            };
            ValidationRequested(this, new ConnectionValidationEventArgs(settings));
        }
        
        private void chkUseProxy_CheckedChanged(object sender, EventArgs e) {
            CheckProxyForm();
        }

        #endregion

        private void CheckProxyForm() {
            if(chkUseProxy.Checked) {
                AddControlTextValidation<ProxyConnectionSettings>(txtProxyUri, ProxyConnectionSettings.UriProperty);

                txtProxyUri.Enabled = true;
                txtProxyUsername.Enabled = true;
                txtProxyPassword.Enabled = true;
                txtProxyDomain.Enabled = true;
            } else {
                RemoveControlValidation<ProxyConnectionSettings>(txtProxyUri);

                txtProxyUri.Enabled = false;
                txtProxyUsername.Enabled = false;
                txtProxyPassword.Enabled = false;
                txtProxyDomain.Enabled = false;
            }
        }
    }
}