using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace VersionOne.Bugzilla.BugzillaAPI.Testss
{
	[TestClass()]
	public class BugzillaClientTest
	{
		private BugzillaClient client = new BugzillaClient("http://184.170.227.113/bugzilla/rest/");

		[TestMethod]
		public void when_calling_login_it_should_respond_with_a_token()
		{
			string token = client.Login("terry.densmore@versionone.com", "admin1425");

			Assert.IsNotNull(token);
		}

		[TestMethod]
		public void when_calling_search_it_should_respond_with_bug_collection()
		{
			string searchCriteria = "email1=terry.densmore%40versionone.com&emailassigned_to1=1&emailtype1=equals&product=TestProduct&query_format=advanced&resolution=---&known_name=AssignedBugs";
			var response = client.Search(searchCriteria);

			var count = response.Count();
			Assert.IsNotNull(count);
		}

		[TestMethod]
		public void when_calling_get_bug_it_should_return_a_bug()
		{
			int ID = 7;
			Bug bug = client.GetBug(ID);

			Assert.IsNotNull(bug);
		}

		[TestMethod]
		public void when_calling_status_exists_it_should_return_true()
		{
			int ID = 7;
			string status = "CONFIRMED";
		//	bool exists = client.StatusExists(status);

		//	Assert.IsTrue(exists);
		}

		[TestMethod]
        public void when_calling_accept_bug_the_status_change()
        {

            client.Login("terry.densmore@versionone.com", "admin1425");
            int ID = 7;
            string status = "IN_PROGRESS";
            client.AcceptBug(ID, status);

            Bug bug = client.GetBug(ID);

            Assert.AreEqual(bug.Status, status);
        }

        [TestMethod]
        public void when_calling_a_bug_that_dont_exist_it_should_throw_an_exception()
        {
            try
            {
                int ID = 9999;
                Bug bug = client.GetBug(ID);
                Assert.Fail("no exception thrown");
            }
            catch (System.Exception ex)
            {
                Assert.IsTrue(ex.Message == "Bug #9999 does not exist.");
            }
           
        }

        [TestMethod]
        public void when_calling_change_status_the_bug_status_should_change()
        {
            int ID = 7;
            string status = "RESOLVED";
            string resolution = "FIXED";
            Bug bug = client.GetBug(ID);
            client.ResolveBug(bug, resolution);
                      
            bug = client.GetBug(ID);

            Assert.AreEqual(bug.Status, status);
        }

        public void when_calling_user_can_edit_the_user_should_be_able_to_do_it()
        {
            int ID = 7;
         
            Bug bug = client.GetBug(ID);

          //  Assert.IsTrue(client.UserCanEdit(bug));
        }

        public void when_calling_find_product_it_should_return_product_id()
        {
            int ID = 7;
            var product_id = 1;

            Bug bug = client.GetBug(ID);

          //  Assert.AreEqual(client.findProductId(bug), product_id);
        }

        public void when_calling_reassign_bug_it_should_change_the_assigned_bug()
        {
            int ID = 7;

            var AssignedToUser = "denise@denise.com";

            Bug bug = client.GetBug(ID);

            client.ReassignBug(Int32.Parse(bug.ID), AssignedToUser);

            Assert.AreEqual(bug.AssignedTo,AssignedToUser);
        }

    }
}
