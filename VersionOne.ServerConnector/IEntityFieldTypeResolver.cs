namespace VersionOne.ServerConnector {
    public interface IEntityFieldTypeResolver {
        void AddMapping(string entityType, string fieldName, string resolvedTypeName);
        string Resolve(string entityType, string fieldName);
        void Reset();
    }
}