using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using Util = Jhu.Graywulf.Web.Api.Util;

namespace Jhu.Footprint.Web.Api.V1
{
    [DataContract]
    [Description("Represents a region.")]
    public class RegionRequest
    {
        [DataMember(Name = "region", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Conveys a region.")]
        public Region Region { get; set; }

        [DataMember(Name = "rotation", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Defines a rotation to be applied to the region.")]
        public Rotation Rotation { get; set; }

        [DataMember(Name = "sys", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Original coordinate system, will be converted to equatorial J2000.")]
        public CoordinateSystem? CoordinateSystem { get; set; }

        [DataMember(Name = "selection", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("When performing operations, conveys the name of source regions.")]
        public string[] Selection { get; set; }

        [DataMember(Name = "keepOriginal", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Keep original region.")]
        public bool? KeepOriginal { get; set; }

        public RegionRequest()
        {
        }

        public RegionRequest(Region region)
        {
            this.Region = region;
            this.CoordinateSystem = V1.CoordinateSystem.EqJ2000;
            this.Rotation = null;
            this.KeepOriginal = null;
        }
    }

}
