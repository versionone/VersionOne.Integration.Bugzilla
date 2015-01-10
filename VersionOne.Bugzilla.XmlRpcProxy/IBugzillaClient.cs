using System;
using System.Collections.Generic;

namespace VersionOne.Bugzilla.XmlRpcProxy
{
	public interface IBugzillaClient
	{
		Version Version { get; }

		int Login(string username, string password, bool remember);
		void Logout();

		IList<int> GetBugs(string searchName);

		Bug GetBug(int bugId);
		Product GetProduct(int productId);
		User GetUser(int userId);

		bool AcceptBug(int bugId);
		bool ResolveBug(int bugId, string resolution, string comment);
		bool ReassignBug(int bugId, string assignTo);

		bool UpdateBug(int bugId, string fieldName, string fieldValue);

	    string GetFieldValue(int bugId, string fieldName);
	}

	public interface IBugzillaClientFactory
	{
		IBugzillaClient CreateNew(string url);
	}
}