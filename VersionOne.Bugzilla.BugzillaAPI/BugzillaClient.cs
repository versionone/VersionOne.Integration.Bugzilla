using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace VersionOne.Bugzilla.BugzillaAPI
{
	public class BugzillaClient: IBugzillaClient
	{
        public RestClient Client{ get; set; }
		public string Token { get; set; }

        public string CommentResolution = "Resolution has changed to {0} by VersionOne";

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

            var response = JObject.Parse(result.Content);
            if (result.StatusCode == System.Net.HttpStatusCode.NotFound) throw new Exception(response["message"].ToString());

            var bugResponse = response["bugs"].First;

            var comment = SearchForComment(ID);

			var bug = new Bug
			{
				ID = bugResponse["id"].ToString(),
				Name = bugResponse["summary"].ToString(),
				AssignedTo = bugResponse["assigned_to"].ToString(),
				ComponentID = bugResponse["component"].ToString(),
				Priority = bugResponse["priority"].ToString(),
				Product = bugResponse["product"].ToString(),
                Description  = comment
			};
			return bug;

		}

        private string SearchForComment(int iD)
        {
            //need to be ordered asc way ??
            var req = new RestRequest("bug/" + iD + "/comment", Method.GET);
            var result = Client.Get(req);
            var response = JObject.Parse(result.Content)["bugs"];

            return response[iD.ToString()]["comments"][0]["text"].ToString();

        }

        public bool AcceptBug(int bugId, string status)
        {
            //validate id
            var bug = GetBug(bugId);

            //if bug.target_milestone is empty
                //throws user errror

            //status can be CONFIRMED, IN_PROGRESS, RESOLVED 
            // or any value defined on the status combo
            ChangeStatus(bug, status); //
           
            return true;
        }

        public bool AcceptBug(int bugId)
        {
            return true;
        }

        public void ChangeStatus(Bug bug, string status, string resolution="")
        {
            if (StatusExists(status)) {
                //validate resolution ok?
                if (string.IsNullOrEmpty(resolution)) {
                    //set remaining time to 0
                    //remaining_time = 0;
                    //set resolution to pararesolution
                    //add comment
                    //set status 

                }
            }
            //change
        }


        public bool StatusExists(string status)
        {
            var req = new RestRequest("field/bug/status/values", Method.GET);
            var result = Client.Get(req);
            var statuses = JObject.Parse(result.Content)["values"].ToList();

			if (statuses.Contains(status))
			{
				return true;
			}

		    return false;
		}
    }
}