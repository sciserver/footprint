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
    public class Circle : Shape
    {
        [DataMember(Name = "center", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Center of the circle.")]
        public Point Center { get; set; }

        [DataMember(Name = "radius", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Radius of the circle.")]
        public Angle Radius { get; set; }

        public override Spherical.Region GetRegion()
        {
            var c = Center.ToCartesian();
            var r = Radius.ToRadians();
            var hf = new Spherical.Halfspace(c, Math.Cos(r));
            var region = new Spherical.Region(hf);

            region.Simplify();
            return region;
        }


    }
}
