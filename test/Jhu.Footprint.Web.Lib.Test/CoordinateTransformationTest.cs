using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Spherical;

namespace Jhu.Footprint.Web.Lib.Test
{
    [TestClass]
    public class CoordinateTransformationTest
    {
        [TestMethod]
        public void ConvertEquatorialToGalacticCTest1()
        {
            EquatorialPoint point = new Cartesian(15, 15);
            GalacticPoint galPoint = point;

            var output = String.Format("{0}, {1}", galPoint.L, galPoint.B);
            System.Diagnostics.Debug.WriteLine(output);
        }

        [TestMethod]
        public void ConvertEquatorialToGalacticTest2()
        {
            GalacticPoint galPoint = new Cartesian(15, 15);

            var output = String.Format("{0}, {1}", galPoint.L, galPoint.B);
            System.Diagnostics.Debug.WriteLine(output);
        }

        [TestMethod]
        public void ConvertGalacticToEquatorialTest1()
        {
            GalacticPoint gp = new Cartesian(15, 15);
            EquatorialPoint ep = gp;

            var output = String.Format("{0}, {1}", ep.RA, ep.Dec);
            System.Diagnostics.Debug.WriteLine(output);
             
        }
    }
}
