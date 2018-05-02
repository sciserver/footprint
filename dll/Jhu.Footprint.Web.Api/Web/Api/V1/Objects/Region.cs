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
    [DataContract(Name = "region")]
    [Description("Represents a celestial region belonging to a footprint.")]
    public class Region
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

        [DataMember(Name = "isSimplified")]
        [Description("True if the region is simplified and area is available.")]
        public bool? IsSimplified
        {
            get { return Region?.IsSimplified; }
            set { }
        }

        [DataMember(Name = "area")]
        [Description("Area in square degrees.")]
        public double? Area
        {
            get
            {
                if (Region != null && Region.IsSimplified && !Double.IsNaN(Region.Area))
                {
                    return Region.Area;
                }
                else
                {
                    return null;
                }
            }
            set { }
        }

        [DataMember(Name = "regionString")]
        [Description("Region string. Set this field to create a custom region")]
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

        [DataMember(Name = "circle", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Set this field to create a circle")]
        public Circle Circle { get; set; }

        [DataMember(Name = "rect", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Set this field to create a rectangle")]
        public Rect Rect { get; set; }

        [DataMember(Name = "poly", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Set this field to create a polygon")]
        public Poly Poly { get; set; }

        [DataMember(Name = "chull", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Set this field to create a convex hull")]
        public CHull CHull { get; set; }

        // TODO: pull out to service?
        [IgnoreDataMember]
        public int BrushIndex { get; set; }

        // TODO: pull out to service?
        [IgnoreDataMember]
        public int PenIndex { get; set; }

        public Region()
        {
        }

        public Region(Lib.Footprint footprint, Lib.FootprintRegion region)
        {
            SetValues(footprint, region);
        }

        private void InitializeMembers()
        {
            this.region = null;
            this.regionString = null;
        }

        public Spherical.Region GetRegion()
        {
            int count = 0;

            if (region == null && regionString != null)
            {
                region = Spherical.Region.Parse(regionString);
                region.Simplify();
                count++;
            }

            if (Circle != null)
            {
                region = Circle.GetRegion();
                count++;
            }

            if (Rect != null)
            {
                region = Rect.GetRegion();
                count++;
            }

            if (Poly != null)
            {
                region = Poly.GetRegion();
                count++;
            }

            if (CHull != null)
            {
                region = CHull.GetRegion();
                count++;
            }

            if (count > 1)
            {
                throw Error.MultipleRegionsSpecified();
            }

            if (region == null)
            {
                throw Error.NoRegionSpecified();
            }

            return region;
        }

        public void SetRegion(Spherical.Region value)
        {
            this.region = value;
        }

        public void GetValues(Lib.FootprintRegion region)
        {
            region.Name = Name ?? region.Name;
            region.FillFactor = FillFactor ?? region.FillFactor;

            if (this.region != null)
            {
                region.Region = this.Region;
            }
            else if (RegionString != null)
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
