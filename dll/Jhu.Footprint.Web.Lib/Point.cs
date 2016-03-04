using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Spherical;

namespace Jhu.Footprint.Web.Lib
{
    public class Point
    {
        public double Ra { get; set; }
        public double Dec { get; set; }

        public static implicit operator Point(Cartesian c)
        {
            return new Point()
            {
                Ra = c.RA,
                Dec = c.Dec
            };
        }
    }
}
