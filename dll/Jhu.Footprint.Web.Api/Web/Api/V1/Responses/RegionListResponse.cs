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
    [Description("Represents a list of regions.")]
    public class RegionListResponse
    {
        [DataMember(Name = "regions")]
        [Description("List of regions.")]
        public  IEnumerable<Region> Regions { get; set; }

        public RegionListResponse()
        {
        }
    }
}
