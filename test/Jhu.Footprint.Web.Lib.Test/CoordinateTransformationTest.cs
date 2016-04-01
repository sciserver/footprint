using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Spherical;

namespace Jhu.Footprint.Web.Lib.Test
{
    [TestClass]
    public class CoordinateTransformationTest
    {
        [TestMethod]
        public void ConvertToGalacticCoordinatesTest()
        {
            Point point = new Cartesian(15, 15);
            GalacticPoint galPoint = point;

            var output = String.Format("{0}, {1}", galPoint.L, galPoint.B);
            System.Diagnostics.Debug.WriteLine(output);
        }
    }
}
