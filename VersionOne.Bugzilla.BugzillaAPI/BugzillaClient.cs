using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using System.Net;
using System.Text;
using VersionOne.ServiceHost.Core.Logging;

namespace VersionOne.Bugzilla.BugzillaAPI
{
    public class BugzillaClient: IBugzillaClient
	{
	    private readonly ILogger _logger;
	    private readonly string _username;
        private readonly string _password;
	    private string _integrationUserToken;

        public RestClient Client { get; }
	    
        public BugzillaClient(IBugzillaClientConfiguration configuration, ILogger logger)
	    {
            SetSslIgnoreErrorMode(configuration);
            this._logger = logger;
	        this._username = configuration.UserName;
	        this._password = configuration.Password;
            Client = new RestClient(configuration.Url);
        }

	    
	    public BugzillaClient(IBugzillaClientConfiguration configuration)
        {
            SetSslIgnoreErrorMode(configuration);
            this._username = configuration.UserName;
            this._password = configuration.Password;
            Client = new RestClient(configuration.Url);
        }

        private static void SetSslIgnoreErrorMode(IBugzillaClientConfiguration configuration)
        {
            if (configuration.IgnoreSSLCert)
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            }
        }

        public string Login()
        {
            var req = new RestRequest("login?", Method.GET);

            req.AddParameter("login", _username);
            req.AddParameter("password", _password);

            var result = Client.Get(req);

            var response = JObject.Parse(result.Content);

            if (result.StatusCode != HttpStatusCode.OK)
            {
                LogAndThrow("Error when trying to log in: ", response["message"].ToString());
            }

            _integrationUserToken = response["token"].ToString();

            return _integrationUserToken;
        }

        public void Logout()
        {
            var req = new RestRequest("logout?", Method.GET);

            req.AddParameter("token", _integrationUserToken);

            var result = Client.Get(req);

            var response = JObject.Parse(result.Content);

            if (result.StatusCode != HttpStatusCode.OK)
            {
                LogAndThrow("Error when trying to log out: ", response["message"].ToString());
            }
        }

	    public List<int> Search(string searchQuery)
		{
            var req = new RestRequest("bug?" + searchQuery, Method.GET);

			var result = Client.Get(req);

			var response = JObject.Parse(result.Content);

		    if (result.StatusCode != HttpStatusCode.OK)
		    {
                LogAndThrow("Error when trying to search for bugs: ", response["message"].ToString());
            }

            var bugIds = response["bugs"].Select(bug => (int) bug["id"]).ToList();
            return bugIds;
		}

		public IBug GetBug(int bugId)
		{
			var req = new RestRequest("bug/" + bugId, Method.GET);
			var result = Client.Get(req);

            var response = JObject.Parse(result.Content);

		    if (result.StatusCode != HttpStatusCode.OK)
		    {
                LogAndThrow($"Error when trying to get bug {bugId}: ", response["message"].ToString());
		    }

            var bugData = response["bugs"].First;

            var comment = GetFirstComment(bugId);

		    var bug = new Bug(bugData, comment);

            bug.ProductId = FindProductId(bug);

            return bug;
		}

	    public string GetLastComment(int bugId)
	    {
            var req = new RestRequest("bug/" + bugId + "/comment", Method.GET);
            req.AddParameter("token", _integrationUserToken);

            var result = Client.Get(req);
            
            var response = JObject.Parse(result.Content);

            if (result.StatusCode != HttpStatusCode.OK)
            {
                LogAndThrow($"Error when trying to get last comment for bug {bugId}: ", response["message"].ToString());
            }
            return (string) response["bugs"][$"{bugId}"]["comments"].ToList().Last()["text"];
        }

        private string GetFirstComment(int bugId)
        {
            var req = new RestRequest("bug/" + bugId + "/comment", Method.GET);
            var result = Client.Get(req);
            var response = JObject.Parse(result.Content)["bugs"];

            if (result.StatusCode != HttpStatusCode.OK)
            {
                LogAndThrow($"Error when getting first comment for bug {bugId}: ", response["message"].ToString());
            }

            return (string) response[bugId.ToString()]["comments"].ToList().First()["text"];
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
                LogAndThrow($"Error when trying to resolve bug {bugId}: ", String.Format("Still {0} unresolved bugs for bugID {1}", bug.DependesOn.Count(), bugId));
            }
            
             ChangeStatusAndResolve(bug, Status.RESOLVED.ToString(), resolution);

            return true;
        }

        public bool UpdateBug(int bugId, string fieldName, string fieldValue)
        {
            var req = new RestRequest("bug/" + bugId, Method.PUT);
            req.AddParameter(fieldName, fieldValue);
            req.AddParameter("token", _integrationUserToken);

            var result = Client.Put(req);
            
            if (result.StatusCode != HttpStatusCode.OK)
            {
                var response = JObject.Parse(result.Content);
                LogAndThrow($"Error when trying to update bug {bugId}: ", response["message"].ToString());
            }
            
            return true;
        }

        public bool ReassignBug(int bugId, string assignToUser)
        {
            var success = false;
            var bug = GetBug(bugId);

            if (IsValidUser(assignToUser))
            {
                bug.AssignedTo = assignToUser;

                var req = new RestRequest("bug/" + bug.ID, Method.PUT);
                req.AddParameter("application/json", bug.GetReassignBugPayload(_integrationUserToken), ParameterType.RequestBody);
                var result = Client.Put(req);

                if (result.StatusCode != HttpStatusCode.OK)
                {
                   var response = JObject.Parse(result.Content);
                   LogAndThrow($"Error when trying to reassign bug {bug.ID}: ", response["message"].ToString());
                }

                success = true;
            }
           
            return success;
        }

        private void ChangeStatus(IBug bug, string status)
        {
            if (StatusExists(status))
            {
                var req = new RestRequest("bug/" + bug.ID, Method.PUT);

                req.AddParameter("status", status);
                req.AddParameter("token", _integrationUserToken);

                var result = Client.Put(req);
                
                if (result.StatusCode != HttpStatusCode.OK)
                {
                    var response = JObject.Parse(result.Content);
                    LogAndThrow($"Error when trying to change status for bug {bug.ID}: ", response["message"].ToString());
                }
            }
        }

        private void ChangeStatusAndResolve(IBug bug, string status,  string resolution)
        {
            if (StatusExists(status))
            {
                var req = new RestRequest("bug/" + bug.ID, Method.PUT);

                req.AddParameter("remaining_time", 0);
                req.AddParameter("resolution", resolution);
                req.AddParameter("status", status);
                req.AddParameter("token", _integrationUserToken);

                var result = Client.Put(req);
                
                if (result.StatusCode != HttpStatusCode.OK)
                {
                    var response = JObject.Parse(result.Content);
                    LogAndThrow($"Error when trying to change status and resolve bug {bug.ID}: ", response["message"].ToString());
                }
                
                var comment = $"Resolution has changed to {resolution} by VersionOne";
                CreateComment(bug, comment);
            }
        }

        private void CreateComment(IBug bug, string comment)
        {
            var req = new RestRequest("bug/" + bug.ID + "/comment", Method.POST);
            req.AddParameter("token", _integrationUserToken);
            req.AddParameter("comment", comment);

            var result = Client.Post(req);
            
            if (result.StatusCode != HttpStatusCode.Created)
            {
                var response = JObject.Parse(result.Content);
                LogAndThrow($"Error when trying to create comment for bug {bug.ID}: ", response["message"].ToString());
            }
        }

        private bool HasOpenDependencies(IBug bug)
        {
            return bug.DependesOn.Select(bugId => GetBug(bugId)).Any(dependantBug => dependantBug.IsOpen);
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

            if (restResponse.StatusCode != HttpStatusCode.OK)
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
            
            if (result.StatusCode != HttpStatusCode.OK)
            {
                var response = JObject.Parse(result.Content);
                LogAndThrow($"Error when checking for valid user {userId}: ", response["message"].ToString());
            }
            
            return result.StatusCode == HttpStatusCode.OK;
        }

	    public bool IsCurrentLoginCredentialsValid()
	    {
            var req = new RestRequest("valid_login", Method.GET);
            req.AddParameter("login", _username);
            req.AddParameter("token", _integrationUserToken);

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