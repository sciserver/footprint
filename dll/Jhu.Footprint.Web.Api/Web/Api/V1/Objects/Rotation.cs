using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.ComponentModel;
using Util = Jhu.Graywulf.Web.Api.Util;
using Lib = Jhu.Footprint.Web.Lib;

namespace Jhu.Footprint.Web.Api.V1
{
    [DataContract]
    [Description("Represents a rotation in Euler angles.")]
    public class Rotation
    {
        private Spherical.Rotation rotation;

        [DataMember(Name = "alpha", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("First Euler angle.")]
        public double? Alpha { get; set; }

        [DataMember(Name = "beta", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Second Euler angle.")]
        public double? Beta { get; set; }

        [DataMember(Name = "gamma", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Third Euler angle.")]
        public double? Gamma { get; set; }

        public Rotation()
        {
        }

        public Spherical.Rotation GetRotation()
        {
            if (rotation == null)
            {
                rotation = new Spherical.Rotation(Alpha.Value, Beta.Value, Gamma.Value);
            }

            return rotation;
        }
    }
}
