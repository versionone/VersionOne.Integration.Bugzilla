using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using VersionOne.ServiceHost.ConfigurationTool.Attributes;
using VersionOne.ServiceHost.ConfigurationTool.UI.Interfaces;
using VersionOne.ServiceHost.ConfigurationTool.Validation;

namespace VersionOne.ServiceHost.ConfigurationTool.Entities {
    /// <summary>
    /// Core ChangesetWriterService configuration entity.
    /// </summary>
    [HasSelfValidation]
    [DependsOnVersionOne]
    [XmlRoot("ChangeSetWriterService")]
    public class ChangesetWriterEntity : BaseServiceEntity, IVersionOneSettingsConsumer {
        public const string ReferenceAttributeProperty = "ReferenceAttribute";
        public const string ChangeCommentProperty = "ChangeComment";
        public const string AlwaysCreateProperty = "AlwaysCreate";

        // NOTE value hardcoded for the moment as nobody ever requested this setting
        public string ReferenceAttribute {
            get { return "Number"; }
            set { }
        }

        [NonEmptyStringValidator]
        [HelpString(HelpResourceKey = "ChangesetsComment")]
        public string ChangeComment { get; set; }

        [HelpString(HelpResourceKey = "ChangesetsAllwaysCreate")]
        public bool AlwaysCreate { get; set; }

        public VersionOneSettings Settings { get; set; }

        public ChangesetLink Link { get; set; }

        public ChangesetWriterEntity() {
            Settings = new VersionOneSettings();
            Link = new ChangesetLink();
        }

        [SelfValidation]
        public void CheckLink(ValidationResults results) {
            var validator = ValidationFactory.CreateValidator<ChangesetLink>();
            var linkResults = validator.Validate(Link);

            if(!linkResults.IsValid) {
                results.AddAllResults(linkResults);
            }
        }

        public override bool Equals(object obj) {
            if(obj == null || GetType() != obj.GetType()) {
                return false;
            }

            var other = (ChangesetWriterEntity)obj;

            return AlwaysCreate == other.AlwaysCreate && Disabled == other.Disabled &&
                string.Equals(ChangeComment, other.ChangeComment) && ChangesetLink.Equals(Link, other.Link);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}