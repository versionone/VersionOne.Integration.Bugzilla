using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VersionOne.Bugzilla.BugzillaAPI.Testss
{
    [TestClass()]
    public class given_a_Bug
    {
        private IBug _bug;
        private string _expectedReassignToPayload;

        [TestInitialize()]
        public void SetContext()
        {
            _bug = new Bug();

            _expectedReassignToPayload = "{\r\n  \"assigned_to\": \"denise@denise.com\",\r\n  \"status\": \"CONFIRMED\",\r\n  \"token\": \"terry.densmore@versionone.com\"\r\n}";
        }

        [TestMethod]
        public void it_should_give_appropriate_reassigneto_payloads()
        {
            var integrationUser = "terry.densmore@versionone.com";
            _bug.AssignedTo = "denise@denise.com";

            Assert.IsTrue(_expectedReassignToPayload == _bug.GetReassignBugPayload(integrationUser));
        }

    }
}

