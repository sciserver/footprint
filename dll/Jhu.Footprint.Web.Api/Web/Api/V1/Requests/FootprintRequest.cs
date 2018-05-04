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
    public class FootprintRequest
    {
        [DataMember(Name = "footprint", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Conveys a footprint.")]
        public Footprint Footprint { get; set; }

        [DataMember(Name = "source", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("When performing operations, conveys the name of source footprint.")]
        public string Source { get; set; }

        public FootprintRequest()
        {
        }

        public FootprintRequest(Footprint footprint)
        {
            Footprint = footprint;
        }
    }
}
