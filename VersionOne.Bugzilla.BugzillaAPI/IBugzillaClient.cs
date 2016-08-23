using System;
using System.Collections.Generic;

namespace VersionOne.Bugzilla.BugzillaAPI
{
	public interface IBugzillaClient
	{
//		Version Version { get; }
//
        string Login(string username, string password);
//		void Logout();
//
//        //Bug GetBug(int bugId);
//		//Product GetProduct(int productId);
//		//User GetUser(int userId);
//
//		bool AcceptBug(int bugId);
//
//        bool AcceptBug(int bugId, string status);
//
//		bool ResolveBug(int bugId, string resolution, string comment);
//		bool ReassignBug(int bugId, string assignTo);
//
//		bool UpdateBug(int bugId, string fieldName, string fieldValue);
//
//	    string GetFieldValue(int bugId, string fieldName);
//	    IList<int> LoginSearch(string userName, string password, bool b, string openIssueFilterId, bool ignoreCert);
	}

	public interface IBugzillaClientFactory
	{
		IBugzillaClient CreateNew(string url);
	}
}