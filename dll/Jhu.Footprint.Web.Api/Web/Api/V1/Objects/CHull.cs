using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel;
using Jhu.Spherical;

namespace Jhu.Footprint.Web.Api.V1
{
    [DataContract]
    public class CHull : Shape
    {
        [DataMember(Name = "p1", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Coordinate points. Must be on a single hemisphere")]
        public Point[] Points { get; set; }

        public override Spherical.Region GetRegion()
        {
            var points = new Cartesian[Points.Length];

            for (int i = 0; i < points.Length; i++)
            {
                points[i] = Points[i].ToCartesian();
            }

            var sb = new Spherical.ShapeBuilder();
            var cx = sb.CreateConvexHull(points);
            var region = new Spherical.Region(cx, false);

            region.Simplify();
            return region;
        }
    }
}
