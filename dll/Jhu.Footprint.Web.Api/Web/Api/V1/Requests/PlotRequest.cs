using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace Jhu.Footprint.Web.Api.V1
{
    [DataContract]
    public class PlotRequest
    {
        [DataMember(Name = "plot", EmitDefaultValue = false)]
        [DefaultValue(null)]
        public Plot Plot { get; set; }

        [DataMember(Name = "regions", EmitDefaultValue = false)]
        [DefaultValue(null)]
        public Region[] Regions { get; set; }

        [DataMember(Name = "selection", EmitDefaultValue = false)]
        [DefaultValue(null)]
        public string[] Selection { get; set; }
    }
}
