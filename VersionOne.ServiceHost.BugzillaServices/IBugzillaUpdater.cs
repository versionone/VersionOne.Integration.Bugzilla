using VersionOne.ServiceHost.WorkitemServices;

namespace VersionOne.ServiceHost.BugzillaServices {
    internal interface IBugzillaUpdater {
        void OnDefectCreated(WorkitemCreationResult createdResult);
        bool OnDefectStateChange(WorkitemStateChangeResult stateChangeResult);
    }
}