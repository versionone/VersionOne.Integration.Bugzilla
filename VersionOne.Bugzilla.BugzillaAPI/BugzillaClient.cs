using Newtonsoft.Json.Linq;
using RestSharp;

namespace VersionOne.Bugzilla.BugzillaAPI
{
	public class BugzillaClient: IBugzillaClient
	{
		public string Login(string username, string password)
		{
			var client = new RestClient("http://184.170.227.113/bugzilla/rest/");

			var req = new RestRequest("login?{login}{password}",Method.GET);

			req.AddParameter("login", username);
			req.AddParameter("password", password);

			var result = client.Get(req);

			var response = JObject.Parse(result.Content);

			return response["token"].ToString();
		}


//		public bool AcceptBug(int bugId)
//		{
//
//			var req = new RestRequest(string.Empty);
//
//			req.AddHeader("Authorization", "Token " + "123456");
//			req.AddParameter("application/json", "post data", ParameterType.RequestBody);
//			var result = Post(req);
//
//			return false;
//		}
//
//		public Bug GetBug(int bugId)
//		{
//			var args = new XmlRpcStruct { { "bugid", bugId } };
//			return Bug.Create(Proxy.GetBug(args));
//		}
//
//		private IRestResponse Post(IRestRequest req)
//		{
//
//			IRestClient client = new RestClient();
//			return client.Post(req);
//		}

	}
}