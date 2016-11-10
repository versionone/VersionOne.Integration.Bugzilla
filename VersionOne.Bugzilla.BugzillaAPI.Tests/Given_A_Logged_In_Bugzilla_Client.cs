using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using VersionOne.ServiceHost.Core.Logging;
using Rhino.Mocks;

namespace VersionOne.Bugzilla.BugzillaAPI.Tests
{
    [TestClass()]
    public class Given_A_Logged_In_Bugzilla_Client
    {

        private IBugzillaClient _client;
        private string _loggedInUserToken;

        [TestInitialize()]
        public void SetContext()
        {
            var liveInstanceOfBugzillaThatHasRestfulApi = "http://184.170.225.111/bugzilla/rest/";

            IBugzillaClientConfiguration bugzillaClientConfiguration = new BugzillaClientConfiguration
            {
                UserName = "terry.densmore@versionone.com",
                Password = "admin1425",
                Url = liveInstanceOfBugzillaThatHasRestfulApi
            };

            ILogger logger = MockRepository.GenerateMock<ILogger>();

            _client = new BugzillaClient(bugzillaClientConfiguration, logger);

            _loggedInUserToken = _client.Login();
        }

        [TestMethod]
        public void when_logged_out_user_and_associated_token_is_not_valid()
        {
            _client.Logout();
            Assert.IsFalse(_client.IsCurrentLoginCredentialsValid());
        }

        [TestMethod]
        public void when_logged_in_user_and_associated_token_is_valid()
        {
            Assert.IsTrue(_client.IsCurrentLoginCredentialsValid());
        }

        [TestMethod]
        public void when_calling_login_it_should_respond_with_a_token()
        {
            Assert.IsNotNull(_loggedInUserToken);
        }

        [TestMethod]
        public void when_calling_search_it_should_respond_with_bug_collection()
        {
            string searchCriteria =
                "email1=terry.densmore%40versionone.com&emailassigned_to1=1&emailtype1=equals&product=TestProduct&query_format=advanced&resolution=---&known_name=AssignedBugs";
            var response = _client.Search(searchCriteria);

            var count = response.Count();
            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public void when_calling_get_bug_it_should_return_a_bug()
        {
            int ID = 7;
            IBug bug = _client.GetBug(ID);

            Assert.IsNotNull(bug);
        }

        [TestMethod]
        public void when_calling_accept_bug_the_status_changes()
        {
            int ID = 7;
            string status = "IN_PROGRESS";
            _client.AcceptBug(ID, status);

            IBug bug = _client.GetBug(ID);

            Assert.AreEqual(bug.Status, status);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Bug #9999 does not exist.")]
        public void when_calling_a_bug_that_doesnt_exist_it_should_throw_an_exception()
        {
            int ID = 9999;
            IBug bug = _client.GetBug(ID);
        }

        [TestMethod]
        public void when_calling_resolve_bug_the_bug_status_should_change()
        {
            int ID = 7;
            string status = "RESOLVED";
            string resolution = "FIXED";
            IBug bug = _client.GetBug(ID);
            _client.ResolveBug(Int32.Parse(bug.ID), resolution);

            bug = _client.GetBug(ID);

            Assert.AreEqual(bug.Status, status);
        }

        [TestMethod]
        public void when_calling_resolve_bug_the_bug_should_get_a_comment()
        {
            int bugId = 26;
            string resolution = "FIXED";
            string expectedComment = $"Resolution has changed to {resolution} by VersionOne";

            _client.ResolveBug(bugId , resolution);

            var comment = _client.GetLastComment(bugId);

            Assert.IsTrue(comment == expectedComment);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void when_calling_resolve_bug_with_fixed_for_a_bug_with_open_dependencies_should_throw_an_exception()
        {
            var bugWithDependencyId = 127;
            var theDependantBugId = 128;

            var bugWithDependency = _client.GetBug(bugWithDependencyId);

            Assert.IsTrue(bugWithDependency.DependesOn.Count > 0);

            _client.ResolveBug(bugWithDependencyId, Resolution.FIXED.ToString());
        }

        [TestMethod]
        public void when_calling_reassign_bug_it_should_change_the_assigned_bug()
        {
            int ID = 7;

            var AssignedToUser = "denise@denise.com";

            _client.ReassignBug(ID, AssignedToUser);

            IBug bug = _client.GetBug(ID);

            Assert.AreEqual(bug.AssignedTo, AssignedToUser);
        }

        [TestMethod]
        public void when_udpating_a_bug_field_the_field_gets_its_new_value()
        {
            var fieldName = "summary";
            var fieldValue = "Today is a good day.";
            var expectedValue = fieldValue;

            var bugId = 25;

            _client.UpdateBug(bugId, fieldName, fieldValue);

            var bug = _client.GetBug(bugId);

            Assert.AreEqual(expectedValue, bug.Name);
        }
    }
}
