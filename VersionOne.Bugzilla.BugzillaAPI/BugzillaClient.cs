using Newtonsoft.Json.Linq;
using RestSharp;

namespace VersionOne.Bugzilla.BugzillaAPI
{
	public class BugzillaClient: IBugzillaClient
	{
        public RestClient Client{ get; set; }
		public string Token { get; set; }

        public BugzillaClient(string URL)
        {
            Client = new RestClient(URL);
        }

        public string Login(string username, string password)
		{
			var req = new RestRequest("login?{login}{password}",Method.GET);

			req.AddParameter("login", username);
			req.AddParameter("password", password);

			var result = Client.Get(req);

			var response = JObject.Parse(result.Content);
			Token = response["token"].ToString();

			return Token;
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