using System;
using System.Linq;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

                Assert.AreEqual(2, fp.Footprints.Count());
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

                Assert.AreEqual(2, fp.Footprints.Count());
            }
        }

        [TestMethod]
        public void FindNoFootprintTest()
        {
            var name = GetTestUniqueName();

            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);
                var fp = client.FindFootprints(null, name + "%", 0, 0);

                Assert.AreEqual(0, fp.Footprints.Count());
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
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(TestUser, TestUser, name, true);
            var r1 = CreateTestRegion(TestUser, TestUser, name, name);
            var r2 = GetTestRegion(TestUser, TestUser, name, name);
            var r3 = ModifyTestRegion(TestUser, TestUser, name, name);

            Assert.AreEqual(0.7, r3.FillFactor);
        }

        [TestMethod]
        [ExpectedException(typeof(EndpointNotFoundException))]
        public void ModifyNonexistingRegionTest()
        {
            var name = GetTestUniqueName();

            var r3 = ModifyTestRegion(TestUser, TestUser, name, name);
        }

        [TestMethod]
        [ExpectedException(typeof(EndpointNotFoundException))]
        public void DeleteRegionTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(TestUser, TestUser, name, true);
            var r1 = CreateTestRegion(TestUser, TestUser, name, name);
            DeleteTestRegion(TestUser, TestUser, name, name);
            var r3 = GetTestRegion(TestUser, TestUser, name, name);
        }

        [TestMethod]
        [ExpectedException(typeof(EndpointNotFoundException))]
        public void DeleteNonexistingRegionTest()
        {
            var name = GetTestUniqueName();
            DeleteTestRegion(TestUser, TestUser, name, name);
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
        #region Footprint combined region get and plot test
        [TestMethod]
        public void GetUserFootprintShapeTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(TestUser, TestUser, name, true);
            var r1 = CreateTestRegion(TestUser, TestUser, name, name);


            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);
                var r2 = client.GetUserFootprintShape(TestUser, name);

                Assert.IsNotNull(r2);
            }
        }

        [TestMethod]
        public void GetUserFootprintOutlineTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(TestUser, TestUser, name, true);
            var r1 = CreateTestRegion(TestUser, TestUser, name, name);

            using (var session = new RestClientSession())
            {
                var url = "http://" + Environment.MachineName + "/footprint/api/v1/Footprint.svc/users/" + TestUser + "/footprints/" + name + "/outline";
                var buffer = session.HttpGet(url);
                Assert.IsTrue(buffer != null && buffer.Length > 0);
            }

        }

        [TestMethod]
        public void GetUserFootprintOutlinePointsTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(TestUser, TestUser, name, true);
            var r1 = CreateTestRegion(TestUser, TestUser, name, name);

            using (var session = new RestClientSession())
            {
                var url = "http://" + Environment.MachineName + "/footprint/api/v1/Footprint.svc/users/" + TestUser + "/footprints/" + name + "/outline/points";

                var buffer = session.HttpGet(url);
                Assert.IsTrue(buffer != null && buffer.Length > 0);
            }
        }

        [TestMethod]
        public void PlotUserFootprintTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(TestUser, TestUser, name, true);
            var r1 = CreateTestRegion(TestUser, TestUser, name, name);

            using (var session = new RestClientSession())
            {
                var url = "http://" + Environment.MachineName + "/footprint/api/v1/Footprint.svc/users/" + TestUser + "/footprints/" + name + "/plot";

                var buffer = session.HttpGet(url, "image/jpeg");
                Assert.IsTrue(buffer != null && buffer.Length > 0);
            }
        }

        [TestMethod]
        public void PlotUserFootprintAdvancedTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(TestUser, TestUser, name, true);
            var r1 = CreateTestRegion(TestUser, TestUser, name, name);


            using (var session = new RestClientSession())
            {
                // Sign in and initiate session
                var client = CreateClient(session, TestUser);

                var url = "http://" + Environment.MachineName + "/footprint/api/v1/Footprint.svc/users/" + TestUser + "/footprints/" + name + "/plot";
                var json = "{ \"ra\": 10, \"dec\": 10 }";
                var data = System.Text.ASCIIEncoding.Default.GetBytes(json);
                var buffer = session.HttpPost(url, "image/png", "application/json", data);
                Assert.IsTrue(buffer != null && buffer.Length > 0);
            }
        }

        #endregion
        #region Individual region get and plot

        [TestMethod]
        public void SetUserFootprintRegionShapeTest()
        {
            var name = GetTestUniqueName();

            // TODO: implement
        }

        [TestMethod]
        public void GetUserFootprinRegionShapeTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(TestUser, TestUser, name, true);
            var r1 = CreateTestRegion(TestUser, TestUser, name, name);

            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);
                var region = client.GetUserFootprintRegionShape(TestUser, name, name);

                Assert.IsTrue(region.Area > 0);
            }
        }

        [TestMethod]
        public void GetUserFootprintRegionOutlineTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(TestUser, TestUser, name, true);
            var r1 = CreateTestRegion(TestUser, TestUser, name, name);

            using (var session = new RestClientSession())
            {
                var url = "http://" + Environment.MachineName + "/footprint/api/v1/Footprint.svc/users/" + TestUser + "/footprints/" + name + "/regions/" + name + "/outline";

                var buffer = session.HttpGet(url);
                Assert.IsTrue(buffer != null && buffer.Length > 0);
            }

        }

        [TestMethod]
        public void GetUserFootprintRegionOutlinePointsTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(TestUser, TestUser, name, true);
            var r1 = CreateTestRegion(TestUser, TestUser, name, name);

            using (var session = new RestClientSession())
            {
                var url = "http://" + Environment.MachineName + "/footprint/api/v1/Footprint.svc/users/" + TestUser + "/footprints/" + name + "/regions/" + name + "/outline/points";

                var buffer = session.HttpGet(url);
                Assert.IsTrue(buffer != null && buffer.Length > 0);
            }
        }

        [TestMethod]
        public void PlotUserFootprintRegionTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(TestUser, TestUser, name, true);
            var r1 = CreateTestRegion(TestUser, TestUser, name, name);

            using (var session = new RestClientSession())
            {
                var url = "http://" + Environment.MachineName + "/footprint/api/v1/Footprint.svc/users/" + TestUser + "/footprints/" + name + "/regions/" + name + "/plot";

                var buffer = session.HttpGet(url, "image/png");
                Assert.IsTrue(buffer != null && buffer.Length > 0);
            }
        }

        [TestMethod]
        public void PlotUserFootprintRegionAdvancedTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(TestUser, TestUser, name, true);
            var r1 = CreateTestRegion(TestUser, TestUser, name, name);

            using (var session = new RestClientSession())
            {
                // Sign in and initiate session
                var client = CreateClient(session, TestUser);

                var url = "http://" + Environment.MachineName + "/footprint/api/v1/Footprint.svc/users/" + TestUser + "/footprints/" + name + "/regions/" + name + "/plot";
                var json = "{ \"ra\": 10, \"dec\": 10 }";
                var data = System.Text.ASCIIEncoding.Default.GetBytes(json);
                var buffer = session.HttpPost(url, "image/png", "application/json", data);
                Assert.IsTrue(buffer != null && buffer.Length > 0);
            }
        }
        #endregion

    }


#if false

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
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(TestUser, TestUser, name, true);
            var r1 = CreateTestRegion(TestUser, TestUser, name, name);

            using (var session = new RestClientSession())
            {
                var buffer = session.MakeWebRequest("http://localhost/footprint.....");
            }
        }
#endif

}
