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
        [DataMember(Name = "width")]
        public float? Width { get; set; }

        [DataMember(Name = "height")]
        public float? Height { get; set; }

        [DataMember(Name = "theme")]
        public string ColorTheme { get; set; }

        [DataMember(Name = "proj")]
        [Description("Projection. Valid values: TODO")]
        public string Projection { get; set; }

        [DataMember(Name = "sys")]
        [Description("Coordinate system. Valid values: eq, gal.")]
        public string CoordinateSystem { get; set; }

        [DataMember(Name = "ra")]
        public double Ra { get; set; }

        [DataMember(Name = "dec")]
        public double Dec { get; set; }

        [DataMember(Name = "b")]
        public double B { get; set; }

        [DataMember(Name = "l")]
        public double L { get; set; }

        [DataMember(Name = "zoom")]
        public bool? AutoZoom { get; set; }

        [DataMember(Name = "rotate")]
        public bool? AutoRotate { get; set; }

        [DataMember(Name = "gridVisible")]
        public bool? GridVisible { get; set; }

        [DataMember(Name = "gridDensity")]
        public float? GridDensity { get; set; }

        [DataMember(Name = "gridSys")]
        [Description("Coordinate system. Valid values: eq, gal.")]
        public string GridCoordinateSystem { get; set; }

        [DataMember(Name = "axesVisible")]
        public bool? AxesVisible { get; set; }

        [DataMember(Name = "axisLabelsVisible")]
        public bool? AxisLabelsVisible { get; set; }

        [DataMember(Name = "degreeStyle")]
        [Description("Degree style. Valid values: hms, decimal")]
        public string DegreeStyle { get; set; }

        [DataMember(Name = "fontSize")]
        public int? FontSize { get; set; }

        [DataMember(Name = "lineWidth")]
        public int? LineWidth { get; set; }

        [DataMember(Name = "regionsVisible")]
        public bool? RegionsVisible { get; set; }

        [DataMember(Name = "outlineVisible")]
        public bool? OutlineVisible { get; set; }

        public void GetValues(Spherical.Visualizer.Plot plot)
        {
            plot.Width = Width ?? plot.Width;
            plot.Height = Height ?? plot.Height;

            plot.AutoRotate = AutoRotate ?? true;
            plot.AutoZoom = AutoZoom ?? false;


            // projection
            if (Projection != "")
            {
                try
                {
                    Projection = "Jhu.Spherical.Visualizer." + Projection + "Projection,Jhu.Spherical.Visualizer";
                    var t = Type.GetType(Projection);
                    plot.Projection = (Projection)Activator.CreateInstance(t);
                }
                catch (Exception e)
                {
                    plot.Projection = new AitoffProjection();
                }
            }
        }

        public Spherical.Visualizer.Plot GetPlot(IEnumerable<Spherical.Region> regions)
        {
            var plot = new Spherical.Visualizer.Plot();
            var regionds = new ObjectListDataSource(regions);

            // Plot regions
            if (RegionsVisible ?? true)
            {
                var r1 = new RegionsLayer();
                r1.DataSource = regionds;
                r1.Outline.Visible = false;
                plot.Layers.Add(r1);
            }         


            // plot outline
            if (OutlineVisible ?? true)
            {
            var r2 = new RegionsLayer();
            r2.DataSource = regionds;
            r2.Fill.Visible = false;
            }

            return plot;
        }

        private void SetValues()
        { }
        
    }
}
