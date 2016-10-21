using System;
using VersionOne.ServiceHost.Core.Configuration;
using VersionOneSettings = VersionOne.ServiceHost.ConfigurationTool.Entities.VersionOneSettings;

namespace VersionOne.ServiceHost.ConfigurationTool.UI
{
    public class ConnectionValidationEventArgs : EventArgs
    {
        private AuthenticationTypes authenticationType;
        private string url;
        private string username;
        private string password;
        private bool integrated;
        private bool useProxy;
        private string proxyUri;
        private string proxyUsername;
        private string proxyPassword;
        private string proxyDomain;
        private readonly VersionOneSettings versionOneSettings = new VersionOneSettings();

        public AuthenticationTypes AuthenticationType
        {
            get { return authenticationType; }
        }

        public string Url
        {
            get { return url; }
        }

        public string Username
        {
            get { return username; }
        }

        public string Password
        {
            get { return password; }
        }

        public bool Integrated
        {
            get { return integrated; }
        }

        public bool UserProxy
        {
            get { return useProxy; }
        }

        public string ProxyUri
        {
            get { return proxyUri; }
        }

        public string ProxyUsername
        {
            get { return proxyUsername; }
        }

        public string ProxyPassword
        {
            get { return proxyPassword; }
        }

        public string ProxyDomain
        {
            get { return proxyDomain; }
        }

        public VersionOneSettings VersionOneSettings
        {
            get { return versionOneSettings; }
        }

        /// <summary>
        /// Validation without proxy.
        /// </summary>
        /// <param name="url">URL to VersionOne instance.</param>
        /// <param name="username">VersionOne username.</param>
        /// <param name="password">VersionOne password.</param>
        /// <param name="integrated">Use integrated authentication.</param>
        /// <param name="authenticationType">VersionOne authentication type</param>
        public ConnectionValidationEventArgs(string url, string username, string password, bool integrated, AuthenticationTypes authenticationType)
        {
            this.authenticationType = authenticationType;
            this.url = url;
            this.username = username;
            this.password = password;
            this.integrated = integrated;
            this.useProxy = false;
            this.versionOneSettings.AuthenticationType = authenticationType;
            this.versionOneSettings.ApplicationUrl = url;
            this.versionOneSettings.Username = username;
            this.versionOneSettings.Password = password;
            this.versionOneSettings.IntegratedAuth = integrated;
        }

        /// <summary>
        /// Validate with proxy settings.
        /// </summary>
        /// <param name="connectionSettings">Connection settings for VersionOne.</param>
        public ConnectionValidationEventArgs(VersionOneSettings connectionSettings)
        {
            this.authenticationType = connectionSettings.AuthenticationType;
            this.url = connectionSettings.ApplicationUrl;
            this.username = connectionSettings.Username;
            this.password = connectionSettings.Password;
            this.integrated = connectionSettings.IntegratedAuth;
            this.useProxy = connectionSettings.ProxySettings.Enabled;
            this.proxyDomain = connectionSettings.ProxySettings.Domain;
            this.proxyUri = connectionSettings.ProxySettings.Uri;
            this.proxyUsername = connectionSettings.ProxySettings.UserName;
            this.proxyPassword = connectionSettings.ProxySettings.Password;
            this.versionOneSettings = connectionSettings;
        }
    }
}
