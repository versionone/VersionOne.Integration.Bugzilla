using System;
using System.Threading;
using System.Windows.Forms;
using VersionOne.ServiceHost.ConfigurationTool.BZ;
using VersionOne.ServiceHost.ConfigurationTool.UI;
using VersionOne.ServiceHost.ConfigurationTool.UI.Interfaces;
using System.Reflection;
using log4net.Config;

namespace VersionOne.ServiceHost.ConfigurationTool {
    internal static class Program {
        private const string MutexName = "VersionOne.ServiceHost.ConfigTool";
        private static Mutex mutex;
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        internal static void Main() {
            try {
                if(!IsSingleInstance()) {
                    MessageBox.Show("You cannot run several instances of Settings Tool. The application will be terminated.", "Warning",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                ConfigureLogger();

                IConfigurationController controller = new ConfigurationFormController(Facade.Instance, UIFactory.Instance);
                var view = new ConfigurationForm();
                controller.RegisterView(view);
                controller.PrepareView();

                // TODO move try/catch with AssemblyLoadException here
                Application.Run(view);
            } finally {
                CloseMutex();
            }
        }

        private static bool IsSingleInstance() {
            mutex = new Mutex(true, MutexName);
            return mutex.WaitOne(0, false);
        }

        private static void CloseMutex() {
            mutex.Close();
        }

        private static void ConfigureLogger() {
            var assembly = Assembly.GetExecutingAssembly();
            var @namespace = typeof(Program).Namespace;

            using(var resourceStream = assembly.GetManifestResourceStream(@namespace + ".log4net.config")) {
                XmlConfigurator.Configure(resourceStream);
            }
        }
    }
}