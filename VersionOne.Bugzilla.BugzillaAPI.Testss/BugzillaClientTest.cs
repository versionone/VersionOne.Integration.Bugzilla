using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;


namespace VersionOne.Bugzilla.BugzillaAPI.Testss
{
	[TestClass]
	public class BugzillaClientTest
	{
		[TestMethod]
		public void when_calling_login_it_should_respond_with_a_token()
		{
			BugzillaClient client = new BugzillaClient();
			string token = client.Login("terry.densmore@versionone.com", "admin1425");

			Assert.IsNotNull(token);
		}
	}
}
