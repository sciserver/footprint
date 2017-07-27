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
    [DataContract(Name = "region")]
    [Description("Represents a region.")]
    public class FootprintRegionResponse
    {
        [DataMember(Name = "region")]
        [Description("A region.")]
        public FootprintRegion Region { get; set; }

        public FootprintRegionResponse(FootprintRegion region)
        {
            Region = region;
        }

        public FootprintRegionResponse(Lib.Footprint footprint, Lib.FootprintRegion region)
        {
            Region = new FootprintRegion(footprint, region);
        }
    }
}
