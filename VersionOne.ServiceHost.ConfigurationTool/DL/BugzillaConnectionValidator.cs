using System;
using VersionOne.ServiceHost.ConfigurationTool.Entities;
using VersionOne.Bugzilla.XmlRpcProxy;

namespace VersionOne.ServiceHost.ConfigurationTool.DL {
    public class BugzillaConnectionValidator : IConnectionValidator {
        private readonly BugzillaServiceEntity entity;

        public BugzillaConnectionValidator(BugzillaServiceEntity entity) {
            if(entity == null) {
                throw new ArgumentNullException();
            }
            this.entity = entity;
        }

        public bool Validate() {
            var client = new BugzillaClient(entity.Url);
            
            try {
                //TO DO: Need to make ignoreCert param to be from checkbox in UI. 
                client.Login(entity.UserName, entity.Password, false, true);
                return true;
            } catch (Exception) {
                return false;
            }
        }
    }
}