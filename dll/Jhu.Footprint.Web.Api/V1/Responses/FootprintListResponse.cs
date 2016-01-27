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
    [Description("Represents a list of footprint queues.")]
    public class FootprintListResponse
    {
        [DataMember(Name = "footprints")]
        [Description("")]
        public Footprint[] Footprints { get; set; }

        public FootprintListResponse()
        {
        }

        public FootprintListResponse(Footprint footprint)
        {
            this.Footprints = new[] { footprint };
        }
    }
}
