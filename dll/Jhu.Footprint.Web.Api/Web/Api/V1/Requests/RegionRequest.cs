using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

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

        [DataMember(Name = "sources", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("When performing operations, conveys the list of names of source regions")]
        public string[] Sources { get; set; }

        public RegionRequest()
        {
        }

        public RegionRequest(Region region)
        {
            this.Region = region;
        }
    }

}
