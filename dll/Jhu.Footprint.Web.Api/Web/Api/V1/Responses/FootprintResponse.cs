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
    [Description("Represents a footprint.")]
    public class FootprintResponse
    {
        [DataMember(Name = "footprint", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Footprint.")]
        public Footprint Footprint { get; set; }

        [DataMember(Name = "links", EmitDefaultValue = false)]
        [DefaultValue(null)]
        public Links Links { get; set; }

        public FootprintResponse()
        {
        }

        public FootprintResponse(Footprint footprint)
        {
            Footprint = footprint;
        }

        public FootprintResponse(Lib.Footprint footprint)
        {
            Footprint = new Footprint(footprint);
            Links = new Links()
            {
                Self = FootprintService.GetUrl(footprint)
            };
        }
    }
}
