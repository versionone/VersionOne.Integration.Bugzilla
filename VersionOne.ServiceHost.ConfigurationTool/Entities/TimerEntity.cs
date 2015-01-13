using System;
using System.ComponentModel;
using System.Xml.Serialization;
using VersionOne.ServiceHost.ConfigurationTool.Attributes;

namespace VersionOne.ServiceHost.ConfigurationTool.Entities {
    /// <summary>
    /// Entity representing Timer triggering Services operations in ServiceHost system.
    /// </summary>
    [XmlRoot("TimePublisherService")]
    public sealed class TimerEntity : BaseEntity {
        public const int DefaultTimerIntervalMinutes = 5;
        public const int MinimumTimerIntervalMinutes = 1;
        private const int minutesToMillisRatio = 60000;
        private const int minimumTimerIntervalMillis = minutesToMillisRatio * MinimumTimerIntervalMinutes;

        [DefaultValue(DefaultTimerIntervalMinutes * minutesToMillisRatio)]
        private long timeoutMilliseconds;

        public const string TimerProperty = "TimeoutMinutes";

        [XmlElement("Interval")]
        public long TimeoutMilliseconds {
            get { return timeoutMilliseconds; }
            set {
                timeoutMilliseconds = Math.Max(value, minimumTimerIntervalMillis);
            }
        }

        /// <summary>
        /// Timer interval in minutes; when elapsed, ServiceHost Timer publishes message (object) of type stored in PublishClass field.
        /// </summary>
        [XmlIgnore]
        [HelpString(HelpResourceKey = "CommonPollInterval")]
        public long TimeoutMinutes {
            get { return TimeoutMilliseconds / minutesToMillisRatio; }
            set { TimeoutMilliseconds = value * minutesToMillisRatio; }
        }

        /// <summary>
        /// Fully-qualified type of object to publish when elapsed.
        /// </summary>
        public string PublishClass { get; set; }

        public TimerEntity() {
            TimeoutMinutes = DefaultTimerIntervalMinutes;
        }
    }
}