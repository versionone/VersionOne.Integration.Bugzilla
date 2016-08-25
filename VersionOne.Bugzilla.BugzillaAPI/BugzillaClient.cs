using System.Linq;
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

		public JEnumerable<JToken> Search(string searchQuery)
		{
			var req = new RestRequest("bug?" + searchQuery, Method.GET);
			//req.AddHeader("Authorization", "Basic" + Token);
			req.AddParameter("token", Token);

			var result = Client.Get(req);

			var response = JObject.Parse(result.Content);
			return response["bugs"].Children();
		}

		public Bug GetBug(int ID)
		{
			var req = new RestRequest("bug/" + ID, Method.GET);
			var result = Client.Get(req);

			var response = JObject.Parse(result.Content)["bugs"].First;

			var bug = new Bug
			{
				ID = response["id"].ToString(),
				Name = response["summary"].ToString(),
				AssignedTo = response["assigned_to"].ToString(),
				ComponentID = response["component"].ToString(),
				Priority = response["priority"].ToString(),
				Product = response["product"].ToString()
			};
			return bug;

		}
	}
}