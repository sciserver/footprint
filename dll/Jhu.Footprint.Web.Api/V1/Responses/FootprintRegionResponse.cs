using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.ServiceModel;

namespace Jhu.Footprint.Web.Api.V1
{
    [DataContract(Name = "footprint")]
    [Description("Represents a footprint.")]
    public class FootprintRegionResponse
    {
        [DataMember(Name = "footprint")]
        [Description("A footprint.")]
        public FootprintRegion Footprint { get; set; }

        public FootprintRegionResponse(FootprintRegion footprint)
        {
            this.Footprint = footprint;
        }
    }
}
