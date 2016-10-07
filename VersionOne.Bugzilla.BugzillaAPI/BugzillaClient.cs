using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using VersionOne.ServiceHost.Core.Logging;

namespace VersionOne.Bugzilla.BugzillaAPI
{
    public class BugzillaClient: IBugzillaClient
	{
	    private readonly ILogger logger;
	    public RestClient Client{ get; set; }
		public string IntegrationUserToken { get; set; }

        public string TokenAssignToUser { get; set; }

        public string CommentResolution = "Resolution has changed to {0} by VersionOne";

        public BugzillaClient(string URL)
        {
            Client = new RestClient(URL);
        }

        public BugzillaClient(string URL, ILogger logger)
        {
            this.logger = logger;
            Client = new RestClient(URL);
        }

        public string Login(string username, string password)
		{
			var req = new RestRequest("login?{login}{password}",Method.GET);
            
			req.AddParameter("login", username);
			req.AddParameter("password", password);

			var result = Client.Get(req);

			var response = JObject.Parse(result.Content);

		    if (result.StatusCode != System.Net.HttpStatusCode.OK)
		    {
                LogAndThrow("Error when trying to log in: ", response["message"].ToString());
            }

            IntegrationUserToken = response["token"].ToString();

			return IntegrationUserToken;
		}

		public List<int> Search(string searchQuery)
		{
            var req = new RestRequest("bug?" + searchQuery, Method.GET);

			var result = Client.Get(req);

			var response = JObject.Parse(result.Content);

		    if (result.StatusCode != System.Net.HttpStatusCode.OK)
		    {
                LogAndThrow("Error when trying to search for bugs: ", response["message"].ToString());
            }

            var bugIds = response["bugs"].Select(bug => (int)bug["id"]).ToList();
            return bugIds;
		}

		public Bug GetBug(int id)
		{
			var req = new RestRequest("bug/" + id, Method.GET);
			var result = Client.Get(req);

            var response = JObject.Parse(result.Content);

		    if (result.StatusCode != System.Net.HttpStatusCode.OK)
		    {
                LogAndThrow($"Error when trying to get bug {id}: ", response["message"].ToString());
		    }

            var bugResponse = response["bugs"].First;

            var comment = GetFirstComment(id);

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
            bug.ProductId = FindProductId(bug);
            return bug;

		}

	    public string GetFieldValue(int bugId, string fieldName)
	    {
            var req = new RestRequest("bug/" + bugId, Method.GET);
            var result = Client.Get(req);

            var response = JObject.Parse(result.Content);

            // What to do in failure cases, not sure yet.
            // Need to account for when we go down the tagging code path.

	        var fieldValue = response["bugs"][0][fieldName].ToString();

	        return fieldValue;
	    }

	    public IComment GetLastComment(int bugId)
	    {
            var req = new RestRequest("bug/" + bugId + "/comment", Method.GET);
            req.AddParameter("token", IntegrationUserToken);

            var result = Client.Get(req);
            
            var response = JObject.Parse(result.Content);

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                LogAndThrow($"Error when trying to get bug comments for bug {bugId}: ", response["message"].ToString());
            }
            var responseComment = response["bugs"][$"{bugId}"]["comments"].ToList().Last();
	        var lastComment = new Comment {Text = (string) responseComment["text"]};

	        return lastComment;
        }

        public bool AcceptBug(int bugId, string newBugStatus)
        {
            //validate id
            var bug = GetBug(bugId);

            //if bug.target_milestone is empty
            //throws user errror

           ChangeStatus(bug, newBugStatus);
           
            return true;
        }

        public bool ResolveBug(int bugId, string resolution)
        {
            var bug = GetBug(bugId);

            if (resolution.Equals(Resolution.FIXED.ToString()) && HasOpenDependencies(bug))
            {
                LogAndThrow($"Error when trying to resolve bug {bugId}: ", String.Format("Still {0} unresolved bugs for bugID {1}", bug.DependesOn.Count, bugId));
            }
            
             ChangeStatusAndResolve(bug, Status.RESOLVED.ToString(), resolution);

            return true;
        }

        public bool UpdateBug(int bugId, string fieldName, string fieldValue)
        {
            var req = new RestRequest("bug/" + bugId, Method.PUT);
            req.AddParameter(fieldName, fieldValue);
            req.AddParameter("token", IntegrationUserToken);

            var result = Client.Put(req);
            
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var response = JObject.Parse(result.Content);
                LogAndThrow($"Error when trying to update bug {bugId}: ", response["message"].ToString());
            }
            
            return true;
        }

        public bool ReassignBug(int bugId, string AssignToUser)
        {
            var success = false;
            var bug = GetBug(bugId);

            if (IsValidUser(AssignToUser))
            {
                //check for strict isolation ??
                
                bug.AssignedTo = AssignToUser;

                var req = new RestRequest("bug/" + bug.ID, Method.PUT);
                req.AddParameter("application/json", bug.GetReassignBugPayload(IntegrationUserToken), ParameterType.RequestBody);
                var result = Client.Put(req);

                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                   var response = JObject.Parse(result.Content);
                   LogAndThrow($"Error when trying to reassign bug {bug.ID}: ", response["message"].ToString());
                }

                success = true;
            }
           
            return success;
        }

        private void ChangeStatus(Bug bug, string status)
        {
            if (StatusExists(status))
            {
                var req = new RestRequest("bug/" + bug.ID, Method.PUT);

                req.AddParameter("status", status);
                req.AddParameter("token", IntegrationUserToken);

                var result = Client.Put(req);
                
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    var response = JObject.Parse(result.Content);
                    LogAndThrow($"Error when trying to change status for bug {bug.ID}: ", response["message"].ToString());
                }
            }
        }

        private void ChangeStatusAndResolve(Bug bug, string status,  string resolution)
        {
            if (StatusExists(status))
            {
                var req = new RestRequest("bug/" + bug.ID, Method.PUT);

                req.AddParameter("remaining_time", 0);
                req.AddParameter("resolution", resolution);
                req.AddParameter("status", status);
                req.AddParameter("token", IntegrationUserToken);

                var result = Client.Put(req);
                
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    var response = JObject.Parse(result.Content);
                    LogAndThrow($"Error when trying to change status and resolve bug {bug.ID}: ", response["message"].ToString());
                }
                
                var comment = $"Resolution has changed to {resolution} by VersionOne";
                CreateComment(bug, comment);
            }
        }

        private void CreateComment(Bug bug, string comment)
        {
            var req = new RestRequest("bug/" + bug.ID + "/comment", Method.POST);
            req.AddParameter("token", IntegrationUserToken);
            req.AddParameter("comment", comment);

            var result = Client.Post(req);
            
            if (result.StatusCode != System.Net.HttpStatusCode.Created)
            {
                var response = JObject.Parse(result.Content);
                LogAndThrow($"Error when trying to create comment for bug {bug.ID}: ", response["message"].ToString());
            }
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

            LogAndThrow("Error when checking if a status exists: ", $"Could not find the status {status}. Please check this status exists in Bugzilla.");
            return false;
        }
        
        private int FindProductId(Bug bug)
        {
            var reqId = new RestRequest("product/" + bug.Product, Method.GET);
            var restResponse = Client.Get(reqId);
            var response = JObject.Parse(restResponse.Content);

            if (restResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                LogAndThrow($"Error when trying to find product {bug.Product}: ", response["message"].ToString());
            }

            var idProduct = (int)response["products"][0]["id"];
            return idProduct;
        }

        public bool IsValidUser(string userId)
        {
            var req = new RestRequest("user/"+ userId, Method.GET);
            var result = Client.Get(req);
            
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var response = JObject.Parse(result.Content);
                LogAndThrow($"Error when checking for valid user {userId}: ", response["message"].ToString());
            }
            
            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        private string GetFirstComment(int id)
        {
            var req = new RestRequest("bug/" + id + "/comment", Method.GET);
            var result = Client.Get(req);
            var response = JObject.Parse(result.Content)["bugs"];

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                LogAndThrow($"Error when getting first comment for bug {id}: ", response["message"].ToString());
            }

            return response[id.ToString()]["comments"][0]["text"].ToString();

        }

        private void LogAndThrow(string logMessage, string additionalDetail)
        {
            logger.Log(LogMessage.SeverityType.Error, logMessage + additionalDetail);
            throw new Exception(additionalDetail);
        }

    }
}