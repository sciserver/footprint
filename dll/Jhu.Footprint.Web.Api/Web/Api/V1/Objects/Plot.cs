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
        public ColorTheme? ColorTheme { get; set; }

        [DataMember(Name = "proj")]
        [Description("Projection. Valid values: TODO")]
        public Projection? Projection { get; set; }

        [DataMember(Name = "sys")]
        [Description("Coordinate system. Valid values: eq, gal.")]
        public CoordinateSystem? CoordinateSystem { get; set; }

        [DataMember(Name = "lon")]
        public double? Lon { get; set; }

        [DataMember(Name = "lat")]
        public double? Lat { get; set; }

        [DataMember(Name = "zoom")]
        public bool? AutoZoom { get; set; }

        [DataMember(Name = "rotate")]
        public bool? AutoRotate { get; set; }

        [DataMember(Name = "grid")]
        public bool? GridVisible { get; set; }

        [DataMember(Name = "gridDensity")]
        public float? GridDensity { get; set; }

        [DataMember(Name = "gridSys")]
        [Description("Coordinate system. Valid values: eq, gal.")]
        public CoordinateSystem? GridCoordinateSystem { get; set; }

        [DataMember(Name = "axesVisible")]
        public bool? AxesVisible { get; set; }

        [DataMember(Name = "axisLabelsVisible")]
        public bool? AxisLabelsVisible { get; set; }

        [DataMember(Name = "degreeStyle")]
        [Description("Degree style. Valid values: hms, decimal")]
        public DegreeStyle? DegreeStyle { get; set; }

        [DataMember(Name = "fontSize")]
        public int? FontSize { get; set; }

        [DataMember(Name = "lineWidth")]
        public int? LineWidth { get; set; }

        [DataMember(Name = "regionsVisible")]
        public bool? RegionsVisible { get; set; }

        [DataMember(Name = "outlineVisible")]
        public bool? OutlineVisible { get; set; }

        [DataMember(Name = "highlights")]
        public string[] Highligths { get; set; }

        public Plot()
        {
        }

        private void GetValues(Spherical.Visualizer.Plot plot, IEnumerable<Lib.FootprintRegion> regions)
        {
            plot.Width = Math.Max(Width ?? 1080, 1080);
            plot.Height = Math.Max(Height ?? 600, 600);

            // TODO: colorTheme

            switch (Projection ?? V1.Projection.Stereo)
            {
                case V1.Projection.Stereo:
                    plot.Projection = new StereographicProjection();
                    break;
                case V1.Projection.Ortho:
                    plot.Projection = new OrthographicProjection();
                    break;
                case V1.Projection.Aitoff:
                    plot.Projection = new AitoffProjection();
                    break;
                case V1.Projection.Hammer:
                    plot.Projection = new HammerAitoffProjection();
                    break;
                case V1.Projection.Mollweide:
                    plot.Projection = new MollweideProjection();
                    break;
                default:
                    throw new NotImplementedException();
            }

            /* TODO: push this down to plotting lib
            switch (CoordinateSystem ?? V1.CoordinateSystem.EqJ2000)
            {
                case V1.CoordinateSystem.EqJ2000:
                    break;
                case V1.CoordinateSystem.GalJ2000:
                    // TODO: this is a bit fishy here, we should probably only tag a rotation matrix to the region
                    regions?.AsParallel().ForAll(r => r.GetRegion(true).Rotate(Spherical.Rotation.EquatorialToGalactic));
                    break;
                default:
                    throw new NotImplementedException();
            }
            */

            // TODO: ra, dec, l, b 
            // plot.Projection.Rotation

            plot.AutoRotate = AutoRotate ?? true;
            plot.AutoZoom = AutoZoom ?? false;

            // plot grid
            var grid = new GridLayer();

            if (GridVisible ?? true)
            {
                grid.RaScale.Density = GridDensity ?? 100;
                grid.DecScale.Density = GridDensity ?? 100;
                plot.Layers.Add(grid);
                plot.Layers.Add(new BorderLayer());
            }

            var axes = new AxesLayer();

            if (AxesVisible ?? true)
            {
                axes.X1Axis.Labels.Visible = AxisLabelsVisible ?? true;
                axes.X2Axis.Labels.Visible = AxisLabelsVisible ?? true;
                axes.Y1Axis.Labels.Visible = AxisLabelsVisible ?? true;
                axes.Y2Axis.Labels.Visible = AxisLabelsVisible ?? true;

                // TODO: Fontsize

                plot.Layers.Add(axes);
            }

            // DegreeStyle

            switch (DegreeStyle ?? V1.DegreeStyle.Dec)
            {
                case V1.DegreeStyle.Dec:
                    grid.RaScale.DegreeFormat.DegreeStyle = Spherical.Visualizer.DegreeStyle.Decimal;
                    grid.DecScale.DegreeFormat.DegreeStyle = Spherical.Visualizer.DegreeStyle.Decimal;
                    axes.X1Axis.Scale.DegreeFormat.DegreeStyle = Spherical.Visualizer.DegreeStyle.Decimal;
                    axes.X2Axis.Scale.DegreeFormat.DegreeStyle = Spherical.Visualizer.DegreeStyle.Decimal;
                    axes.Y1Axis.Scale.DegreeFormat.DegreeStyle = Spherical.Visualizer.DegreeStyle.Decimal;
                    axes.Y2Axis.Scale.DegreeFormat.DegreeStyle = Spherical.Visualizer.DegreeStyle.Decimal;
                    break;
                case V1.DegreeStyle.Sexa:
                    grid.RaScale.DegreeFormat.DegreeStyle = Spherical.Visualizer.DegreeStyle.Hours;
                    grid.DecScale.DegreeFormat.DegreeStyle = Spherical.Visualizer.DegreeStyle.Symbols;
                    axes.X1Axis.Scale.DegreeFormat.DegreeStyle = Spherical.Visualizer.DegreeStyle.Hours;
                    axes.X2Axis.Scale.DegreeFormat.DegreeStyle = Spherical.Visualizer.DegreeStyle.Hours;
                    axes.Y1Axis.Scale.DegreeFormat.DegreeStyle = Spherical.Visualizer.DegreeStyle.Symbols;
                    axes.Y2Axis.Scale.DegreeFormat.DegreeStyle = Spherical.Visualizer.DegreeStyle.Symbols;
                    break;
                default:
                    throw new NotImplementedException();
            }

            // TODO: Linewidth

            var regionds = new ObjectListDataSource(regions);

            // Plot regions
            if (RegionsVisible ?? true)
            {                
                var r = new RegionsLayer();
                r.DataSource = regionds;
                r.RegionDataField = "Region";
                r.Outline.Visible = false;
                r.Fill.PaletteSelection = PaletteSelection.Field;
                r.Fill.Field = "BrushIndex";
                plot.Layers.Add(r);
            }

            // plot outline
            if (OutlineVisible ?? true)
            {
                var r = new RegionsLayer();
                r.DataSource = regionds;
                r.RegionDataField = "Region";
                r.Fill.Visible = false;
                r.Outline.PaletteSelection = PaletteSelection.Field;
                r.Outline.Field = "PenIndex";
                plot.Layers.Add(r);
            }
        }

        public Spherical.Visualizer.Plot GetPlot(IEnumerable<Lib.FootprintRegion> regions)
        {
            var plot = new Spherical.Visualizer.Plot();

            GetValues(plot, regions);

            return plot;

        }

    }
}
