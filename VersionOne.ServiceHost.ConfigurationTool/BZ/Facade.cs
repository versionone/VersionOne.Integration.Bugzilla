using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using VersionOne.ServerConnector.Entities;
using VersionOne.ServiceHost.ConfigurationTool.DL;
using VersionOne.ServiceHost.ConfigurationTool.Entities;
using VersionOne.ServiceHost.ConfigurationTool.Validation;
using log4net;
using System.Text.RegularExpressions;

namespace VersionOne.ServiceHost.ConfigurationTool.BZ {
    // TODO rename methods containing 'List' in name and not actually returning lists
    public class Facade : IFacade {
        public static readonly List<string> ConfigurationFileNames = new List<string>(new[] { "VersionOne.ServiceHost.exe.config", "VersionOne.ServiceExecutor.exe.config"});

        private static IFacade instance;
        private readonly XmlEntitySerializer serializer = new XmlEntitySerializer();

        private readonly ILog logger = LogManager.GetLogger(typeof(Facade));

        public static IFacade Instance {
            get { return instance ?? (instance = new Facade()); }
        }

        private Facade() { }
        
        /// <summary>
        /// Get VersionOne connection state
        /// </summary>
        public bool IsConnected {
            get { return V1Connector.Instance.IsConnected; }
        }

        /// <summary>
        /// Validate V1 connection settings.
        /// </summary>
        /// <param name="settings">settings for validation.</param>
        /// <returns>true, if validation succeeds.</returns>
        public bool IsVersionOneConnectionValid(VersionOneSettings settings) {
            //TODO remove this hack after changing V1Connector with ServerConnector
            return V1Connector.Instance.ValidateConnection(settings);
        }        


        /// <summary>
        /// Create empty Settings entity.
        /// </summary>
        public ServiceHostConfiguration CreateConfiguration() {
            return new ServiceHostConfiguration();
        }

        /// <summary>
        /// Get list of available values for Test Status.
        /// </summary>
        public IList<ListValue> GetTestStatuses() {
            try {
                var testStatuses = V1Connector.Instance.GetTestStatuses();
                return ConvertToListValues(testStatuses);
            } catch (Exception ex) {
                ProcessException("Failed to get test status list", ex);
                return null;
            }            
        }

        /// <summary>
        /// Get list of available values for Story Status.
        /// </summary>
        public IList<ListValue> GetStoryStatuses() {
            try {
                var storyStatuses = V1Connector.Instance.GetStoryStatuses();
                return ConvertToListValues(storyStatuses);
            } catch (Exception ex) {
                ProcessException("Failed to get story statuses", ex);
                return null;
            }
        }

        /// <summary>
        /// Get list of available values for Workitem Priority. 
        /// </summary>
        public IList<ListValue> GetVersionOnePriorities() {
            try {
                var priorities = V1Connector.Instance.GetWorkitemPriorities();
                return ConvertToListValues(priorities);
            } catch (Exception ex) {
                ProcessException("Failed to get test status list", ex);
                return null;
            }
        }

        private static IList<ListValue> ConvertToListValues(IDictionary<string, string> items) {
            var list = new List<ListValue>(items.Count);
            list.AddRange(items.Select(pair => new ListValue(pair.Key, pair.Value)));

            return list;
        }

        /// <summary>
        /// Get list of available values for Workitem Type. 
        /// </summary>
        public IList<ListValue> GetVersionOneWorkitemTypes() {
            try {
                var types = V1Connector.Instance.GetPrimaryWorkitemTypes();
                return types.Select(x => new ListValue(x, x)).ToList();
            } catch(Exception ex) {
                ProcessException("Failed to get primary workitem type list", ex);
                return null;
            }
        }

        /// <summary>
        /// Get list of available values for custom List Type by name
        /// </summary>
        public IList<ListValue> GetVersionOneCustomTypeValues(string typeName) {
            try {
                return V1Connector.Instance.GetValuesForType(typeName);
            } catch(Exception ex) {
                ProcessException("Failed to get custom list type values", ex);
                return null;
            }
        }

        /// <summary>
        /// Dump settings XML to file.
        /// </summary>
        /// <param name="settings">Configuration entity</param>
        /// <param name="fileName">Path to output file</param>
        // TODO decide on whether throwing exception when validation fails lets us get better code
        public ConfigurationValidationResult SaveConfigurationToFile(ServiceHostConfiguration settings, string fileName) {
            var validationResult = ValidateConfiguration(settings);
            
            if(!validationResult.IsValid) {
                return validationResult;
            }
            
            try {
                serializer.Serialize(settings.Services);
                serializer.SaveToFile(fileName);
            } catch(Exception ex) {
                ProcessException("Failed to flush data to file", ex);
            }

            return validationResult;
        }

        /// <summary>
        /// Load configuration from XML file.
        /// </summary>
        /// <param name="fileName">Path to input file</param>
        /// <returns>Settings entity or NULL if loading failed.</returns>
        public ServiceHostConfiguration LoadConfigurationFromFile(string fileName) {
            var shortFileName = Path.GetFileName(fileName);

            if (!ConfigurationFileNames.Contains(shortFileName)) {
                var filesNames = "'" + ConfigurationFileNames[0] + "' or '" + ConfigurationFileNames[1] + "'";
                throw new InvalidFilenameException("Invalid filename. Please use " + filesNames);
            }
            try {
                serializer.LoadFromFile(fileName);
                var entities = serializer.Deserialize();
                return new ServiceHostConfiguration(entities);
            } catch (Exception ex) {
                ProcessException("Failed to load data from file", ex);
            }
            return null;
        }

        /// <summary>
        /// Verify whether the file exists.
        /// </summary>
        /// <param name="fileName">The file to check</param>
        /// <returns>True if path contains the name of an existing file, otherwise, false</returns>
        // TODO refactor to encapsulate filename
        public bool FileExists(string fileName) {
            return File.Exists(fileName);
        }

        public bool AnyFileExists(IEnumerable<string> fileNames) {
            if (fileNames == null) {
                throw new ArgumentNullException("fileNames");
            }

            return fileNames.Any(FileExists);
        }

        /// <summary>
        /// Get collection of fields belonging to Test asset type that could be set up to store reference value.
        /// </summary>
        public string[] GetTestReferenceFieldList() {
            try {
                return V1Connector.Instance.GetReferenceFieldList(V1Connector.TestTypeToken).ToArray();
            } catch (Exception ex) {
                ProcessException("Failed to get reference field list for Tests", ex);
            }

            return null;
        }

        /// <summary>
        /// Get collection of sources from VersionOne server
        /// </summary>
        public string[] GetSourceList() {
            try {
                return V1Connector.Instance.GetSourceList().ToArray();
            } catch (Exception ex) {
                ProcessException("Failed to get source list", ex);
            }

            return null;
        }

        /// <summary>
        /// Get collection of fields belonging to Defect asset type that could be set up to store reference value.
        /// </summary>
        public string[] GetDefectReferenceFieldList() {
            try {
                return V1Connector.Instance.GetReferenceFieldList(V1Connector.DefectTypeToken).ToArray();
            } catch (Exception ex) {
                ProcessException("Failed to get reference field list for Defects", ex);
            }

            return null;
        }

        /// <summary>
        /// Gets collection of custom list fields depends on specified typeName
        /// </summary>
        /// <param name="typeName">Asset type name</param>
        /// <returns>list fields</returns>
        public IList<ListValue> GetVersionOneCustomListFields(string typeName) {
            try {
                var fields = V1Connector.Instance.GetCustomFields(typeName, FieldType.List);
                return fields.Select(x => new ListValue(ConvertFromCamelCase(x), x)).ToList();
            } catch(Exception ex) {
                ProcessException("Failed to get custom list fields", ex);
            }

            return null;
        }

        /// <summary>
        /// Gets collection of custom numeric fields depends on specified type
        /// </summary>
        /// <param name="typeName">Asset type name</param>
        /// <returns>numeric fields</returns>
        public IList<ListValue> GetVersionOneCustomNumericFields(string typeName) {
            try {
                var fields = V1Connector.Instance.GetCustomFields(typeName, FieldType.Numeric);
                return fields.Select(x => new ListValue(ConvertFromCamelCase(x), x)).ToList();
            } catch(Exception ex) {
                ProcessException("Failed to get custom list fields", ex);
            }

            return null;
        }

        public IList<ListValue> GetVersionOneCustomTextFields(string typeName) {
            try {
                var fields = V1Connector.Instance.GetCustomFields(typeName, FieldType.Text);
                return fields.Select(x => new ListValue(ConvertFromCamelCase(x), x)).ToList();
            } catch (Exception ex) {
                ProcessException("Failed to get custom list fields", ex);
            }

            return null;            
        }

        private static string ConvertFromCamelCase(string camelCasedString) {
            const string customPrefix = "Custom_";
            
            if (camelCasedString.StartsWith(customPrefix)) {
                camelCasedString = camelCasedString.Remove(0, customPrefix.Length);
            }
            
            return Regex.Replace(camelCasedString,
                @"(?<a>(?<!^)((?:[A-Z][a-z])|(?:(?<!^[A-Z]+)[A-Z0-9]+(?:(?=[A-Z][a-z])|$))|(?:[0-9]+)))", @" ${a}");
        }

        public string GetVersionOneCustomListTypeName(string fieldName, string containingTypeName) {
            try {
                return V1Connector.Instance.GetTypeByFieldName(fieldName, containingTypeName);
            } catch(Exception ex) {
                ProcessException("Failed to get custom list type for field", ex);
            }

            return null;
        }

        public IList<string> GetProjectList() {
            var projectList = new List<ProjectWrapper>();
            
            try {
                projectList = V1Connector.Instance.GetProjectList();
            } catch(Exception ex) {
                ProcessException("Failed to get project list", ex);
            }

            return projectList.Select(wrapper => wrapper.ProjectName).ToList();
        }

        public IList<ProjectWrapper> GetProjectWrapperList() {
            IList<ProjectWrapper> projectsList = new List<ProjectWrapper>();
            
            try {
                projectsList = V1Connector.Instance.GetProjectList();
            } catch(Exception ex) {
                ProcessException("Failed to get project list", ex);
            }
            
            return projectsList;
        }

        /// <summary>
        /// Validate connection to third party system.
        /// </summary>
        public bool ValidateConnection(BaseServiceEntity configuration) {
            try {
                var validator = ConnectionValidatorFactory.Instance.CreateValidator(configuration);
                return validator.Validate();
            } catch(FileNotFoundException ex) {
                throw new AssemblyLoadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Validate service configurations
        /// </summary>
        /// <param name="config">Settings to validate</param>
        /// <returns>Validation result</returns>
        public ConfigurationValidationResult ValidateConfiguration(ServiceHostConfiguration config) {
            var result = new ConfigurationValidationResult();
            var configResults = ValidateEntity(config);
            
            foreach (var configResult in configResults) {
                result.AddError(configResult.Message);
            }

            foreach (var entity in config.Services) {
                var results = ValidateEntity(entity);
                
                if(!results.IsValid) {
                    var messages = results.Select(x => x.Message).ToList();
                    result.AddEntity(entity, messages);
                }
            }

            return result;
        }

        public ValidationResults ValidateEntity<TEntity>(TEntity entity) {
            return ValidationFactory.CreateValidatorFromAttributes(entity.GetType(), string.Empty).Validate(entity);
        }

        [Obsolete]
        public void LogMessage(string message) {
            logger.Info(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message">Human-readable error message.</param>
        /// <param name="ex">Original exception to log and wrap/rethrow.</param>
        private void ProcessException(string message, Exception ex) {
            logger.Error(message, ex);
            throw new BusinessException(message, ex);
        }

        public void AddTestServiceMappingIfRequired(ServiceHostConfiguration config, TestPublishProjectMapping mapping)
        {
            //foreach (var entity in config.Services)
            //{
            //    if (!(entity is QCServiceEntity))
            //    {
            //        continue;
            //    }

            //    var qcEntity = (QCServiceEntity)entity;

            //    if (qcEntity.Projects.Any(qcProject => string.Equals(qcProject.Id, mapping.DestinationProject)))
            //    {
            //        return;
            //    }

            //    var newProject = new QCProject { Id = mapping.DestinationProject, VersionOneProject = mapping.Name };
            //    qcEntity.Projects.Add(newProject);
            //}
        }

        /// <summary>
        /// Reset V1 connection
        /// </summary>
        public void ResetConnection()
        {
            V1Connector.Instance.ResetConnection();
        }

    }
}