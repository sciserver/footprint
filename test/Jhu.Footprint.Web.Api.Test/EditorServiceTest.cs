using System;
using System.Linq;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.Web.Services;

namespace Jhu.Footprint.Web.Api.V1
{
    [TestClass]
    public class EditorServiceTest: EditorApiTestBase
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

        protected void IntersectTest(Spherical.Region region)
        {

        }

        protected void SubtractTest(Spherical.Region region)
        {

        }

        protected void GrowTest(double arcmin)
        {

        }

        protected void LoadTest(string owner, string name, string regionName)
        {

        }

        protected void SaveTest(string owner, string name, string regionName)
        {

        }

        protected byte[] GetTestOutline()
        {
            throw new NotImplementedException();
        }

        protected byte[] PlotTestFootprintRegion()
        {
            throw new NotImplementedException();
        }

        protected byte[] PlotTestFootprintRegionAdvanced()
        {
            throw new NotImplementedException();
        }
    }
}
