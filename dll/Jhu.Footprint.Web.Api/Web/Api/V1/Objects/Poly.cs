﻿using System;
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
    public class Poly : Shape
    {
        [DataMember(Name = "points", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Coordinate points. Avoid bowties.")]
        public Point[] Points { get; set; }

        public Poly()
        {
        }

        public override Spherical.Region GetRegion()
        {
            var points = new Cartesian[Points.Length];

            for (int i = 0; i < points.Length; i++)
            {
                points[i] = Points[i].ToCartesian();
            }

            var sb = new Spherical.ShapeBuilder();
            var cx = sb.CreatePolygon(points);
            var region = new Spherical.Region(cx, false);

            region.Simplify();
            return region;
        }
    }
}
