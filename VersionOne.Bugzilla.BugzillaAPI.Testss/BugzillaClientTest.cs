using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
			bool exists = client.StatusExists(status);

			Assert.IsTrue(exists);
		}

		[TestMethod]
        public void when_calling_accept_bug_the_status_change()
        {
            int ID = 7;
            string status = "ASSIGNED";
            client.AcceptBug(ID, status);

            Bug bug = client.GetBug(ID);

           // Assert.AreEqual(bug.s)
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
    }
}
