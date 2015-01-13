using System.Xml.Serialization;
using VersionOne.ServiceHost.ConfigurationTool.Attributes;
using VersionOne.ServiceHost.ConfigurationTool.UI.Interfaces;
using VersionOne.ServiceHost.ConfigurationTool.Validation;

namespace VersionOne.ServiceHost.ConfigurationTool.Entities {
    /// <summary>
    /// Core WorkitemWriterService configuration entity.
    /// </summary>
    [DependsOnVersionOne]
    [XmlRoot("WorkitemWriterHostedService")]
    public class WorkitemWriterEntity : BaseServiceEntity, IVersionOneSettingsConsumer {
        public const string ExternalIdFieldNameProperty = "ExternalIdFieldName";

        [NonEmptyStringValidator]
        [HelpString(HelpResourceKey = "WorkitemsReference")]
        public string ExternalIdFieldName { get; set; }

        public VersionOneSettings Settings { get; set; }

        public WorkitemWriterEntity() {
            Settings = new VersionOneSettings();
        }

        public override bool Equals(object obj) {
            if(obj == null || obj.GetType() != GetType()) {
                return false;
            }

            var other = (WorkitemWriterEntity) obj;

            return Disabled == other.Disabled && string.Equals(ExternalIdFieldName, other.ExternalIdFieldName);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}
