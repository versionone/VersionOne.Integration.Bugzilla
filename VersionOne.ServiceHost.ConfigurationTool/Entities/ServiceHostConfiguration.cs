using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using VersionOne.ServiceHost.ConfigurationTool.UI.Interfaces;

namespace VersionOne.ServiceHost.ConfigurationTool.Entities {
    /// <summary>
    /// Container entity for all services settings.
    /// </summary>
    [HasSelfValidation]
    public class ServiceHostConfiguration {
        private readonly List<BaseServiceEntity> services = new List<BaseServiceEntity>();

        private ProxyConnectionSettings proxySettings = new ProxyConnectionSettings();

        public VersionOneSettings Settings { get; private set; }

        public ProxyConnectionSettings ProxySettings {
            get { return proxySettings; }
            private set { proxySettings = value; }
        }

        public bool HasChanged { get; set; }

        public IEnumerable<BaseServiceEntity> Services {
            get { return services; }
        }

        public ServiceHostConfiguration() {
            HasChanged = true;
        }

        public ServiceHostConfiguration(IEnumerable<BaseServiceEntity> entities) : this() {
            foreach(var entity in entities) {
                AddService(entity);
            }
        }

        public BaseServiceEntity this[Type type] {
            get { return services.Find(entity => entity.GetType() == type); }
        }

        public void AddService(BaseServiceEntity entity) {
            if(entity is IVersionOneSettingsConsumer) {
                var settingsConsumer = (IVersionOneSettingsConsumer) entity;

                if(Settings != null) {
                    settingsConsumer.Settings = Settings;
                } else {
                    Settings = settingsConsumer.Settings;
                    ProxySettings = settingsConsumer.Settings.ProxySettings;
                }
            }
            
            services.Add(entity);
        }
    }
}