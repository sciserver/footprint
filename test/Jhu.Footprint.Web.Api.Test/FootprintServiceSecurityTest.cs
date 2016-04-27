using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.ServiceModel;
using System.ServiceModel.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lib = Jhu.Footprint.Web.Lib;
using Jhu.Spherical;
using Jhu.Graywulf.Web.Api.V1;
using Jhu.Graywulf.Web.Services;


namespace Jhu.Footprint.Web.Api.V1
{
    [TestClass]
    public class FootprintServiceSecurityTest : FootprintApiTestBase
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            InitializeDatabase();
        }

        protected Footprint CreateTestFootprint(string user, string owner, string name, bool @public)
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, user);
                var req = new FootprintRequest()
                {
                    Footprint = new Footprint()
                    {
                        Public = @public
                    }
                };
                return client.CreateUserFootprint(owner, name, req).Footprint;
            }
        }

        protected Footprint GetTestFootprint(string user, string owner, string name)
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, user);
                return client.GetUserFootprint(owner, name).Footprint;
            }
        }

        #region Create footprint tests

        [TestMethod]
        public void CreateUserFootprintTest()
        {
            var name = GetTestUniqueName();

            var footprint = CreateTestFootprint(TestUser, TestUser, name, false);

            Assert.AreEqual(footprint.Owner, TestUser);
            Assert.AreEqual(footprint.Name, name);
            Assert.AreEqual(footprint.Type, Lib.FootprintType.None);
            Assert.AreEqual(footprint.Public, false);
        }

        [TestMethod]
        public void GrantCreateGroupFootprintByAdminTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(GroupAdminUser, TestGroup, name, true);
        }

        [TestMethod]
        public void GrantCreateGroupFootprintByWriterTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(GroupWriterUser, TestGroup, name, true);
        }

        [TestMethod]
        [ExpectedException(typeof(MessageSecurityException))]
        public void DenyCreateGroupFootprintByReaderTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(GroupWriterUser, TestGroup, name, true);
        }

        [TestMethod]
        [ExpectedException(typeof(MessageSecurityException))]
        public void DenyCreateGroupFootprintByOtherTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(OtherUser, TestGroup, name, true);
        }

        #endregion
        #region Get footprint tests

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
        public void GetNonexistingUserFootprintTest()
        {
            var name = GetTestUniqueName();

            GetTestFootprint(TestUser, TestUser, name);
        }

        [TestMethod]
        public void GrantGetPublicUserFootprintTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(TestUser, TestUser, name, true);
            var f2 = GetTestFootprint(OtherUser, TestUser, name);

            Assert.AreEqual(f1.Name, f2.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(MessageSecurityException))]
        public void DenyGetPrivateUserFootprintTest()
        {
            var name = GetTestUniqueName();

            CreateTestFootprint(TestUser, TestUser, name, false);
            GetTestFootprint(OtherUser, TestUser, name);
        }

        [TestMethod]
        public void GrantGetPrivateGroupFootprintTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(GroupWriterUser, TestGroup, name, false);
            var f2 = GetTestFootprint(GroupReaderUser, TestGroup, name);
        }

        [TestMethod]
        public void GrantGetPublicGroupFootprintTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(GroupWriterUser, TestGroup, name, true);
            var f2 = GetTestFootprint(OtherUser, TestGroup, name);
        }

        [TestMethod]
        [ExpectedException(typeof(MessageSecurityException))]
        public void DenyGetPrivateGroupFootprintTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(GroupWriterUser, TestGroup, name, false);
            var f2 = GetTestFootprint(OtherUser, TestGroup, name);
        }

        #endregion
        #region Modify footprint tests

        [TestMethod]
        public void ModifyUserFootprintTest()
        {
            var owner = TestUser;
            var name = GetTestUniqueName();

            CreateTestFootprint(owner, owner, name, true);

            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);
                var footprint = client.GetUserFootprint(owner, name).Footprint;

                footprint.Type = Lib.FootprintType.Intersection;

                var req = new FootprintRequest()
                {
                    Footprint = footprint
                };

                footprint = client.ModifyUserFootprint(owner, name, req).Footprint;

                Assert.AreEqual(owner, footprint.Owner);
                Assert.AreEqual(name, footprint.Name);
            }
        }

        [TestMethod]
        public void GrantModifyPrivateUserFootprintTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void DenyModifyPrivateUserFootprintTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void GrantModifyPrivateGroupFootprintTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void DenyModifyPrivateGroupFootprintTest()
        {
            throw new NotImplementedException();
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
