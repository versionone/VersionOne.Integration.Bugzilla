using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Threading;
using Ninject;
using VersionOne.Profile;
using VersionOne.SDK.APIClient;
using VersionOne.ServiceHost.Core;
using VersionOne.ServiceHost.Core.Configuration;
using VersionOne.ServiceHost.Core.Eventing;
using VersionOne.ServiceHost.Core.Logging;
using VersionOne.ServiceHost.Core.Services;
using VersionOne.ServiceHost.Eventing;

namespace VersionOne.ServiceHost {
    public sealed class CommonMode {
        public class FlushProfile { }

        private IProfileStore profileStore;
        private IList<ServiceInfo> services;
        private readonly IKernel container;

        public IEventManager EventManager { get; private set; }
        public ILogger Logger { get; private set; }

        public CommonMode(IKernel container) {
            this.container = container;
        }

        public void Initialize() {
            EventManager = new EventManager();
            Logger = new Logger(EventManager);
            services = (IList<ServiceInfo>) ConfigurationManager.GetSection("Services");
            profileStore = new XmlProfileStore("profile.xml");
        }

        private void LogDiagnosticInformation() {
            try {
                Logger.Log(LogMessage.SeverityType.Info, "Diagnostic information:");

                var installerInfo = (InstallerConfiguration) ConfigurationManager.GetSection("Installer");
                Logger.Log(LogMessage.SeverityType.Info,
                           string.Format("    Installer long name: '{0}', short name: '{1}'", installerInfo.LongName, installerInfo.ShortName));

                var version = Assembly.GetEntryAssembly().GetName().Version;
                Logger.Log(LogMessage.SeverityType.Info, string.Format("    ServiceHost version is {0}", version));

                var sdkVersion = typeof (V1Connector).Assembly.GetName().Version;
                Logger.Log(LogMessage.SeverityType.Info, string.Format("    VersionOne SDK version is {0}", sdkVersion));

                Logger.Log(LogMessage.SeverityType.Info, "End of diagnostic section.");
            } catch(Exception ex) {
                Logger.Log(LogMessage.SeverityType.Error, 
                    "Failed to log diagnostic information. This could happen if configuration is invalid or integration package is incomplete, for example, some of required DLL files are missing.", ex);
            }
        }

        public void Startup() {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            foreach(var ss in services) {
                Logger.Log(string.Format("Initializing {0}", ss.Name));
                ss.Service.Initialize(ss.Config, EventManager, profileStore[ss.Name]);

                if(ss.Service is IComponentProvider) {
                    ((IComponentProvider) ss.Service).RegisterComponents(container);
                }

                ss.Service.Start();
                Logger.Log(string.Format("Initialized {0}", ss.Name));
            }

            EventManager.Publish(ServiceHostState.Validate);

            LogDiagnosticInformation();

            EventManager.Publish(ServiceHostState.Startup);
            EventManager.Subscribe(typeof(FlushProfile), FlushProfileImpl);
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e) {
            Logger.Log("Service Host Caught Unhandled Exception", (Exception)e.ExceptionObject);
        }

        public void Shutdown() {
            EventManager.Publish(ServiceHostState.Shutdown);
            Thread.Sleep(5 * 1000);
            profileStore.Flush();
        }

        private void FlushProfileImpl(object o) {
            profileStore.Flush();
        }
    }
}