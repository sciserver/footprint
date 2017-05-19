using System;
using System.Linq;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.Web.Services;

namespace Jhu.Footprint.Web.Api.V1
{
    [TestClass]
    public class EditorServiceTest : EditorApiTestBase
    {
        [TestMethod]
        public void ResetTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);
                client.Reset();
                var res = client.GetShape();
                Assert.AreEqual(0, res.ConvexList.Count);
            }
        }

        [TestMethod]
        public void NewTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);
                var region = GetTestRegion();
                client.New(region);
                var res = client.GetShape();
                Assert.AreEqual(1, res.ConvexList.Count);
            }
        }

        [TestMethod]
        public void UnionTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);
                var r1 = GetTestRegion();
                client.New(r1);
                var r2 = GetTestRegion("CIRCLE J2000 15 10 15");
                client.Union(r2);
                var res = client.GetShape();
                Assert.AreEqual(2, res.ConvexList.Count);
            }
        }

        [TestMethod]
        public void IntersectTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);
                var r1 = GetTestRegion();
                client.New(r1);
                var r2 = GetTestRegion("CIRCLE J2000 15 10 15");
                client.Intersect(r2);
                var res = client.GetShape();
                Assert.IsTrue(res.ConvexList.Count >= 1);
            }

        }

        [TestMethod]
        public void SubtractTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);
                var r1 = GetTestRegion();
                client.New(r1);
                var r2 = GetTestRegion("CIRCLE J2000 15 10 15");
                client.Subtract(r2);
                var res = client.GetShape();
                Assert.IsTrue(res.ConvexList.Count >= 1);
            }

        }

        [TestMethod]
        public void GrowTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);
                var r1 = GetTestRegion();
                client.New(r1);
                client.Grow(10);
                var res = client.GetShape();

                Assert.IsTrue(res.Area > r1.Region.GetRegion().Area);
            }

        }

        [TestMethod]
        public void SaveLoadTest()
        {

            using (var session = new RestClientSession())
            {
                var name = GetTestUniqueName();
                var client = CreateClient(session, TestUser);
                var r1 = GetTestRegion();
                client.New(r1);
                client.Save(TestUser, name, name, "");

                client.Load(TestUser, name, name);

                var res = client.GetShape();
                Assert.AreEqual(res.ToString(), r1.Region.GetRegion().ToString());
            }
        }

        [TestMethod]
        public void GetTestOutline()
        {
            using (var session = new RestClientSession())
            {
                var name = GetTestUniqueName();
                var client = CreateClient(session, TestUser);
                var r1 = GetTestRegion();
                client.New(r1);
                var url = EditorApiBaseUrl + "/outline";
                var buffer = session.HttpGet(url);
                Assert.IsTrue(buffer != null && buffer.Length > 0);
            }
        }

        [TestMethod]
        public void PlotTestFootprintRegion()
        {
            using (var session = new RestClientSession())
            {
                var name = GetTestUniqueName();
                var client = CreateClient(session, TestUser);
                var r1 = GetTestRegion();
                client.New(r1);
                var url = EditorApiBaseUrl + "/plot";
                var buffer = session.HttpGet(url,"image/png");
                Assert.IsTrue(buffer != null && buffer.Length > 0);
            }
        }

        [TestMethod]
        public void PlotTestFootprintRegionAdvanced()
        {
            using (var session = new RestClientSession())
            {
                var name = GetTestUniqueName();
                var client = CreateClient(session, TestUser);
                var r1 = GetTestRegion();
                client.New(r1);
                var url = EditorApiBaseUrl + "/plot";
                var json = "{ \"ra\": 10, \"dec\": 10 }";
                var data = System.Text.ASCIIEncoding.Default.GetBytes(json);
                var buffer = session.HttpPost(url, "image/png", "application/json", data);
                Assert.IsTrue(buffer != null && buffer.Length > 0);
            }
        }
    }
}
