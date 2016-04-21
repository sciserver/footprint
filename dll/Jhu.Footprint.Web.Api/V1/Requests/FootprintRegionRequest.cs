using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace Jhu.Footprint.Web.Api.V1
{
    [DataContract]
    [Description("Represents a footprint.")]
    public class FootprintRegionRequest
    {
        [DataMember(Name = "footprint", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Conveys a footprint.")]
        public FootprintRegion Footprint { get; set; }

        public FootprintRegionRequest()
        {
        }
    }

}
