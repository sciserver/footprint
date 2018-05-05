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
        [DataMember(Name = "region", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("A region.")]
        public Region Region { get; set; }

        [DataMember(Name = "links", EmitDefaultValue = false)]
        [DefaultValue(null)]
        public Links Links { get; set; }

        public RegionResponse(Region region)
        {
            Region = region;
            /* TODO
            Links = new Links()
            {
                Self = FootprintService.GetUrl(null, region.re)
            };
            */
        }

        public RegionResponse(Lib.Footprint footprint, Lib.FootprintRegion region)
        {
            Region = new Region(footprint, region);
            Links = new Links()
            {
                Self = FootprintService.GetUrl(footprint, region)
            };
        }
    }
}
