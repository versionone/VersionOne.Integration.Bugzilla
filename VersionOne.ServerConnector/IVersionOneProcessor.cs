using System;
using System.Collections.Generic;
using VersionOne.ServerConnector.Entities;
using VersionOne.ServerConnector.Filters;

namespace VersionOne.ServerConnector {
    public interface IVersionOneProcessor {
        bool ValidateConnection();

        Member GetLoggedInMember();
        ICollection<Member> GetMembers(IFilter filter);
        
        IList<FeatureGroup> GetFeatureGroups(IFilter filters, IFilter childrenFilters);
        
        void SaveEntities<T>(ICollection<T> entities) where T : BaseEntity;
        void Save(BaseEntity entity);
        void CloseWorkitem(PrimaryWorkitem workitem);
        void UpdateProject(string projectId, Link link);
        
        string GetWorkitemLink(Workitem workitem);
        string GetSummaryLink(Workitem workitem);
        
        ValueId CreateWorkitemStatus(string statusName);
        ValueId CreateWorkitemPriority(string priorityName);
        IList<ValueId> GetWorkitemStatuses();
        IList<ValueId> GetWorkitemPriorities();
        IList<ValueId> GetBuildRunStatuses();

        PropertyValues GetAvailableListValues(string typeToken, string fieldName);
        
        bool ProjectExists(string projectId);
        bool AttributeExists(string typeName, string attributeName);
        
        void AddProperty(string attr, string prefix, bool isList);
        void AddListProperty(string fieldName, string typeToken);
        void AddOptionalProperty(string attr, string prefix);
        void AddLinkToEntity(BaseEntity entity, Link link);

        IList<Workitem> GetWorkitems(string type, IFilter filter, SortBy sortBy = null);
        IList<PrimaryWorkitem> GetPrimaryWorkitems(IFilter filter, SortBy sortBy = null);
        IList<BuildProject> GetBuildProjects(IFilter filter);
        IList<ChangeSet> GetChangeSets(IFilter filter);

        Workitem CreateWorkitem(string assetType, string title, string description, string projectToken, 
                                string externalFieldName, string externalId, string externalSystemName, string priorityId, string owners);

        string GetProjectTokenByName(string projectName);
        string GetRootProjectToken();

        ICollection<Scope> LookupProjects(string term);
        Scope CreateProject(string name);
        IList<ListValue> GetCustomTextFields(string typeName);

        BuildRun CreateBuildRun(BuildProject buildProject, string name, DateTime date, double elapsed);
        ChangeSet CreateChangeSet(string name, string reference, string description);

        void LogConnectionConfiguration();
        void LogConnectionInformation();
    }
}