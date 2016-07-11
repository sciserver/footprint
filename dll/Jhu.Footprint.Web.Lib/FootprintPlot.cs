using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Jhu.Spherical.Visualizer;

namespace Jhu.Footprint.Web.Lib
{
    public class FootprintPlot
    {
        public static Spherical.Visualizer.Plot GetDefaultPlot(IEnumerable<Spherical.Region> regions)
        {
            return GetDefaultPlot(regions, null);
        }
        public static Spherical.Visualizer.Plot GetDefaultPlot(IEnumerable<Spherical.Region> regions, string sys)
        {
            var plot = new Spherical.Visualizer.Plot();

            var regionds = new ObjectListDataSource(regions);

            // Create plot
            plot.Projection = new OrthographicProjection();
            plot.Projection.InvertX = true;

            // plot grid
            var grid = new GridLayer();
            grid.RaScale.Density = 100;
            grid.DecScale.Density = 100;

            plot.Layers.Add(grid);
            plot.Layers.Add(new BorderLayer());

            // Plot regions
            var r1 = new RegionsLayer();
            r1.DataSource = regionds;
            r1.Outline.Visible = false;
            plot.Layers.Add(r1);


            // plot outline
            var r2 = new RegionsLayer();
            r2.DataSource = regionds;
            r2.Fill.Visible = false;

            var pen = new Pen(Brushes.Black, 1)
            {
                LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel
            };

            r2.Outline.Pens = new[] { pen };
            plot.Layers.Add(r2);

            var axes = new AxesLayer();
            plot.Layers.Add(axes);

            grid.RaScale.DegreeFormat.DegreeStyle = DegreeStyle.Decimal;
            grid.DecScale.DegreeFormat.DegreeStyle = DegreeStyle.Decimal;
            axes.X1Axis.Scale.DegreeFormat.DegreeStyle = DegreeStyle.Decimal;
            axes.X2Axis.Scale.DegreeFormat.DegreeStyle = DegreeStyle.Decimal;
            axes.Y1Axis.Scale.DegreeFormat.DegreeStyle = DegreeStyle.Decimal;
            axes.Y2Axis.Scale.DegreeFormat.DegreeStyle = DegreeStyle.Decimal;

            switch (sys)
            {
                default:
                case "dms":
                    axes.X1Axis.Title.Text = "Right ascension (deg)";
                    break;
                case "galactic":
                    axes.X1Axis.Title.Text = "Galactic longitude (deg)";
                    axes.Y1Axis.Title.Text = "Galactic latitude (deg)";
                    break;
                case "hms":
                    axes.X1Axis.Title.Text = "Right ascension (hour)";
                    grid.RaScale.DegreeFormat.DegreeStyle = DegreeStyle.Hours;
                    grid.DecScale.DegreeFormat.DegreeStyle = DegreeStyle.Symbols;
                    axes.X1Axis.Scale.DegreeFormat.DegreeStyle = DegreeStyle.Hours;
                    axes.X2Axis.Scale.DegreeFormat.DegreeStyle = DegreeStyle.Hours;
                    axes.Y1Axis.Scale.DegreeFormat.DegreeStyle = DegreeStyle.Symbols;
                    axes.Y2Axis.Scale.DegreeFormat.DegreeStyle = DegreeStyle.Symbols;
                    break;

            }
            

            plot.AutoRotate = true;
            plot.AutoZoom = true;

            return plot;


        }

        public static Spherical.Visualizer.Plot GetPlot(IEnumerable<Spherical.Region> regions, string projection, string sys, string ra, string dec, string b, string l, float width, float height, string colorTheme)
        {
            var plot = GetDefaultPlot(regions, sys);

            if (width > 0) plot.Width = width * 96;
            if (height > 0) plot.Height = height * 96;

            if (projection != "")
            {
                try
                {
                    projection = "Jhu.Spherical.Visualizer." + projection + "Projection,Jhu.Spherical.Visualizer";
                    var t = Type.GetType(projection);
                    plot.Projection = (Jhu.Spherical.Visualizer.Projection)Activator.CreateInstance(t);
                }
                catch (Exception e)
                {
                    //plot.Projection = new Jhu.Spherical.Visualizer.AitoffProjection();
                }
            }
            
            /*
            var axes = new AxesLayer();
            plot.Layers.RemoveAt(plot.Layers.Count);

            switch (sys)
            {
                default:
                case "dms":
                    axes.X1Axis.Title.Text = "Right ascension (deg)";
                    break;
                case "galactic":
                    axes.X1Axis.Title.Text = "Galactic longitude (deg)";
                    axes.Y1Axis.Title.Text = "Galactic latitude (deg)";
                    break;
                case "hms":
                    axes.X1Axis.Title.Text = "Right ascension (hour)";
                    axes.X1Axis.Scale.DegreeFormat.DegreeStyle = DegreeStyle.Hours;
                    axes.X2Axis.Scale.DegreeFormat.DegreeStyle = DegreeStyle.Hours;
                    axes.Y1Axis.Scale.DegreeFormat.DegreeStyle = DegreeStyle.Symbols;
                    axes.Y2Axis.Scale.DegreeFormat.DegreeStyle = DegreeStyle.Symbols;
                    break;

            }

            plot.Layers.Add(axes);
            */
            return plot;
        }
    }
}
