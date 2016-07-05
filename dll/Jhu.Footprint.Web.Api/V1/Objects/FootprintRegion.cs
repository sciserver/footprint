using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.ComponentModel;
using Util = Jhu.Graywulf.Web.Api.Util;
using Lib = Jhu.Footprint.Web.Lib;

namespace Jhu.Footprint.Web.Api.V1
{
    [DataContract(Name = "footprintRegion")]
    [Description("Represents a celestial region belonging to a footprint.")]
    public class FootprintRegion
    {
        [DataMember(Name = "owner")]
        [Description("Owner of the footprint.")]
        public string Owner { get; set; }

        [DataMember(Name = "footprintName")]
        [Description("Name of the footprint containing the region.")]
        public string FootprintName { get; set; }

        [DataMember(Name = "footprintUrl")]
        [Description("Footprint url.")]
        public Uri FootprintUrl { get; set; }

        [DataMember(Name = "name")]
        [Description("Name of the region.")]
        public string Name { get; set; }

        [DataMember(Name = "url")]
        [Description("Region url.")]
        public Uri Url { get; set; }

        [DataMember(Name = "fillFactor")]
        [Description("Fill factor of the region.")]
        public double? FillFactor { get; set; }

        [DataMember(Name = "regionString")]
        [Description("Region string.")]
        public string RegionString { get; set; }

        public FootprintRegion()
        {
        }

        public FootprintRegion(Lib.Footprint footprint, Lib.FootprintRegion region)
        {
            SetValues(footprint, region);
        }

        public Spherical.Region GetRegion()
        {
            if (RegionString != null)
            {
                var region = Spherical.Region.Parse(RegionString);
                region.Simplify();
                return region;
            }
            else
            {
                return null;
            }
        }

        public void GetValues(Lib.FootprintRegion region)
        {
            region.FillFactor = FillFactor ?? region.FillFactor;

            if (RegionString != null)
            {
                region.Region = Spherical.Region.Parse(RegionString);
            }
        }

        public void SetValues(Lib.Footprint footprint, Lib.FootprintRegion region)
        {
            this.Owner = footprint.Owner;
            this.FootprintName = footprint.Name;
            this.Name = region.Name;
            this.FillFactor = region.FillFactor;
            this.Url = FootprintService.GetUrl(footprint, region);
        }
    }
}
