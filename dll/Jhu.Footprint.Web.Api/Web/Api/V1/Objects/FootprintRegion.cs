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
        private Spherical.Region region;
        private string regionString;

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
        public string RegionString
        {
            get
            {
                // We could generate the region string here when the
                // region itself is available but we don't because we
                // don't want to return the region itself in every response
                return regionString;
            }
            set
            {
                this.region = null;
                this.regionString = value;
            }
        }

        // TODO: pull out to service?
        [IgnoreDataMember]
        public int BrushIndex { get; set; }

        // TODO: pull out to service?
        [IgnoreDataMember]
        public int PenIndex { get; set; }

        [IgnoreDataMember]
        public Spherical.Region Region
        {
            get
            {
                if (region == null && regionString != null)
                {
                    region = Spherical.Region.Parse(regionString);
                    region.Simplify();
                }

                return region;
            }
            set { this.region = value; }
        }

        public FootprintRegion()
        {
        }

        public FootprintRegion(Lib.Footprint footprint, Lib.FootprintRegion region)
        {
            SetValues(footprint, region);
        }

        private void InitializeMembers()
        {
            this.region = null;
            this.regionString = null;
        }

        public void GetValues(Lib.FootprintRegion region)
        {
            region.FillFactor = FillFactor ?? region.FillFactor;

            if (RegionString != null)
            {
                region.Region = this.Region;
            }
        }

        public void SetValues(Lib.Footprint footprint, Lib.FootprintRegion region)
        {
            this.Owner = footprint.Owner;
            this.FootprintName = footprint.Name;
            this.Name = region.Name;
            this.FillFactor = region.FillFactor;
            this.Url = FootprintService.GetUrl(footprint, region);
            this.Region = region.Region;
        }
    }
}
