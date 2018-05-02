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
        public Plot Plot { get; set; }

        public Footprint[] Footprints { get; set; }

        public Region[] Regions { get; set; }
    }
}
