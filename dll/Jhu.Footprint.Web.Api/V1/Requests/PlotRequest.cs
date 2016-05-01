using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace Jhu.Footprint.Web.Api.V1
{
    public class PlotRequest
    {
        public Plot Plot { get; set; }

        public Footprint[] Footprints { get; set; }

        public FootprintRegion[] Regions { get; set; }
    }
}
