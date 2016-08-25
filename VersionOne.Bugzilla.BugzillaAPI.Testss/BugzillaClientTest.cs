using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using RestSharp;


namespace VersionOne.Bugzilla.BugzillaAPI.Testss
{
	[TestClass]
	public class BugzillaClientTest
	{
		[TestMethod]
		public void when_calling_login_it_should_respond_with_a_token()
		{
			BugzillaClient client = new BugzillaClient("http://184.170.227.113/bugzilla/rest/");
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
	}
}
