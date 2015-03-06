using VersionOne.ServiceHost.Core.Utility;
using VersionOne.ServiceHost.Core.Configuration;
using System.Collections.Generic;

namespace VersionOne.ServiceHost.BugzillaServices {
    public class BugzillaServiceConfiguration {
        [IgnoreConfigField]
        public readonly IDictionary<MappingInfo, MappingInfo> ProjectMappings = new Dictionary<MappingInfo, MappingInfo>();

        [IgnoreConfigField]
        public readonly IDictionary<MappingInfo, MappingInfo> PriorityMappings = new Dictionary<MappingInfo, MappingInfo>();

        [ConfigFileValue("BugzillaUserName", null)]
        public string UserName;

        [ConfigFileValue("BugzillaPassword", null)]
        public string Password;

        [ConfigFileValue("BugzillaIgnoreCertificate", "false", true)]
        public bool IgnoreCert;

        [ConfigFileValue("BugzillaUrl", null)]
        public string Url;

        [ConfigFileValue("BugzillaSearchName", null)]
        public string OpenIssueFilterId;

        [ConfigFileValue("CreateFieldId")]
        public string OnCreateFieldName;

        [ConfigFileValue("CreateFieldValue")]
        public string OnCreateFieldValue;

        [ConfigFileValue("CreateAccept", "false", true)]
        public bool OnCreateAccept;

        [ConfigFileValue("CreateResolveValue")]
        public string OnCreateResolveValue;

        [ConfigFileValue("CreateReassignValue")]
        public string OnCreateReassignValue;

        [ConfigFileValue("CloseFieldId")]
        public string OnStateChangeFieldName;

        [ConfigFileValue("CloseFieldValue")]
        public string OnStateChangeFieldValue;

        [ConfigFileValue("CloseAccept", "false", true)]
        public bool OnStateChangeAccept;

        [ConfigFileValue("CloseResolveValue")]
        public string OnStateChangeResolveValue;

        [ConfigFileValue("CloseReassignValue")]
        public string OnStateChangeReassignValue;

        [ConfigFileValue("DefectLinkFieldId")]
        public string DefectLinkFieldName;

        [ConfigFileValue("BugzillaBugUrlTemplate")]
        public string UrlTemplateToIssue;

        [ConfigFileValue("BugzillaBugUrlTitle")]
        public string UrlTitleToIssue;
    }
}