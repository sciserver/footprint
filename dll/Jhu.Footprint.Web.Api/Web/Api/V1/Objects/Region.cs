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

        private bool? isSimplified;
        private double? area;

        [DataMember(Name = "owner", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Owner of the footprint.")]
        public string Owner { get; set; }

        [DataMember(Name = "footprintName", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Name of the footprint containing the region.")]
        public string FootprintName { get; set; }

        [DataMember(Name = "footprintUrl", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Footprint url.")]
        public Uri FootprintUrl { get; set; }

        [DataMember(Name = "name", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Name of the region.")]
        public string Name { get; set; }

        [DataMember(Name = "fillFactor", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Fill factor of the region.")]
        public double? FillFactor { get; set; }

        [DataMember(Name = "isSimplified", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("True if the region is simplified and area is available.")]
        public bool? IsSimplified
        {
            get
            {
                GetRegion(false);

                if (region != null)
                {
                    return region.IsSimplified;
                }
                else
                {
                    return isSimplified;
                }
            }
            set { isSimplified = value; }
        }

        [DataMember(Name = "area", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Area in square degrees.")]
        public double? Area
        {
            get
            {
                GetRegion(false);

                if (region != null && region.IsSimplified && !Double.IsNaN(region.Area))
                {
                    return region.Area;
                }
                else
                {
                    return area;
                }
            }
            set
            {
                area = value;
            }
        }

        [DataMember(Name = "regionString", EmitDefaultValue = false)]
        [DefaultValue(null)]
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
        public int? BrushIndex { get; set; }

        // TODO: pull out to service?
        [IgnoreDataMember]
        public int? PenIndex { get; set; }

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
        
        public Spherical.Region GetRegion(bool required)
        {
            if (region != null)
            {
                return region;
            }

            int count = 0;

            if (regionString != null)
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

            if (required && region == null)
            {
                throw Error.NoRegionSpecified();
            }

            return region;
        }

        public void SetRegion(Spherical.Region value)
        {
            this.region = value;
        }

        public void GetValues(Lib.FootprintRegion region, bool required)
        {
            GetRegion(required);

            region.Name = Name ?? region.Name;
            region.FillFactor = FillFactor ?? region.FillFactor;
            region.BrushIndex = BrushIndex ?? region.BrushIndex;
            region.PenIndex = PenIndex ?? region.PenIndex;

            if (this.region != null)
            {
                region.Region = this.region;
            }
        }

        public void SetValues(Lib.Footprint footprint, Lib.FootprintRegion region)
        {
            this.Owner = footprint.Owner;
            this.FootprintName = footprint.Name;
            // this.FootprintUrl = // TODO
            this.Name = region.Name;
            this.FillFactor = region.FillFactor;
            this.BrushIndex = region.BrushIndex;
            this.PenIndex = region.PenIndex;
            this.region = region.Region;
        }
    }
}
