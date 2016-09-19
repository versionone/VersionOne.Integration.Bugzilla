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
                Status = bugResponse["status"].ToString(),
                Description = comment,
                IsOpen = bugResponse["is_open"].ToString(),
                DependesOn = bugResponse["depends_on"].ToList()
			};
            bug.ProductId = findProductId(bug);
            return bug;

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

        public bool ResolveBug(Bug bug, string resolution)
        {
            //validate id
            //var bug = GetBug(bugdep.ID);
            //get dependecies
            if (resolution.Equals(Resolution.FIXED.ToString()) && HasOpenDependencies(bug))
            {
                throw new Exception(String.Format("Still {0} unresolved bugs for bugID {1}", bug.DependesOn.Count, bug.ID));
            }

            //status can be CONFIRMED, IN_PROGRESS, RESOLVED 
            // or any value defined on the status combo
            ChangeStatus(bug, Status.RESOLVED.ToString(), resolution); //

            return true;
        }

        public bool AcceptBug(int bugId)
        {
            return true;
        }

        public bool UpdateBug(int bugId, string fieldName, string fieldValue)
        {
            //validate bug

            //check field to update

            return true;
        }

        public bool ReassignBug(int bugId, string assignTo)
        {
            var bug = GetBug(bugId);
            //validate assignto user /rest/user/
            if (IsValidUser(assignTo))
            {
                //check for strict isolation ??

                //user can edit on this product
                if (!UserCanEdit(bug))
                {
                    throw new Exception(String.Format("Invalid User group for User {0} and product {1} for bug {2}", bug.AssignedTo, bug.Product, bug.Name));
                }
                //change the assign to field ??

                //call change status
                ChangeStatus(bug, Status.CONFIRMED.ToString());
            }
            //   var args = new XmlRpcStruct { { "bugid", bugId }, { "assignto", assignTo } };
            return true;
        }

        private void ChangeStatus(Bug bug, string status, string resolution="")
        {
            if (StatusExists(status)) {
                var req = new RestRequest("bug/" + bug.ID, Method.PUT);
                if (HasResolution(resolution)) {
                    req.AddParameter("remaining_time", 0);
                    req.AddParameter("resolution", resolution);
                }
               
                req.AddParameter("status", status);
                req.AddParameter("token", Token);

                var result = Client.Put(req);

                if (HasResolution(resolution))
                {
                    CreateComment(bug, resolution);
                }

                var response = JObject.Parse(result.Content)["bugs"];
            }
        }

        private void CreateComment(Bug bug, string comment)
        {
            //need to be ordered asc way ??
            var req = new RestRequest("bug/" + bug.ID + "/comment", Method.POST);
            req.AddParameter("token", Token);
            req.AddParameter("comment", comment);

            var result = Client.Post(req);

            var response = JObject.Parse(result.Content);
            if (result.StatusCode == System.Net.HttpStatusCode.NotFound) throw new Exception(response["message"].ToString());

        }

        private bool HasOpenDependencies(Bug bug)
        {
            foreach (JToken bugId in bug.DependesOn)
            {
                var dependantBug = GetBug((int)bugId);
                if (dependantBug.IsOpen.Equals("true"))
                {
                    return true;
                }
            }
            return false;
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

        public bool UserCanEdit(Bug bug)
        {
            //looks for a list of product IDs a user can enter a bug against:
            var req = new RestRequest("rest/product_enterable", Method.GET);
            var result = Client.Get(req);
            var ids = JObject.Parse(result.Content)["ids"].ToList();

            return ids.Contains(bug.ProductId);
        }

        public int findProductId(Bug bug)
        {
            //look for the id of the product
            var reqId = new RestRequest("rest/product/" + bug.Product, Method.GET);
            var resultId = Client.Get(reqId);
            var response = JObject.Parse(resultId.Content);
            if (resultId.StatusCode == System.Net.HttpStatusCode.NotFound) throw new Exception(response["message"].ToString());
            var idProduct = (int)JObject.Parse(resultId.Content)["id"];
            return idProduct;
        }

        private bool IsValidUser(string assignTo)
        {
            var req = new RestRequest("rest/user/"+ assignTo, Method.GET);
            var result = Client.Get(req);
            var response = JObject.Parse(result.Content);

            if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new Exception(response["message"].ToString());
            }
 
            return (result.StatusCode == System.Net.HttpStatusCode.NotFound);
        }

        private string SearchForComment(int iD)
        {
            //need to be ordered asc way ??
            var req = new RestRequest("bug/" + iD + "/comment", Method.GET);
            var result = Client.Get(req);
            var response = JObject.Parse(result.Content)["bugs"];

            return response[iD.ToString()]["comments"][0]["text"].ToString();

        }

        private bool HasResolution(string resolution)
        {
            return !(String.IsNullOrEmpty(resolution));
        }

    }
}