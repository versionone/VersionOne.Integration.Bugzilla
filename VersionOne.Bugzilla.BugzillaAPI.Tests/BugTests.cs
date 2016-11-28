using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace VersionOne.Bugzilla.BugzillaAPI.Tests
{
    [TestClass()]
    public class Given_A_Bug
    {
        private IBug _bug;
        private string _expectedReassignToPayload;
        private string _commentWhenCreated;

        [TestInitialize()]
        public void Setup()
        {
            var representativeResponseFromBugzillaRestAPIForGettingABug = JObject.Parse("{\"bugs\":[{\"summary\":\"This one has a depencancy\",\"creator_detail\":{\"email\":\"terry.densmore@versionone.com\",\"name\":\"terry.densmore@versionone.com\",\"id\":1,\"real_name\":\"Terry Densmore\"},\"priority\":\"---\",\"deadline\":null,\"status\":\"CONFIRMED\",\"depends_on\":[128],\"groups\":[],\"cf_upper\":null,\"classification\":\"Unclassified\",\"id\":127,\"keywords\":[],\"last_change_time\":\"2016-11-10T15:01:07Z\",\"assigned_to\":\"terry.densmore@versionone.com\",\"url\":\"\",\"product\":\"TestProduct\",\"creator\":\"terry.densmore@versionone.com\",\"cf_versiononeurl\":\"\",\"resolution\":\"\",\"flags\":[],\"cf_versiononestate\":\"---\",\"version\":\"unspecified\",\"cf_upperurl\":\"\",\"component\":\"TestComponent\",\"qa_contact\":\"\",\"see_also\":[],\"creation_time\":\"2016-11-10T14:54:38Z\",\"cc\":[],\"platform\":\"PC\",\"assigned_to_detail\":{\"email\":\"terry.densmore@versionone.com\",\"name\":\"terry.densmore@versionone.com\",\"id\":1,\"real_name\":\"Terry Densmore\"},\"target_milestone\":\"---\",\"blocks\":[],\"is_open\":true,\"cc_detail\":[],\"whiteboard\":\"\",\"severity\":\"enhancement\",\"op_sys\":\"Windows\",\"alias\":[],\"is_creator_accessible\":true,\"is_cc_accessible\":true,\"dupe_of\":null,\"is_confirmed\":true}],\"faults\":[]}");
            var representativeBugData = representativeResponseFromBugzillaRestAPIForGettingABug["bugs"].First;
            
            _commentWhenCreated = "This bug is for a test";

            _bug = new Bug(representativeBugData, _commentWhenCreated);

            _expectedReassignToPayload = "{\r\n  \"assigned_to\": \"denise@denise.com\",\r\n  \"status\": \"CONFIRMED\",\r\n  \"token\": \"terry.densmore@versionone.com\"\r\n}";
        }

        [TestMethod]
        public void It_Should_Give_Appropriate_Reassignto_Payloads()
        {
            var integrationUser = "terry.densmore@versionone.com";
            _bug.AssignedTo = "denise@denise.com";

            Assert.IsTrue(_expectedReassignToPayload == _bug.GetReassignBugPayload(integrationUser));
        }

        [TestMethod]
        public void It_Should_Have_An_ID()
        {
            var expectedId = "127";
            Assert.IsTrue(_bug.ID == expectedId);
        }

        [TestMethod]
        public void It_Should_Have_A_Name()
        {
            var expectedName = "This one has a depencancy";
            Assert.IsTrue(_bug.Name == expectedName);
        }

        [TestMethod]
        public void It_Should_Have_An_Assigned_To()
        {
            var expectedAssignedTo = "terry.densmore@versionone.com";
            Assert.IsTrue(_bug.AssignedTo == expectedAssignedTo);
        }

        [TestMethod]
        public void It_Should_Have_An_Associated_Component()
        {
            var expectedComponent = "TestComponent";
            Assert.IsTrue(_bug.Component == expectedComponent);
        }

        [TestMethod]
        public void It_Should_Have_A_Priority()
        {
            var expectedPriority = "---";
            Assert.IsTrue(_bug.Priority == expectedPriority);
        }

        [TestMethod]
        public void It_Should_Have_A_Product()
        {
            var expectedProduct = "TestProduct";
            Assert.IsTrue(_bug.Product == expectedProduct);
        }


        [TestMethod]
        public void It_Should_Have_A_Status()
        {
            var expectedStatus = "CONFIRMED";
            Assert.IsTrue(_bug.Status == expectedStatus);
        }

        [TestMethod]
        public void It_Should_Have_A_Description()
        {
            Assert.IsTrue(_bug.Description == _commentWhenCreated);
        }

        [TestMethod]
        public void It_Should_Indicate_If_It_Is_Open()
        {
            Assert.IsTrue(_bug.IsOpen);
        }

        [TestMethod]
        public void It_Should_Indicate_If_It_Has_Dependencies()
        {
            Assert.IsTrue(_bug.DependesOn.Count() > 0);
        }
    }
}

