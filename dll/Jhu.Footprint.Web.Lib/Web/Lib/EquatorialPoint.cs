using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Jhu.Spherical;

namespace Jhu.Footprint.Web.Lib
{
    [DataContract]
    public class EquatorialPoint
    {
        public double RA { get; set; }
        public double Dec { get; set; }

        public static implicit operator EquatorialPoint(Cartesian c)
        {
            return new EquatorialPoint()
            {
                RA = c.RA,
                Dec = c.Dec
            };
        }

        public static implicit operator EquatorialPoint(GalacticPoint gp)
        {
            double d = CoordinateTransformations.GetDeclinationFromGalactic(gp.L, gp.B);

            return new EquatorialPoint()
            {
                Dec = d,
                RA = CoordinateTransformations.GetRightAscensionFromGalactic(gp.L, gp.B, d)
            };
        }
    }
}
