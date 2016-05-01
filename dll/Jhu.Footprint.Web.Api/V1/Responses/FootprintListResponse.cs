using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.ServiceModel;
using Jhu.Graywulf.Web.Services;
using Lib = Jhu.Footprint.Web.Lib;

namespace Jhu.Footprint.Web.Api.V1
{
    [DataContract(Name = "footprintList")]
    [Description("Represents a list of footprints.")]
    public class FootprintListResponse //: StreamingListResponse<Footprint>
    {
        [DataMember(Name = "footprints")]
        [Description("List of footprints.")]
        public IEnumerable<Footprint> Footprints { get; set; }

        public FootprintListResponse()
        {
        }

        public FootprintListResponse(IEnumerable<Lib.Footprint> footprints)
        {
            this.Footprints = footprints.Select(f => new Footprint(f));
        }
    }
}
