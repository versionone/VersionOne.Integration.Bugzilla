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

        bool AcceptBug(int bugId);

        bool AcceptBug(int bugId, string status);

		bool StatusExists(string status);

        string SearchForComment(int iD);

        bool UpdateBug(int bugId, string fieldName, string fieldValue);

        bool ResolveBug(Bug bug, string resolution);

        bool ReassignBug(int bugId, string assignTo)

    }

	public interface IBugzillaClientFactory
	{
		IBugzillaClient CreateNew(string url);
	}
}