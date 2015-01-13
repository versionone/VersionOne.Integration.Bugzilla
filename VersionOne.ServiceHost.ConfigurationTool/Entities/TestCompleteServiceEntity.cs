using System;
using System.ComponentModel;
using System.Xml.Serialization;
using VersionOne.ServiceHost.ConfigurationTool.Attributes;
using VersionOne.ServiceHost.ConfigurationTool.Validation;

namespace VersionOne.ServiceHost.ConfigurationTool.Entities {
    /// <summary>
    /// TCReaderService configuration entity.
    /// </summary>
    [DependsOnService(typeof(TestWriterEntity))]
    [XmlRoot("TCReaderService")]
    public class TestCompleteEntity : BaseServiceEntity {
        private const int defaultRetryIntervalSeconds = 5;
        private const int minimumTimeoutIntervalMillis = 5000;
        private const int secondsToMillisRatio = 1000;

        [DefaultValue(defaultRetryIntervalSeconds * secondsToMillisRatio)]
        private long retryTimeoutMilliSeconds;

        #region Property list

        public const string ProjectSuiteConfigProperty = "ProjectSuiteConfig";
        public const string RetryTimeoutProperty = "RetryTimeoutSeconds";
        public const string RetryAttemptsProperty = "RetryAttempts";

        #endregion

        [NonEmptyStringValidator]
        [HelpString(HelpResourceKey = "TcProjectSuiteConfig")]
        public string ProjectSuiteConfig { get; set; }

        [XmlElement("RetryTimeout")]
        public long RetryTimeoutMilliSeconds {
            get { return retryTimeoutMilliSeconds; }
            set { retryTimeoutMilliSeconds = Math.Max(value, minimumTimeoutIntervalMillis); }
        }

        [XmlIgnore]
        [HelpString(HelpResourceKey = "TcRetryTimeoutSeconds")]
        public long RetryTimeoutSeconds {
            get { return RetryTimeoutMilliSeconds / secondsToMillisRatio; }
            set { RetryTimeoutMilliSeconds = secondsToMillisRatio * value; }
        }

        [HelpString(HelpResourceKey = "TcRetryAttempts")]
        public int RetryAttempts { get; set; }

        public TestCompleteEntity() {
            CreateTimer(TimerEntity.DefaultTimerIntervalMinutes);
        }

        public override bool Equals(object obj) {
            if(obj == null || obj.GetType() != typeof(TestCompleteEntity)) {
                return false;
            }

            var other = (TestCompleteEntity)obj;

            return string.Equals(ProjectSuiteConfig, other.ProjectSuiteConfig) &&
                int.Equals(RetryTimeoutSeconds, other.RetryTimeoutSeconds) &&
                    int.Equals(RetryAttempts, other.RetryAttempts);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}
