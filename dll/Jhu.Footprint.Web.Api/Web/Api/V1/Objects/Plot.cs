using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.ComponentModel;
using Util = Jhu.Graywulf.Web.Api.Util;
using Jhu.Spherical.Visualizer;

namespace Jhu.Footprint.Web.Api.V1
{
    [DataContract]
    [Description("Footprint plot prameters")]
    public class Plot
    {
        [DataMember(Name = "width", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Width of plot. Pixels for bitmaps, inches for vectors.")]
        public float? Width { get; set; }

        [DataMember(Name = "height", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Height of plot. Pixels for bitmaps, inches for vectors.")]
        public float? Height { get; set; }

        [IgnoreDataMember]
        public ColorTheme? ColorTheme { get; set; }

        [DataMember(Name = "theme", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Color theme")]
        public string ColorTheme_ForXml
        {
            get { return Util.EnumFormatter.ToNullableXmlString(ColorTheme); }
            set { ColorTheme = Util.EnumFormatter.FromNullableXmlString<ColorTheme>(value); }
        }

        [IgnoreDataMember]
        public Projection? Projection { get; set; }

        [DataMember(Name = "proj", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Projection. Valid values: TODO")]
        public string Projection_ForXml
        {
            get { return Util.EnumFormatter.ToNullableXmlString(Projection); }
            set { Projection = Util.EnumFormatter.FromNullableXmlString<Projection>(value); }
        }

        [IgnoreDataMember]
        public CoordinateSystem? CoordinateSystem { get; set; }

        [DataMember(Name = "sys", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Coordinate system. Valid values: eq, gal.")]
        public string CoordinateSystem_ForXml
        {
            get { return Util.EnumFormatter.ToNullableXmlString(CoordinateSystem); }
            set { CoordinateSystem = Util.EnumFormatter.FromNullableXmlString<CoordinateSystem>(value); }
        }

        [DataMember(Name = "lon", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Longitude of projection origin.")]
        public double? Lon { get; set; }

        [DataMember(Name = "lat", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Latitude of projection origin.")]
        public double? Lat { get; set; }

        [DataMember(Name = "zoom", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Zoom in on region automatically.")]
        public bool? AutoZoom { get; set; }

        [DataMember(Name = "rotate", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Rotate projection origin to render region in the center.")]
        public bool? AutoRotate { get; set; }

        [DataMember(Name = "grid", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Grid lines visible.")]
        public bool? GridVisible { get; set; }

        [DataMember(Name = "gridDensity", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Grid line density.")]
        public float? GridDensity { get; set; }

        [IgnoreDataMember]
        public CoordinateSystem? GridCoordinateSystem { get; set; }

        [DataMember(Name = "gridSys", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Coordinate system. Valid values: eq, gal.")]
        public string GridCoordinateSystem_ForXml
        {
            get { return Util.EnumFormatter.ToNullableXmlString(GridCoordinateSystem); }
            set { GridCoordinateSystem = Util.EnumFormatter.FromNullableXmlString<CoordinateSystem>(value); }
        }

        [DataMember(Name = "axesVisible", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Axes visible.")]
        public bool? AxesVisible { get; set; }

        [DataMember(Name = "axisLabelsVisible", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Axis labels visible.")]
        public bool? AxisLabelsVisible { get; set; }

        [IgnoreDataMember]
        public DegreeStyle? DegreeStyle { get; set; }

        [DataMember(Name = "degreeStyle", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Degree style. Valid values: hms, decimal")]
        public string DegreeStyle_ForXml
        {
            get { return Util.EnumFormatter.ToNullableXmlString(DegreeStyle); }
            set { DegreeStyle = Util.EnumFormatter.FromNullableXmlString<DegreeStyle>(value); }
        }

        [DataMember(Name = "fontSize", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Font size in points.")]
        public int? FontSize { get; set; }

        [DataMember(Name = "lineWidth", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Line width in points.")]
        public int? LineWidth { get; set; }

        [DataMember(Name = "regionsVisible", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Plot filled regions.")]
        public bool? RegionsVisible { get; set; }

        [DataMember(Name = "outlineVisible", EmitDefaultValue = false)]
        [DefaultValue(null)]
        [Description("Plot region outline.")]
        public bool? OutlineVisible { get; set; }

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
