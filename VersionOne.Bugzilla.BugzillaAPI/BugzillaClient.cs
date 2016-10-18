using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Remoting.Messaging;
using VersionOne.ServiceHost.Core.Logging;

namespace VersionOne.Bugzilla.BugzillaAPI
{
    public class BugzillaClient: IBugzillaClient
	{
	    private readonly ILogger _logger;
	    private readonly string _username;
        private readonly string _password;
        public RestClient Client{ get; set; }
		public string IntegrationUserToken { get; set; }

        public string CommentResolution = "Resolution has changed to {0} by VersionOne";
	    
        public BugzillaClient(IBugzillaClientConfiguration configuration, ILogger logger)
	    {
            if (configuration.IgnoreSSLCert)
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            }
            this._logger = logger;
	        this._username = configuration.UserName;
	        this._password = configuration.Password;
            Client = new RestClient(configuration.Url);
        }

        public BugzillaClient(IBugzillaClientConfiguration configuration)
        {
            if (configuration.IgnoreSSLCert)
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            }
            this._username = configuration.UserName;
            this._password = configuration.Password;
            Client = new RestClient(configuration.Url);
        }
        
        public string Login()
        {
            var req = new RestRequest("login?", Method.GET);

            req.AddParameter("login", _username);
            req.AddParameter("password", _password);

            var result = Client.Get(req);

            var response = JObject.Parse(result.Content);

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                LogAndThrow("Error when trying to log in: ", response["message"].ToString());
            }

            IntegrationUserToken = response["token"].ToString();

            return IntegrationUserToken;
        }

        public void Logout()
        {
            var req = new RestRequest("logout?", Method.GET);

            req.AddParameter("token", IntegrationUserToken);

            var result = Client.Get(req);

            var response = JObject.Parse(result.Content);

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                LogAndThrow("Error when trying to log out: ", response["message"].ToString());
            }
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

	    public string GetLastComment(int bugId)
	    {
            var req = new RestRequest("bug/" + bugId + "/comment", Method.GET);
            req.AddParameter("token", IntegrationUserToken);

            var result = Client.Get(req);
            
            var response = JObject.Parse(result.Content);

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                LogAndThrow($"Error when trying to get last comment for bug {bugId}: ", response["message"].ToString());
            }
            return (string) response["bugs"][$"{bugId}"]["comments"].ToList().Last()["text"];
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

            return (string) response[id.ToString()]["comments"].ToList().First()["text"];
        }

        public bool AcceptBug(int bugId, string newBugStatus)
        {
            var bug = GetBug(bugId);

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

	    public bool IsCurrentLoginCredentialsValid()
	    {
            var req = new RestRequest("valid_login", Method.GET);
            req.AddParameter("login", _username);
            req.AddParameter("token", IntegrationUserToken);

            var result = Client.Get(req);
	        var response = JObject.Parse(result.Content);
            
	        bool credentialsCheck = false;
            var validUser = response["result"];

            if (result.StatusCode == HttpStatusCode.OK && validUser != null && (bool) validUser)
	        {
	            credentialsCheck = true;
	        } 
	        
            return credentialsCheck;
	    }

	    private void LogAndThrow(string logMessage, string additionalDetail)
        {
            _logger.Log(LogMessage.SeverityType.Error, logMessage + additionalDetail);
            throw new Exception(additionalDetail);
        }

    }
}