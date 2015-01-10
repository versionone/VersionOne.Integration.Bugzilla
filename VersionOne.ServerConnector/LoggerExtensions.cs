using VersionOne.ServiceHost.Core.Logging;

namespace VersionOne.ServerConnector {
    /// <summary>
    /// This class provides logger extension method so we could safely reuse <see cref="VersionOneProcessor"/> outside of ServiceHost without having to deploy VersionOne.ServiceHost.Core.
    /// </summary>
    public static class LoggerExtensions {
        public static void MaybeLog(this ILogger logger, LogMessage.SeverityType severity, string message) {
            if(logger != null) {
                logger.Log(severity, message);
            }
        }
    }
}