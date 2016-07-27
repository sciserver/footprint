﻿using System;
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
        private IEnumerable<Spherical.Region> regions = null;

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

        // Do we need two of them? 
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

        public Plot()
        {
        }

        private void GetValues(Spherical.Visualizer.Plot plot)
        {

            plot.Width = Math.Max(Width ?? 1080, 1080);
            plot.Height = Math.Max( Height ?? 600, 600);

            // TODO: colorTheme


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


            if (regions != null)
            {
                switch (CoordinateSystem)
                {
                    default:
                    case "equatorial":
                        break;
                    case "galactic":
                        regions.AsParallel().ForAll(r => r.Rotate(Spherical.Rotation.EquatorialToGalactic));
                        break;
                }
            }


            // TODO: ra, dec, l, b 

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

            switch (DegreeStyle)
            {
                default:
                case "decimal":
                    grid.RaScale.DegreeFormat.DegreeStyle = Spherical.Visualizer.DegreeStyle.Decimal;
                    grid.DecScale.DegreeFormat.DegreeStyle = Spherical.Visualizer.DegreeStyle.Decimal;
                    axes.X1Axis.Scale.DegreeFormat.DegreeStyle = Spherical.Visualizer.DegreeStyle.Decimal;
                    axes.X2Axis.Scale.DegreeFormat.DegreeStyle = Spherical.Visualizer.DegreeStyle.Decimal;
                    axes.Y1Axis.Scale.DegreeFormat.DegreeStyle = Spherical.Visualizer.DegreeStyle.Decimal;
                    axes.Y2Axis.Scale.DegreeFormat.DegreeStyle = Spherical.Visualizer.DegreeStyle.Decimal;
                    break;
                case "hms":
                    grid.RaScale.DegreeFormat.DegreeStyle = Spherical.Visualizer.DegreeStyle.Hours;
                    grid.DecScale.DegreeFormat.DegreeStyle = Spherical.Visualizer.DegreeStyle.Symbols;
                    axes.X1Axis.Scale.DegreeFormat.DegreeStyle = Spherical.Visualizer.DegreeStyle.Hours;
                    axes.X2Axis.Scale.DegreeFormat.DegreeStyle = Spherical.Visualizer.DegreeStyle.Hours;
                    axes.Y1Axis.Scale.DegreeFormat.DegreeStyle = Spherical.Visualizer.DegreeStyle.Symbols;
                    axes.Y2Axis.Scale.DegreeFormat.DegreeStyle = Spherical.Visualizer.DegreeStyle.Symbols;
                    break;

            }

            // TODO: Linewidth


            // plot regions
            var regionds = new ObjectListDataSource(regions);
            
            if (RegionsVisible ?? true)
            {
                var r = new RegionsLayer();
                r.DataSource = regionds;
                r.Outline.Visible = false;
                plot.Layers.Add(r);

            }
            
            // plot outline
            if (OutlineVisible ?? true)
            {
                var r = new RegionsLayer();
                r.DataSource = regionds;
                r.Fill.Visible = false;

                var pen = new System.Drawing.Pen(System.Drawing.Brushes.Black, 1)
                {
                    LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel
                };

                r.Outline.Pens = new[] { pen };
                plot.Layers.Add(r);
            }


        }

        public Spherical.Visualizer.Plot GetPlot(IEnumerable<Spherical.Region> regions)
        {
            var plot = new Spherical.Visualizer.Plot();

            this.regions = regions;
            GetValues(plot);

            return plot;

        }
        
    }
}
