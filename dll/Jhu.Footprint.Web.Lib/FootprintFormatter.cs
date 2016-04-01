using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Spherical;

namespace Jhu.Footprint.Web.Lib
{
    public static class FootprintFormatter
    {
        public static IEnumerable<EquatorialPoint> InterpolateOutlinePoints(Outline outline, double resolution)
        {
            if (resolution == 0)
            {
                resolution = 0.1;
            }

            resolution = resolution / 3600.0 / 180.0 * Math.PI;

            var res = new List<EquatorialPoint>();

            foreach (var loop in outline.LoopList)
            {
                int q = 0;
                foreach (var arc in loop.ArcList)
                {
                    // Starting point
                    if (q == 0)
                    {
                        res.Add(arc.Point1);
                    }

                    // If a small circle arc, interpolate
                    if (arc.Circle.Cos0 != 0)
                    {
                        var n = (int)Math.Min(1000, Math.Max(6, arc.Length / resolution));
                        var a = arc.Angle / n;

                        for (int i = 1; i < n - 1; i++)
                        {
                            var p = arc.GetPoint(i * a);
                            res.Add(p);
                        }
                    }

                    res.Add(arc.Point2);
                    q++;
                }
            }

            return res;
        }
    }
}
