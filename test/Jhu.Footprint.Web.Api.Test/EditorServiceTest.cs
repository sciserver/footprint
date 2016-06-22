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
        public void UnionTest()
        {
            var session = ResetTest(TestUser);
            NewTest(session, TestUser);
            var shape = GetTestShape(session, TestUser);
            
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
