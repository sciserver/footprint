using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.ComponentModel;
using Jhu.Spherical.Visualizer;

namespace Jhu.Footprint.Web.Api.V1
{
    [DataContract(Name = "plot")]
    [Description("Footprint plot prameters")]
    public class Plot
    {
        public float? Width { get; set; }

        public float? Height { get; set; }

        public string ColorTheme { get; set; }

        [Description("Projection. Valid values: TODO")]
        public string Projection { get; set; }

        [Description("Coordinate system. Valid values: eq, gal.")]
        public string CoordinateSystem { get; set; }

        public bool? AutoZoom { get; set; }

        public bool? AutoRotate { get; set; }

        public bool? GridVisible { get; set; }

        public float? GridDensity { get; set; }

        [Description("Coordinate system. Valid values: eq, gal.")]
        public string GridCoordinateSystem { get; set; }

        public bool? AxesVisible { get; set; }

        public bool? AxisLabelsVisible { get; set; }

        [Description("Degree style. Valid values: hms, decimal")]
        public string DegreeStyle { get; set; }

        public int? FontSize { get; set; }

        public int? LineWidth { get; set; }

        public bool? RegionsVisible { get; set; }

        public bool? OutlineVisible { get; set; }
    }
}
