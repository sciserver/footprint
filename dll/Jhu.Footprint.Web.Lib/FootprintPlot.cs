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

            grid.RaScale.DegreeFormat.DegreeStyle = DegreeStyle.Hours;
            grid.DecScale.DegreeFormat.DegreeStyle = DegreeStyle.Symbols;
            axes.X1Axis.Scale.DegreeFormat.DegreeStyle = DegreeStyle.Hours;
            axes.X2Axis.Scale.DegreeFormat.DegreeStyle = DegreeStyle.Hours;
            axes.Y1Axis.Scale.DegreeFormat.DegreeStyle = DegreeStyle.Symbols;
            axes.Y2Axis.Scale.DegreeFormat.DegreeStyle = DegreeStyle.Symbols;

            plot.AutoRotate = true;
            plot.AutoZoom = true;

            return plot;
        }
    }
}
