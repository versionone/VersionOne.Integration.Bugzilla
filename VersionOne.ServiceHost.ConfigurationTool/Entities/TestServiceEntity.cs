using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using VersionOne.ServiceHost.ConfigurationTool.Attributes;
using VersionOne.ServiceHost.ConfigurationTool.Validation;

namespace VersionOne.ServiceHost.ConfigurationTool.Entities {
    // NOTE Nobody is using TestReader service directly, so to decrease redundancy we inherit from TestWriter entity. It would also allow simpler XML serialization. 
    [XmlRoot("TestService")]
    [HasSelfValidation]
    public class TestServiceEntity : TestWriterEntity {
        public const string BaseQueryFilterProperty = "BaseQueryFilter";
        public const string ProjectsProperty = "Projects";

        [NonEmptyStringValidator]
        [HelpString(HelpResourceKey = "TestServicesBaseQueryFilter")]
        public string BaseQueryFilter { get; set; }

        [XmlArray("TestPublishProjectMap")]
        [XmlArrayItem("V1Project")]
        [HelpString(HelpResourceKey = "TestServicesV1Project")]
        public List<TestPublishProjectMapping> Projects { get; set; }

        public TestServiceEntity() {
            Projects = new List<TestPublishProjectMapping>();
            CreateTimer(TimerEntity.DefaultTimerIntervalMinutes);
        }

        [SelfValidation]
        public void CheckProjects(ValidationResults results) {
            var validator = ValidationFactory.CreateValidatorFromAttributes(typeof(TestPublishProjectMapping),
                string.Empty);

            foreach (TestPublishProjectMapping project in Projects) {
                var projectResults = validator.Validate(project);
                
                if(!projectResults.IsValid) {
                    results.AddAllResults(projectResults);
                }
            }
        }
    }
}