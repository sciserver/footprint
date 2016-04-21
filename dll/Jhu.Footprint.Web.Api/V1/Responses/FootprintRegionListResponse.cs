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
    [DataContract(Name = "footprintList")]
    [Description("Represents a list of footprints.")]
    public class FootprintRegionListResponse
    {
        [DataMember(Name = "footprints")]
        [Description("List of footprints.")]
        public FootprintRegion[] Footprints { get; set; }

        public FootprintRegionListResponse()
        {
        }

        public FootprintRegionListResponse(IEnumerable<Lib.Footprint> footprints, string folderName)
        {
            this.Footprints = footprints.Select(f => new FootprintRegion(f, folderName)).ToArray();
        }
    }
}
