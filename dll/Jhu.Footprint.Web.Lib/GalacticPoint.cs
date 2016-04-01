using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.Footprint.Web.Lib
{
    public class GalacticPoint
    {

        public double L
        { get; set; }

        public double B
        { get; set; }

        public static implicit operator GalacticPoint(Point point)
        {
                 var b = CoordinateTransformations.GetLatitude(point.Ra, point.Dec);
            return new GalacticPoint()
             {
                 B = b ,
                 L = CoordinateTransformations.GetLongitude(point.Ra, point.Dec, b)
             };
        }
    }
}
