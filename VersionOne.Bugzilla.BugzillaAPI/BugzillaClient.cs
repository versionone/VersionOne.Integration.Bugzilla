using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace VersionOne.Bugzilla.BugzillaAPI
{
	public class BugzillaClient: IBugzillaClient
	{
        public RestClient Client{ get; set; }
		public string IntegrationUserToken { get; set; }

        public string TokenAssignToUser { get; set; }

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
                        
            if (result.StatusCode == System.Net.HttpStatusCode.NotFound) throw new Exception(response["message"].ToString());

            IntegrationUserToken = response["token"].ToString();

			return IntegrationUserToken;
		}

		public JEnumerable<JToken> Search(string searchQuery)
		{
			var req = new RestRequest("bug?" + searchQuery, Method.GET);
			//req.AddHeader("Authorization", "Basic" + IntegrationUserToken);
			req.AddParameter("token", IntegrationUserToken);

			var result = Client.Get(req);

			var response = JObject.Parse(result.Content);
            
            if (result.StatusCode == System.Net.HttpStatusCode.NotFound) throw new Exception(response["message"].ToString());

            return response["bugs"].Children();
		}

		public Bug GetBug(int ID)
		{
			var req = new RestRequest("bug/" + ID, Method.GET);
			var result = Client.Get(req);

            var response = JObject.Parse(result.Content);
            if (result.StatusCode != System.Net.HttpStatusCode.OK) throw new Exception(response["message"].ToString());

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
           // ChangeStatus(bug, status); //
           
            return true;
        }

        public bool ResolveBug(int bugId, string resolution)
        {
            //validate id
            var bug = GetBug(bugId);
            //get dependecies
            if (resolution.Equals(Resolution.FIXED.ToString()) && HasOpenDependencies(bug))
            {
                throw new Exception(String.Format("Still {0} unresolved bugs for bugID {1}", bug.DependesOn.Count, bugId));
            }

            //status can be CONFIRMED, IN_PROGRESS, RESOLVED 
            // or any value defined on the status combo
            ChangeStatus(bug, Status.RESOLVED.ToString(), resolution); //

            return true;
        }

        //<CreateFieldId>cf_versiononestate</CreateFieldId>
        //<CreateFieldValue>New</CreateFieldValue>

        //DefectLinkFieldId --configuration.DefectLinkFieldName
        //createdResult.Permalink --

        //configuration.OnStateChangeFieldName-- CloseFieldId
        //configuration.OnStateChangeFieldValue--CloseFieldValue

        public bool UpdateBug(int bugId, string fieldName, string fieldValue)
        {
            //validate bug
            //my $f = new Bugzilla::Field({ name => $field });

            //if obsolete??{

                //if field == FIELD_TYPE_SINGLE_SELECT{
                  //checkfield
           //     }
              //  Updatefield
         //       return true;
           // }
           return false;
        }

        public bool ReassignBug(int bugId, string AssignToUser)
        {
            var response = false;
            var bug = GetBug(bugId);

            if (IsValidUser(AssignToUser))
            {
                //check for strict isolation ??
                
                bug.AssignedTo = AssignToUser;

                var req = new RestRequest("bug/" + bug.ID, Method.PUT);
                req.AddParameter("application/json", bug.GetReassignBugPayload(IntegrationUserToken), ParameterType.RequestBody);
                Client.Put(req);

                response = true;
            }
           
            return response;
        }

        private void ChangeStatus(Bug bug, string status, string resolution="")
        {
            if (StatusExists(status)) {
                var req = new RestRequest("bug/" + bug.ID, Method.PUT);
                if (HasResolution(resolution)) {
                    req.AddParameter("remaining_time", 0);
                    req.AddParameter("resolution", resolution);
                }

                req.AddParameter("assigned_to", bug.AssignedTo);
                req.AddParameter("status", status);
                req.AddParameter("token", IntegrationUserToken);

                var result = Client.Put(req);

                var response = JObject.Parse(result.Content);

                if (result.StatusCode == System.Net.HttpStatusCode.NotFound) throw new Exception(response["message"].ToString());

                if (HasResolution(resolution))
                {
                    CreateComment(bug, resolution);
                }

                //var response = JObject.Parse(result.Content)["bugs"];
            }
        }

        private void CreateComment(Bug bug, string comment)
        {
            //need to be ordered asc way ??
            var req = new RestRequest("bug/" + bug.ID + "/comment", Method.POST);
            req.AddParameter("token", IntegrationUserToken);
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

        private bool StatusExists(string status)
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
        
        private int findProductId(Bug bug)
        {
            //look for the id of the product
            var reqId = new RestRequest("product/" + bug.Product, Method.GET);
            var restResponse = Client.Get(reqId);
            var responseContent = JObject.Parse(restResponse.Content);
            if (restResponse.StatusCode == System.Net.HttpStatusCode.NotFound) throw new Exception(responseContent["message"].ToString());
            var idProduct = (int)responseContent["products"][0]["id"];
            return idProduct;
        }

        private bool IsValidUser(string userId)
        {
            var req = new RestRequest("user/"+ userId, Method.GET);
            var result = Client.Get(req);
            var response = JObject.Parse(result.Content);

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception(response["message"].ToString());
            }
            
            return result.StatusCode == System.Net.HttpStatusCode.OK;
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