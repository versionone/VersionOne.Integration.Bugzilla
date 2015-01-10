using VersionOne.SDK.APIClient;

namespace VersionOne.ServerConnector.Filters {
    public interface IFilter {
        GroupFilterTerm GetFilter(IAssetType type);
    }
}