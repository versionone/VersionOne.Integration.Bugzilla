using System.Collections.Generic;

namespace VersionOne.Bugzilla.BugzillaAPI
{
    public interface IBugzillaClient
	{
        string Login();

        void Logout();

        List<int> Search(string searchQuery);

		IBug GetBug(int bugId);
        
        string GetLastComment(int bugId);

        bool AcceptBug(int bugId, string newBugStatus);
        
        bool UpdateBug(int bugId, string fieldName, string fieldValue);

        bool ResolveBug(int bugId, string resolution);

        bool ReassignBug(int bugId, string assignToUser);

        bool IsValidUser(string userId);
        
	    bool IsCurrentLoginCredentialsValid();
	}
}