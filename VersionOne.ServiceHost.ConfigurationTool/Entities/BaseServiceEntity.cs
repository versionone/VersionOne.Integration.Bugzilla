using System.Xml.Serialization;

namespace VersionOne.ServiceHost.ConfigurationTool.Entities {
    /// <summary>
    /// Base class for all Service configuration entities.
    /// </summary>
    public abstract class BaseServiceEntity : BaseEntity {
        [XmlIgnore]
        public bool HasTimer {
            get { return Timer != null; }
        }

        [XmlIgnore]
        public TimerEntity Timer { get; set; }

        protected void CreateTimer(int timeoutMinutes) {
            var thisService = ServicesMap.GetByEntityType(GetType());

            Timer = new TimerEntity {
                PublishClass = thisService.PublishClass + ", " + thisService.Assembly,
                Disabled = false,
                TimeoutMinutes = timeoutMinutes
            };
        }
    }
}