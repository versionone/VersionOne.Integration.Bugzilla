using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace VersionOne.Bugzilla.BugzillaAPI
{
	public interface IBugzillaClient
	{
        //		Version Version { get; }
        //
        RestClient Client { get; set; }
		string Token { get; set; }

		string Login(string username, string password);

		JEnumerable<JToken> Search(string searchQuery);

		Bug GetBug(int bugId);

	}

	public interface IBugzillaClientFactory
	{
		IBugzillaClient CreateNew(string url);
	}
}