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
		string IntegrationUserToken { get; set; }

		string Login(string username, string password);

		JEnumerable<JToken> Search(string searchQuery);

		Bug GetBug(int bugId);

        IList<IComment> GetComments(int bugId);

        bool AcceptBug(int bugId, string newBugStatus);


        bool UpdateBug(int bugId, string fieldName, string fieldValue);

        bool ResolveBug(int bugId, string resolution);

        bool ReassignBug(int bugId, string assignTo);

    }

    public interface IBugzillaClientFactory
	{
		IBugzillaClient CreateNew(string url);
	}
}