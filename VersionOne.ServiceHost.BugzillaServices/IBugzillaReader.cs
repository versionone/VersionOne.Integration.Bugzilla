using System.Collections.Generic;
using VersionOne.ServiceHost.WorkitemServices;

namespace VersionOne.ServiceHost.BugzillaServices {
    internal interface IBugzillaReader {
        List<Defect> GetBugs();
    }
}