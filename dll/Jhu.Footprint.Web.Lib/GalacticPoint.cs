using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Spherical;

namespace Jhu.Footprint.Web.Lib
{
    public class GalacticPoint
    {

        public double L
        { get; set; }

        public double B
        { get; set; }

        public static implicit operator GalacticPoint(EquatorialPoint point)
        {
            var b = CoordinateTransformations.GetLatitudeFromEquatorial(point.RA, point.Dec);
            return new GalacticPoint()
             {
                 B = b,
                 L = CoordinateTransformations.GetLongitudeFromEquatorial(point.RA, point.Dec, b)
             };
        }

        public static implicit operator GalacticPoint(Cartesian point)
        {
            var b = CoordinateTransformations.GetLatitudeFromEquatorial(point.RA, point.Dec);
            return new GalacticPoint()
            {
                B = b,
                L = CoordinateTransformations.GetLongitudeFromEquatorial(point.RA, point.Dec,b)
            };
        }
    }
}
