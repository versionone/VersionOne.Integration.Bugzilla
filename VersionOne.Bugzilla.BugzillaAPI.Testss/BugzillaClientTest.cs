using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace VersionOne.Bugzilla.BugzillaAPI.Testss
{
    [TestClass()]
    public class BugzillaClientTest
    {

        private IBugzillaClient _client;
        private string _loggedInUserToken;

        [TestInitialize()]
        public void SetContext()
        {
            _client = new BugzillaClient("http://184.170.227.113/bugzilla/rest/");
            _loggedInUserToken = _client.Login("terry.densmore@versionone.com", "admin1425");
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
            Assert.IsNotNull(count);
        }

        [TestMethod]
        public void when_calling_get_bug_it_should_return_a_bug()
        {
            int ID = 7;
            Bug bug = _client.GetBug(ID);

            Assert.IsNotNull(bug);
        }

        [TestMethod, Ignore]
        public void when_calling_status_exists_it_should_return_true()
        {
            int ID = 7;
            string status = "CONFIRMED";
            //	bool exists = _client.StatusExists(status);

            //	Assert.IsTrue(exists);
        }

        [TestMethod]
        public void when_calling_accept_bug_the_status_change()
        {
            int ID = 7;
            string status = "IN_PROGRESS";
            _client.AcceptBug(ID, status);

            Bug bug = _client.GetBug(ID);

            Assert.AreEqual(bug.Status, status);
        }

        [TestMethod]
        public void when_calling_a_bug_that_dont_exist_it_should_throw_an_exception()
        {
            try
            {
                int ID = 9999;
                Bug bug = _client.GetBug(ID);
                Assert.Fail("no exception thrown");
            }
            catch (System.Exception ex)
            {
                Assert.IsTrue(ex.Message == "Bug #9999 does not exist.");
            }

        }

        [TestMethod]
        public void when_calling_resolve_bug_the_bug_status_should_change()
        {
            int ID = 7;
            string status = "RESOLVED";
            string resolution = "FIXED";
            Bug bug = _client.GetBug(ID);
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

            var comments = _client.GetComments(bugId);

            Assert.IsTrue(comments.First().Text == expectedComment);
        }


        [TestMethod, Ignore]
        public void when_calling_user_can_edit_the_user_should_be_able_to_do_it()
        {
            int ID = 7;

            Bug bug = _client.GetBug(ID);

            //  Assert.IsTrue(_client.UserCanEdit(bug));
        }

        [TestMethod, Ignore]
        public void when_calling_find_product_it_should_return_product_id()
        {
            int ID = 7;
            var product_id = 1;

            Bug bug = _client.GetBug(ID);

            //  Assert.AreEqual(_client.findProductId(bug), product_id);
        }

        [TestMethod]
        public void when_calling_reassign_bug_it_should_change_the_assigned_bug()
        {
            int ID = 7;

            var AssignedToUser = "denise@denise.com";

            _client.ReassignBug(ID, AssignedToUser);

            Bug bug = _client.GetBug(ID);

            Assert.AreEqual(bug.AssignedTo, AssignedToUser);
        }


        [TestMethod]
        public void when_calling_accept_bug_it_should_change_the_status()
        {
            int ID = 25;

            var newBugStatus = "IN_PROGRESS";

            _client.AcceptBug(ID, newBugStatus);

            Bug bug = _client.GetBug(ID);

            Assert.AreEqual(bug.Status, newBugStatus);
        }


        [TestMethod]
        public void when_calling_gets_comments_it_should_returns_all_the_comments_for_the_bug()
        {
            int ID = 25;
            
            var list = _client.GetComments(ID);

            Assert.IsNotNull(list);
        }
    }
}
