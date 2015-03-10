using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using VersionOne.ServiceHost.ConfigurationTool.Attributes;
using VersionOne.ServiceHost.ConfigurationTool.Validation;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace VersionOne.ServiceHost.ConfigurationTool.Entities {
    [HasSelfValidation]
    [DependsOnVersionOne]
    [DependsOnService(typeof(WorkitemWriterEntity))]
    [XmlRoot("BugzillaHostedService")]
    public class BugzillaServiceEntity : BaseServiceEntity {
        public const string UrlProperty = "Url";
        public const string UserNameProperty = "UserName";
        public const string PasswordProperty = "Password";
        public const string IgnoreCertificateProperty = "IgnoreCertificate";
        public const string SearchNameProperty = "SearchName";
        public const string UrlTemplateProperty = "UrlTemplate";
        public const string UrlTitleProperty = "UrlTitle";
        public const string SourceNameProperty = "SourceName";
        public const string LinkFieldProperty = "LinkField";
        public const string CreateFieldIdProperty = "CreateFieldId";
        public const string CreateFieldValueProperty = "CreateFieldValue";
        public const string CreateAcceptProperty = "CreateAccept";
        public const string CreateReassignValueProperty = "CreateReassignValue";
        public const string CreateResolveValueProperty = "CreateResolveValue";
        public const string CloseFieldIdProperty = "CloseFieldId";
        public const string CloseFieldValueProperty = "CloseFieldValue";
        public const string CloseAcceptProperty = "CloseAccept";
        public const string CloseReassignValueProperty = "CloseReassignValue";
        public const string CloseResolveValueProperty = "CloseResolveValue";
        public const string ProjectMappingsProperty = "ProjectMappings";
        public const string PriorityMappingsProperty = "PriorityMappings";

        public BugzillaServiceEntity () {
            CreateTimer(TimerEntity.DefaultTimerIntervalMinutes);
            CreateAccept = new NullableBool();
            CloseAccept = new NullableBool();

            AssignButtonPressed = true;

            ProjectMappings = new List<BugzillaProjectMapping>();
            PriorityMappings = new List<BugzillaPriorityMapping>();
        }

        [NonEmptyStringValidator]
        [RegexValidator(@"^[a-z]+:\/\/.+?\.cgi$", MessageTemplate = "URL must be valid and has to contain CGI file.")]
        [HelpString(HelpResourceKey="BugzillaUrl")]
        [XmlElement("BugzillaUrl")]
        public string Url { get; set; }

        [NonEmptyStringValidator]
        [XmlElement("BugzillaUserName")]
        public string UserName { get; set; }

        [NonEmptyStringValidator]
        [XmlElement("BugzillaPassword")]
        public string Password { get; set; }

        [HelpString(HelpResourceKey = "BugzillaIgnoreCertificate")]
        public NullableBool IgnoreCertificate { get; set; }

        [NonEmptyStringValidator]
        [HelpString(HelpResourceKey="BugzillaSearchName")]
        [XmlElement("BugzillaSearchName")]
        public string SearchName { get; set; }

        [NonEmptyStringValidator]
        [RegexValidator(@"^[a-z]+:\/\/.+?\#key\#$", MessageTemplate = "URL must be valid and has content #key# pattern at the end.")]
        [HelpString(HelpResourceKey="BugzillaUrlTemplate")]
        [XmlElement("BugzillaBugUrlTemplate")]
        public string UrlTemplate { get; set; }

        [NonEmptyStringValidator]
        [XmlElement("BugzillaBugUrlTitle")]
        public string UrlTitle { get; set; }

        [NonEmptyStringValidator]
        [HelpString(HelpResourceKey="BugzillaSource")]
        [XmlElement("SourceFieldValue")]
        public string SourceName { get; set; }

        [NonEmptyStringValidator]
        [HelpString(HelpResourceKey="BugzillaV1LinkFieldId")]
        [XmlElement("DefectLinkFieldId")]
        public string LinkField { get; set; }

        [HelpString(HelpResourceKey="BugzillaCreateFieldId")]
        public string CreateFieldId { get; set; }

        [HelpString(HelpResourceKey="BugzillaCreateFieldValue")]
        public string CreateFieldValue { get; set; }

        [HelpString(HelpResourceKey="BugzillaCreateAccept")]
        public NullableBool CreateAccept { get; set; }

        [HelpString(HelpResourceKey="BugzillaCreateReassignValue")]
        public string CreateReassignValue { get; set; }

        [HelpString(HelpResourceKey="BugzillaCreateResolve")]
        public string CreateResolveValue { get; set; }

        [HelpString(HelpResourceKey="BugzillaCloseFieldId")]
        public string CloseFieldId { get; set; }

        [HelpString(HelpResourceKey="BugzillaCloseFieldValue")]
        public string CloseFieldValue { get; set; }

        [HelpString(HelpResourceKey="BugzillaCloseAccept")]
        public NullableBool CloseAccept { get; set; }

        [HelpString(HelpResourceKey="BugzillaCloseReassignValue")]
        public string CloseReassignValue { get; set; }

        [HelpString(HelpResourceKey="BugzillaCloseResolveValue")]
        public string CloseResolveValue { get; set; }

        [HelpString(HelpResourceKey="BugzillaProjectMappings")]
        [XmlArray("ProjectMappings")]
        [XmlArrayItem("Mapping")]
        public List<BugzillaProjectMapping> ProjectMappings { get; set; }

        [HelpString(HelpResourceKey="BugzillaPriorityMappings")]
        [XmlArray("PriorityMappings")]
        [XmlArrayItem("Mapping")]
        public List<BugzillaPriorityMapping> PriorityMappings { get; set; }

        [XmlIgnore]
        public bool AssignButtonPressed { get; set; }

        #region Self Validation

        [SelfValidation]
        public void CheckProjectMappings(ValidationResults results) {
            Validator validator = ValidationFactory.CreateValidator<BugzillaProjectMapping>();

            foreach (var mapping in ProjectMappings) {
                var mappingValidationResults = validator.Validate(mapping);

                if(!mappingValidationResults.IsValid) {
                    results.AddAllResults(mappingValidationResults);
                }
            }
        }

        [SelfValidation]
        public void CheckPriorityMappings(ValidationResults results) {
            Validator validator = ValidationFactory.CreateValidator<BugzillaPriorityMapping>();

            foreach (var mapping in PriorityMappings) {
                var mappingValidationResults = validator.Validate(mapping);

                if(!mappingValidationResults.IsValid) {
                    results.AddAllResults(mappingValidationResults);
                }
            }
        }

        #endregion

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType()) {
                return false;
            }

            var other = (BugzillaServiceEntity)obj;
            return NullableBool.Equals(other.CloseAccept, CloseAccept) && string.Equals(other.CloseFieldId, CloseFieldId) &&
                string.Equals(other.CloseFieldValue, CloseFieldValue) && string.Equals(other.CloseReassignValue, CloseReassignValue) && 
                string.Equals(other.CloseResolveValue, CloseResolveValue) && NullableBool.Equals(other.CreateAccept, CreateAccept) && 
                string.Equals(other.CreateFieldId, CreateFieldId) && string.Equals(other.CreateFieldValue, CreateFieldValue) && 
                string.Equals(other.CreateReassignValue, CreateReassignValue) && string.Equals(other.CreateResolveValue, CreateResolveValue) && 
                string.Equals(other.LinkField, LinkField) && string.Equals(other.Password, Password) && 
                string.Equals(other.UserName, UserName) && string.Equals(other.SearchName, SearchName) && 
                string.Equals(other.SourceName, SourceName) && string.Equals(other.Url, Url) && 
                string.Equals(other.UrlTemplate, UrlTemplate) && string.Equals(other.UrlTitle, UrlTitle) &&
                NullableBool.Equals(other.IgnoreCertificate, IgnoreCertificate);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}