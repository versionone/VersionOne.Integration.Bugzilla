using System;
using VersionOne.ServiceHost.ConfigurationTool.Entities;
using VersionOne.Bugzilla.BugzillaAPI;

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
            var bugzillaClientConfiguration = new BugzillaClientConfiguration {Password = entity.Password, UserName = entity.UserName, Url = entity.Url};
            IBugzillaClient client = new BugzillaClient(bugzillaClientConfiguration);
            try
            {
                // client.Login(entity.UserName, entity.Password, false, entity.IgnoreCertificate.BoolValue);
                client.Login();
                return true;
            } catch (Exception) {
                return false;
            }
        }
    }
}