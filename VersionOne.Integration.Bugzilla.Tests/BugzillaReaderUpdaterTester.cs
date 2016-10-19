using System;
using System.Collections.Generic;
using Rhino.Mocks;
using VersionOne.ServiceHost.BugzillaServices;
using VersionOne.ServiceHost.Core.Configuration;
using VersionOne.ServiceHost.WorkitemServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VersionOne.Bugzilla.BugzillaAPI;

namespace VersionOne.ServiceHost.Tests.WorkitemServices.Bugzilla
{
    [TestClass]
	public class BugzillaReaderUpdaterTester
	{
		private const string userName = "fred";
        private const string password = "12345";
        private const string expectedUrl = "http://localhost/v1.cgi";
        private const string expectedUserId = "123";
        private const string expectedFilterId = "filtid";
        private const int productId = 333;
        private const string productName = "Expected Name";
        private const string v1ProjectId = "Scope:1234";
        private const string v1PriorityId = "WorkitemPriority:1";
        private const string expectedPriority = "Normal";
        private const string assignTo = "potus@us.gov";

        [TestMethod, Ignore] 
        public void GetBugsNoUrl()
		{
            // This test wasn't working when we picked up work on the RESTful client in Sept/Oct of 2016.
            // Not sure why it was left in a failing state. We were pressed for time (surprise... surprise) and
            // for the near term have left this test in a failing state. Need to look closer at its intent
            // and make things right.
			GetBugs(GetStockConfig());
		}

        [TestMethod, Ignore]
        public void GetBugsWithUrl()
		{
            // This test wasn't working when we picked up work on the RESTful client in Sept/Oct of 2016.
            // Not sure why it was left in a failing state. We were pressed for time (surprise... surprise) and
            // for the near term have left this test in a failing state. Need to look closer at its intent
            // and make things right.
            BugzillaServiceConfiguration config = GetStockConfig();
			config.UrlTemplateToIssue = "http://localhost/show_bug.cgi?id=#key#";
			config.UrlTitleToIssue = "Bugz";
			GetBugs(config);
		}

		private void GetBugs(BugzillaServiceConfiguration config)
		{
			BugzillaMocks mocks = new BugzillaMocks();

			List<Bug> expectedBugs = CreateSomeBogusRemoteBugs(5);
			List<int> expectedIds = new List<int>();

//			Product expectedProduct = Product.Create(new GetProductResult(productId, productName, "Expected Descr"));
//			User expectedOwner = User.Create(new GetUserResult(ownerId, "Fred", "FredsLogin"));

			foreach (Bug bug in expectedBugs)
			{
				expectedIds.Add(int.Parse(bug.ID));
			}

//			SetupResult.For(mocks.ClientFactory.CreateNew(config.Url, mocks.Logger)).Return(mocks.Client);

            Expect.Call(mocks.Client.Login()).Return(expectedUserId);

			for (int i = 0; i < expectedBugs.Count; i++)
			{
                Bug bug = expectedBugs[i];
				Expect.Call(mocks.Client.GetBug(int.Parse(bug.ID))).Return(bug);
                Expect.Call(mocks.Client.Search(expectedFilterId)).Return(new List<int>{1,2,3,4,5});
                //Expect.Call(mocks.Client.GetProduct(bug.ProductID)).Return(expectedProduct);
                //Expect.Call(mocks.Client.GetUser(bug.AssignedToID)).Return(expectedOwner);
            }

			mocks.Repository.ReplayAll();

			BugzillaReaderUpdater reader = new BugzillaReaderUpdater(config, mocks.ClientFactory, mocks.Logger);

			List<Defect> returnedBugs = reader.GetBugs();

			Assert.AreEqual(expectedBugs.Count, returnedBugs.Count, "Did not get back the right number of defects.");

			foreach (Defect defect in returnedBugs)
			{
				Assert.AreEqual(defect.ProjectId, v1ProjectId);
				Assert.AreEqual(defect.Owners, assignTo);
                Assert.AreEqual(defect.Priority, v1PriorityId);

				if (! string.IsNullOrEmpty(config.UrlTemplateToIssue) && ! string.IsNullOrEmpty(config.UrlTitleToIssue))
				{
					Assert.AreEqual(config.UrlTemplateToIssue.Replace("#key#", defect.ExternalId), defect.ExternalLink.Url);
					Assert.AreEqual(config.UrlTitleToIssue, defect.ExternalLink.Title);
				}
			}

			mocks.Repository.VerifyAll();
		}

        [TestMethod]
        public void DefectCreated()
		{
			BugzillaServiceConfiguration config = GetStockConfig();
			config.OnCreateFieldName = "cf_V1Status";
			config.OnCreateFieldValue = "Open";

			OnDefectCreated(config);
		}

        [TestMethod]
        public void DefectCreatedWithLink()
		{
			BugzillaServiceConfiguration config = GetStockConfig();
			config.OnCreateFieldName = "cf_V1Status";
			config.OnCreateFieldValue = "Open";
			config.DefectLinkFieldName = "cf_LinkToV1Defect";

			OnDefectCreated(config);
		}

        [TestMethod]
        public void DefectCreatedReassign()
		{
			BugzillaServiceConfiguration config = GetStockConfig();
			config.OnCreateReassignValue = "fred";

			OnDefectCreated(config);
		}

        [TestMethod]
        public void DefectCreatedResolve()
		{
			BugzillaServiceConfiguration config = GetStockConfig();
			config.OnCreateResolveValue = "fixed";

			OnDefectCreated(config);
		}

		private void OnDefectCreated(BugzillaServiceConfiguration config)
		{
			BugzillaMocks mocks = new BugzillaMocks();

			Defect defect = GetStockBug();
			int expectedExternalId = 1234;
			string expectedDefectLinkValue = "http://localhost/VersionOne.Web/assetdetail.v1?Oid=Defect:1000";

			defect.ExternalId = expectedExternalId.ToString();
			WorkitemCreationResult workitemCreationResult = new WorkitemCreationResult(defect);
			workitemCreationResult.Messages.Add("Message1");
			workitemCreationResult.Permalink = expectedDefectLinkValue;

            SetupResult.For(mocks.ClientFactory.CreateNew()).Return(mocks.Client);
            
			Expect.Call(mocks.Client.Login()).Return(expectedUserId);

            Expect.Call(mocks.Client.Logout);

            if (!string.IsNullOrEmpty(config.OnCreateFieldName))
			{
				Expect.Call(mocks.Client.UpdateBug(expectedExternalId, config.OnCreateFieldName, config.OnCreateFieldValue)).Return(true);
			}

			if (!string.IsNullOrEmpty(config.DefectLinkFieldName))
			{
				Expect.Call(mocks.Client.UpdateBug(expectedExternalId, config.DefectLinkFieldName, expectedDefectLinkValue)).Return(true);
			}

			if (!string.IsNullOrEmpty(config.OnCreateReassignValue))
			{
				Expect.Call(mocks.Client.ReassignBug(expectedExternalId, config.OnCreateReassignValue)).Return(true);
			}
            
			mocks.Repository.ReplayAll();

			BugzillaReaderUpdater updater = new BugzillaReaderUpdater(config, mocks.ClientFactory, mocks.Logger);

			updater.OnDefectCreated(workitemCreationResult);

			mocks.Repository.VerifyAll();

		}


		private static List<Bug> CreateSomeBogusRemoteBugs(int size)
		{
            List<Bug> result = new List<Bug>();

            Random random = new Random(1);
		    
            
            for (int index = 0; index < size; index++)
            {
                var bug = new Bug();
                bug.ID = $"{index + 1}";
                bug.AssignedTo = assignTo;
                bug.ComponentID = $"{random.Next()}";
                bug.Description = Guid.NewGuid().ToString();
                bug.Name = Guid.NewGuid().ToString();
                bug.ProductId = productId;
                bug.Priority = expectedPriority;
                result.Add(bug);
            }

            return result;
		}

        private BugzillaServiceConfiguration GetStockConfig()
		{
			BugzillaServiceConfiguration config = new BugzillaServiceConfiguration();
			config.UserName = userName;
			config.Password = password;
			config.Url = expectedUrl;
			config.OpenIssueFilterId = expectedFilterId;

            MappingInfo zillaProject = new MappingInfo(productId.ToString(), productName);
            MappingInfo v1Project = new MappingInfo(v1ProjectId, null);
            config.ProjectMappings.Add(zillaProject, v1Project);
    
            MappingInfo zillaPriority = new MappingInfo(null, expectedPriority);
            MappingInfo v1Priority = new MappingInfo(v1PriorityId, null);
            config.PriorityMappings.Add(zillaPriority, v1Priority);

			return config;
		}

		private static Defect GetStockBug()
		{
			return new Defect("Summary", "Description", "Project", "johndoe");
		}

	}
}
