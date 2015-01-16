/***************************************************************************************
 * This code uses XML-RPC.NET Copyright (c) 2006 Charles Cook
***************************************************************************************/
using System;
using System.Collections.Generic;
using CookComputing.XmlRpc;

namespace VersionOne.Bugzilla.XmlRpcProxy {
    public class BugzillaClient : IBugzillaClient {
        public BugzillaClient(string url) {
            Proxy.Url = url;
        }

        private IServer proxy;

        private IServer Proxy {
            get {
                if (proxy == null) {
                    proxy = XmlRpcProxyGen.Create<IServer>();
                    proxy.KeepAlive = false;
                }
                return proxy;
            }
        }

        public Version Version {
            get { return new Version(Proxy.Version()); }
        }

        public int Login(string username, string password, bool remember) {
            try {
                //var args = new XmlRpcStruct {{"login", username}, {"password", password}, {"remember", (remember ? "1" : "0")}};
                var args = new XmlRpcStruct { { "login", username }, { "password", password } };
                var result = Proxy.Login(args);
                return int.Parse(result["id"].ToString());
            } catch (Exception ex) {
                throw new BugzillaException(
                    string.Format("Error attempting to log in to Bugzilla as ({0}) on {1}. {2}", username, Proxy.Url, ex.Message), ex);
            }
        }

        public IList<int> GetBugs(string searchName) {
            try {
                var args = new XmlRpcStruct {{"searchphrase", searchName}};
                return new List<int>(Proxy.GetBugs(args));
            } catch (Exception ex) {
                throw new BugzillaException(
                    string.Format("Error running saved search ({0}) on Bugzilla at {1}. {2}", searchName, Proxy.Url, ex.Message), ex);
            }
        }

        public void Logout() {
            Proxy.Logout();
        }

        public Bug GetBug(int bugId) {
            var args = new XmlRpcStruct {{"bugid", bugId}};
            return Bug.Create(Proxy.GetBug(args));
        }

        public Product GetProduct(int productId) {
            var args = new XmlRpcStruct {{"productid", productId}};
            return Product.Create(Proxy.GetProduct(args));
        }

        public User GetUser(int userId) {
            var args = new XmlRpcStruct {{"userid", userId}};
            return User.Create(Proxy.GetUser(args));
        }

        public bool AcceptBug(int bugId) {
            var args = new XmlRpcStruct {{"bugid", bugId}};
            return Proxy.AcceptBug(args);
        }

        /// <summary>
        /// Resolve Issue.
        /// </summary>
        /// <throws><see cref="BugzillaException"/>, when 'noresolveopenblockers' setting is turned on and Issue has unresolved dependencies.</throws>
        public bool ResolveBug(int bugId, string resolution, string comment) {
            try {
                var args = new XmlRpcStruct {{"bugid", bugId}, {"resolution", resolution}, {"comment", comment}};
                return Proxy.ResolveBug(args);
            } catch(XmlRpcFaultException ex) {
                throw new BugzillaException(string.Format("Failed to resolve bug {0} with resolution '{1}' and comment '{2}'", bugId, resolution, comment), ex);
            }
        }

        public bool ReassignBug(int bugId, string assignTo) {
            var args = new XmlRpcStruct {{"bugid", bugId}, {"assignto", assignTo}};
            return Proxy.ReassignBug(args);
        }

        public bool UpdateBug(int bugId, string fieldName, string fieldValue) {
            var args = new XmlRpcStruct {{"bugid", bugId}, {"fieldname", fieldName}, {"fieldvalue", fieldValue}};
            return Proxy.UpdateBug(args);
        }

        public string GetFieldValue(int bugId, string fieldName) {
            var args = new XmlRpcStruct {{"bugid", bugId}, {"fieldname", fieldName}};
            return Proxy.GetFieldValue(args);
        }
    }
}