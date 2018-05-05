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
    public class Rect : Shape
    {
        [DataMember(Name = "p1", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Coordinates of the first corner.")]
        public Point P1 { get; set; }

        [DataMember(Name = "p2", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Coordinates of the second corner.")]
        public Point P2 { get; set; }

        public override Spherical.Region GetRegion()
        {
            var p1 = P1.ToCartesian();
            var p2 = P2.ToCartesian();

            var sb = new Spherical.ShapeBuilder();
            var cx = sb.CreateRectangle(p1, p2);
            var region = new Spherical.Region(cx, false);

            region.Simplify();
            return region;
        }
    }
}
