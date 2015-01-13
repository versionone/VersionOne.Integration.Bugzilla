using System.Collections.Generic;
using VersionOne.ServiceHost.ConfigurationTool.DL;
using VersionOne.ServiceHost.ConfigurationTool.Entities;
using VersionOne.ServiceHost.ConfigurationTool.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;

namespace VersionOne.ServiceHost.ConfigurationTool.BZ {

    // TODO shorten method signatures that contain 'List' and return lists
    public interface IFacade {
        ServiceHostConfiguration CreateConfiguration();
        ConfigurationValidationResult SaveConfigurationToFile(ServiceHostConfiguration settings, string fileName);
        ConfigurationValidationResult ValidateConfiguration(ServiceHostConfiguration settings);
        ValidationResults ValidateEntity<TEntity>(TEntity entity);
        ServiceHostConfiguration LoadConfigurationFromFile(string fileName);
        
        IList<ListValue> GetTestStatuses();
        IList<ListValue> GetStoryStatuses();
        IList<ListValue> GetVersionOnePriorities();
        IList<ListValue> GetVersionOneWorkitemTypes();
        IList<ListValue> GetVersionOneCustomTypeValues(string typeName);
        IList<ListValue> GetVersionOneCustomListFields(string typeName);
        IList<ListValue> GetVersionOneCustomNumericFields(string typeName);
        IList<ListValue> GetVersionOneCustomTextFields(string typeName);
        string GetVersionOneCustomListTypeName(string fieldName, string containingTypeName);

        string[] GetTestReferenceFieldList();
        string[] GetDefectReferenceFieldList();
        string[] GetSourceList();
        IList<string> GetProjectList();
        IList<ProjectWrapper> GetProjectWrapperList();
        
        bool ValidateConnection(BaseServiceEntity configuration);
        void ResetConnection();
        bool IsConnected { get; }
        bool IsVersionOneConnectionValid(VersionOneSettings settings);
        void LogMessage(string message);
        bool FileExists(string fileName);
        bool AnyFileExists(IEnumerable<string> fileName);
        void AddTestServiceMappingIfRequired(ServiceHostConfiguration config, TestPublishProjectMapping mapping);
    }
}