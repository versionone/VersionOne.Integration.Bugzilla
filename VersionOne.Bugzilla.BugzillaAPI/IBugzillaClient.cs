using RestSharp;
using System.Collections.Generic;

namespace VersionOne.Bugzilla.BugzillaAPI
{
    public interface IBugzillaClient
	{
        //		Version Version { get; }
        //
        RestClient Client { get; set; }
		string IntegrationUserToken { get; set; }

		string Login(string username, string password);

		List<int> Search(string searchQuery);

		Bug GetBug(int bugId);

        string GetFieldValue(int bugId, string fieldName);

        IComment GetLastComment(int bugId);

        bool AcceptBug(int bugId, string newBugStatus);
        
        bool UpdateBug(int bugId, string fieldName, string fieldValue);

        bool ResolveBug(int bugId, string resolution);

        bool ReassignBug(int bugId, string assignTo);

        bool IsValidUser(string userId);

    }
    
}