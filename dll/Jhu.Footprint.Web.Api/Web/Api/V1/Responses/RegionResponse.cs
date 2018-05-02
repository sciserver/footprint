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
    [DataContract]
    [Description("Represents a region.")]
    public class RegionResponse
    {
        [DataMember(Name = "region")]
        [Description("A region.")]
        public Region Region { get; set; }

        public RegionResponse(Region region)
        {
            Region = region;
        }

        public RegionResponse(Lib.Footprint footprint, Lib.FootprintRegion region)
        {
            Region = new Region(footprint, region);
        }
    }
}
