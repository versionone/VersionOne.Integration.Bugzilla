using VersionOne.SDK.APIClient;

namespace VersionOne.ServerConnector.Entities {
    public class Scope : Entity {
        internal Scope(Asset asset) : base(asset, null) {}
    }
}