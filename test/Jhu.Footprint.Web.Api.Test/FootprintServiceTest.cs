using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.AccessControl;
using Jhu.Graywulf.Web.Api.V1;
using Jhu.Graywulf.Web.Services;

namespace Jhu.Footprint.Web.Api.V1
{
    [TestClass]
    public class FootprintServiceTest : FootprintApiTestBase
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            InitializeDatabase();
        }

        #region Footprint CRUD operation tests

        [TestMethod]
        public void GetUserFootprintTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(TestUser, TestUser, name, true);
            var f2 = GetTestFootprint(TestUser, TestUser, name);

            Assert.AreEqual(TestUser, f2.Owner);
            Assert.AreEqual(name, f2.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(EndpointNotFoundException))]
        public void GetNonexistingFootprintTest()
        {
            var name = GetTestUniqueName();

            GetTestFootprint(TestUser, TestUser, name);
        }

        [TestMethod]
        public void ModifyFootprintTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(TestUser, TestUser, name, true);
            var f2 = ModifyTestFootprint(TestUser, TestUser, name);

            Assert.AreEqual(TestUser, f2.Owner);
            Assert.AreEqual(name, f2.Name);
            Assert.AreEqual("modified", f2.Comments);
        }

        [TestMethod]
        [ExpectedException(typeof(EndpointNotFoundException))]
        public void ModifyNonexistingFootprintTest()
        {
            var name = GetTestUniqueName();
            var f1 = ModifyTestFootprint(TestUser, TestUser, name);
        }

        [TestMethod]
        [ExpectedException(typeof(EndpointNotFoundException))]
        public void DeleteFootprintTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(TestUser, TestUser, name, true);
            DeleteTestFootprint(TestUser, TestUser, name);

            var f2 = GetTestFootprint(TestUser, TestUser, name);
        }

        [TestMethod]
        [ExpectedException(typeof(EndpointNotFoundException))]
        public void DeleteNonexistingFootprintTest()
        {
            var name = GetTestUniqueName();
            DeleteTestFootprint(TestUser, TestUser, name);
        }

        #endregion
        #region Footprint search tests

        [TestMethod]
        public void FindUserFootprintTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(TestUser, TestUser, name + "_1", true);
            var f2 = CreateTestFootprint(TestUser, TestUser, name + "_2", true);

            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);
                var fp = client.FindUserFootprints(TestUser, name + "%", 0, 0);

                Assert.AreEqual(2, fp.Footprints.Length);
            }
        }

        [TestMethod]
        public void FindFootprintTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(TestUser, TestUser, name + "_1", true);
            var f2 = CreateTestFootprint(OtherUser, OtherUser, name + "_2", true);

            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);
                var fp = client.FindFootprints(null, name + "%", 0, 0);

                Assert.AreEqual(2, fp.Footprints.Length);
            }
        }

        #endregion
        #region Region CRUD operation tests

        [TestMethod]
        public void GetUserRegionTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(TestUser, TestUser, name, true);
            var r1 = CreateTestRegion(TestUser, TestUser, name, name);
            var r2 = GetTestRegion(TestUser, TestUser, name, name);

            Assert.AreEqual(TestUser, r2.Owner);
            Assert.AreEqual(name, r2.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(EndpointNotFoundException))]
        public void GetNonexistingRegionTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(TestUser, TestUser, name, true);
            var r1 = GetTestRegion(TestUser, TestUser, name, name);
        }

        [TestMethod]
        public void ModifyRegionTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [ExpectedException(typeof(EndpointNotFoundException))]
        public void ModifyNonexistingRegionTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [ExpectedException(typeof(EndpointNotFoundException))]
        public void DeleteRegionTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [ExpectedException(typeof(EndpointNotFoundException))]
        public void DeleteNonexistingRegionTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void DeleteFootprintWithRegionsTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(TestUser, TestUser, name, true);
            var r1 = CreateTestRegion(TestUser, TestUser, name, name);

            DeleteTestFootprint(TestUser, TestUser, name);
        }

        #endregion

#if false
        [TestMethod]
        public void GetUserFootprintFolderTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var folder = client.GetUserFootprintFolder("evelin", "SDSS.DR7");
            }
        }

        [TestMethod]
        public void GetUserFootprintFolderRegionTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var footprint = client.GetUserFootprintFolderRegion("evelin", "SDSS.DR7");
            }
        }

        [TestMethod]
        public void GetUserFootprintFolderRegionOutlineTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var folder = client.GetUserFootprintFolderRegionOutline("evelin", "SDSS.DR7");
            }
        }

        [TestMethod]
        public void GetUserFootprintFolderRegionOutlinePointsTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var folder = client.GetUserFootprintFolderRegionOutlinePoints("evelin", "SDSS.DR7", 0.3);
            }

        }

        [TestMethod]
        public void GetUserFootprintFolderRegionConvexHullTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var folder = client.GetUserFootprintFolderRegionConvexHull("evelin", "SDSS.DR7");
            }
        }

        [TestMethod]
        public void GetUserFootprintFolderRegionConvexHullOutlineTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var folder = client.GetUserFootprintFolderRegionConvexHullOutline("evelin", "SDSS.DR7");
            }
        }

        [TestMethod]
        public void GetUserFootprintFolderRegionConvexHullOutlinePointsTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var folder = client.GetUserFootprintFolderRegionConvexHullOutlinePoints("evelin", "SDSS.DR7", 0.3);
            }
        }

        [TestMethod]
        public void GetUserFootprintFolderRegionReducedOutlineTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var footprint = client.GetUserFootprintFolderRegionReducedOutline("evelin", "SDSS.DR7", 100);
            }

        }

        [TestMethod]
        public void GetUserFootprintFolderRegionReducedOutlinePointsTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var footprint = client.GetUserFootprintFolderRegionReducedOutlinePoints("evelin", "SDSS.DR7", 0.1, 100);
            }

        }

        [TestMethod]
        public void GetUserFootprintFolderPlotTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var footprint = client.GetUserFootprintFolderPlot("evelin", "SDSS.DR7", "Equirectangular", 0f, 0f, "", true, true, true);
            }
        }

        [TestMethod]
        public void CreateUserFootprintFolderTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);

                var request = new FootprintRequest();
                var folder = new Lib.FootprintFolder();

                folder.Comments = "Test Api Create Folder";
                folder.Owner = "Evelin";
                folder.Name = "Test Api";
                folder.Type = FootprintType.Intersection;

                request.FootprintFolder = new V1.Footprint(folder);
                client.CreateUserFootprintFolder(request.FootprintFolder.User, request.FootprintFolder.Name, request);
            }
        }

        [TestMethod]
        public void ModifyUserFootprintFolderTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);

                var folder = new Jhu.Footprint.Web.Lib.FootprintFolder();
                var request = new FootprintRequest();

                using (var context = new Context())
                {
                    folder.Context = context;
                    folder.Id = 8;
                    folder.Load();
                }

                folder.Comments = "Api modification test.";

                request.FootprintFolder = new V1.Footprint(folder);

                client.ModifyUserFootprintFolder(request.FootprintFolder.User, request.FootprintFolder.Name, request);
            }
        }

        [TestMethod]
        public void DeleteUserFootprintFolderTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);

                client.DeleteUserFootprintFolder("kate", "COSMOS");
            }
        }

        [TestMethod]
        public void GetUserFootprintTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var footprint = client.GetUserFootprint("evelin", "SDSS.DR7", "Stripe2");
            }
        }

        [TestMethod]
        public void GetUserFootprintRegionTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var footprint = client.GetUserFootprintRegion("evelin", "SDSS.DR7", "Stripe2");
            }
        }



        [TestMethod]
        public void GetUserFootprintRegionOutlineTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var footprint = client.GetUserFootprintRegionOutline("evelin", "SDSS.DR7", "Stripe2");
            }
        }

        [TestMethod]
        public void GetUserFootprintRegionOutlinePointsTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var footprint = client.GetUserFootprintRegionOutlinePoints("evelin", "SDSS.DR7", "Stripe2", 0.9);
            }
        }

        [TestMethod]
        public void GetUserFootprintRegionConvexHullTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var footprint = client.GetUserFootprintRegionConvexHullOutline("evelin", "SDSS.DR7", "Stripe5");
            }

        }

        [TestMethod]
        public void GetUserFootprintRegionConvexHullOutlineTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var footprint = client.GetUserFootprintRegionConvexHullOutline("evelin", "SDSS.DR7", "Stripe5");
            }

        }

        [TestMethod]
        public void GetUserFootprintRegionConvexHullOutlinePointsTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var footprint = client.GetUserFootprintRegionConvexHullOutlinePoints("evelin", "SDSS.DR7", "Stripe5", 0.1);
            }

        }

        [TestMethod]
        public void GetUserFootprintRegionReducedOutlineTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var footprint = client.GetUserFootprintRegionReducedOutline("evelin", "SDSS.DR7", "Stripe5", 100);
            }

        }

        [TestMethod]
        public void GetUserFootprintRegionReducedOutlinePointsTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var footprint = client.GetUserFootprintRegionReducedOutlinePoints("evelin", "SDSS.DR7", "Stripe5", 0.1, 100);
            }

        }

        [TestMethod]
        public void GetUserFootprintPlotTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var footprint = client.GetUserFootprintPlot("evelin", "SDSS.DR7", "Stripe5", "Equirectangular", 0f, 0f, "", true, true, true);
            }
        }



        [TestMethod]
        public void CreatUserFootprintTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var request = new FootprintRegionRequest();


                var footprint = new Lib.Footprint();
                footprint.Comment = "Test Api Create Footprint";
                footprint.User = "Evelin";
                footprint.Name = "Test api";
                footprint.FolderId = 1;

                request.Region = new V1.FootprintRegion(footprint, "SDSS.DR7");

                client.CreateUserFootprint(request.Region.User, "SDSS.DR7", request.Region.Name, request);

            }
        }

        [TestMethod]
        public void ModifyUserFootprintTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);

                var footprint = new Jhu.Footprint.Web.Lib.Footprint();
                var request = new FootprintRegionRequest();

                using (var context = new Context())
                {
                    footprint.Context = context;
                    footprint.Id = 4;
                    footprint.Load();
                }

                footprint.Comment = "Api modification test.";

                request.Region = new V1.FootprintRegion(footprint, "2MASS");

                client.ModifyUserFootprint("mike", "2MASS", "South", request);
            }

        }

        [TestMethod]
        public void DeleteUserFootprintTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);

                client.DeleteUserFootprint("evelin", "SDSS.DR7", "ApiDelete");
            }
        }

#endif
    }
}
