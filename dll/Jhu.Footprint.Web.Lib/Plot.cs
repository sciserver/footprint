using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Spherical;
using Jhu.Spherical.Visualizer;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data.SqlClient;

namespace Jhu.Footprint.Web.Lib
{
    public class Plot
    {
        public Plot()
        {
        }


        public void PlotFootprint(Jhu.Spherical.Region region, float w, float h, string projection, string degStyle)
        {
            var plot = InitPlot(w, h, projection);

            AppendRegionsLayer(plot, region);
            AppendGridLayer(plot, degStyle);
            FinishPlot(plot, degStyle);
        }

        private Jhu.Spherical.Visualizer.Plot InitPlot(float w, float h, string projection)
        {
            if (w == 0f) w = 7f; // setup default value
            if (h == 0f) h = 7f; // setup default value

            w = (float)(w * 96);
            h = (float)(h * 96);

            Projection p;

            try
            {
                projection = "Jhu.Spherical.Visualizer." + projection + "Projection,Jhu.Spherical.Visualizer";
                var t = Type.GetType(projection);
                p = (Jhu.Spherical.Visualizer.Projection)Activator.CreateInstance(t);
            }
            catch (Exception e)
            {
                // TODO: throw exception ? or just set a default ?
                p = new HammerAitoffProjection();
            }

            var plot = new Jhu.Spherical.Visualizer.Plot()
            {
                AutoScale = true,
                Width = w,
                Height = h,
                ImageSize = new System.Drawing.SizeF(w, h),
                Projection = p
            };

            if (p.GetType().Name == "EquirectangularProjection" | p.GetType().Name == "StereographicProjection" | p.GetType().Name == "OrthographicProjection")
            {
                plot.AutoZoom = true;
                plot.AutoRotate = true;
            }

            plot.Margins.Left = 50f;
            plot.Margins.Right = 50f;
            plot.Margins.Top = 50f;
            plot.Margins.Bottom = 50f;

            plot.Layers.Add(new BorderLayer());

            return plot;
        }

        private void AppendRegionsLayer(Spherical.Visualizer.Plot plot, Spherical.Region region)
        {

            var rl = new RegionsLayer();
            rl.DataSource = new ObjectListDataSource(new[] { region });

            // fill area of the region
            rl.Outline.Visible = false;
            rl.Fill.Brushes = new Brush[] { Brushes.LightYellow };

            // draw outline of the region
            var ol = new RegionsLayer();
            ol.DataSource = new ObjectListDataSource(new[] { region });
            ol.Outline.Pens = new Pen[] { Pens.Red };
            ol.Fill.Visible = false;

            plot.Layers.Add(rl);
            plot.Layers.Add(ol);

        }

        static void AppendGridLayer(Jhu.Spherical.Visualizer.Plot plot, string degStyle)
        {
            var grid = new GridLayer();
            grid.Line.Pen = Pens.LightGray;
            grid.DecScale.Density = 150f;

            switch (degStyle)
            {
                default:
                case "dms":
                    grid.RaScale.DegreeFormat.DegreeStyle = DegreeStyle.Decimal;
                    grid.DecScale.DegreeFormat.DegreeStyle = DegreeStyle.Decimal;
                    break;
                case "hms":
                    grid.RaScale.DegreeFormat.DegreeStyle = DegreeStyle.Hours;
                    grid.DecScale.DegreeFormat.DegreeStyle = DegreeStyle.Symbols;
                    break;
            }

            plot.Layers.Add(grid);
        }

        private void FinishPlot(Spherical.Visualizer.Plot plot, string degStyle)
        {
            // TODO: Axis labels not showin up...
            var font = new Font("Consolas", 7.5f);

            var axes = new AxesLayer();
            axes.X1Axis.Title.Font = font;
            axes.X1Axis.Labels.Font = font;
            axes.X2Axis.Labels.Visible = false;
            axes.Y1Axis.Title.Font = font;
            axes.Y1Axis.Labels.Font = font;
            axes.Y1Axis.Title.Text = "Declination (deg)";
            axes.Y2Axis.Labels.Visible = false;

            switch (degStyle.ToLower())
            {
                default:
                case "dms":
                    axes.X1Axis.Title.Text = "Right ascension (deg)";
                    axes.X1Axis.Scale.DegreeFormat.DegreeStyle = DegreeStyle.Decimal;
                    axes.X2Axis.Scale.DegreeFormat.DegreeStyle = DegreeStyle.Decimal;
                    axes.Y1Axis.Scale.DegreeFormat.DegreeStyle = DegreeStyle.Decimal;
                    axes.Y2Axis.Scale.DegreeFormat.DegreeStyle = DegreeStyle.Decimal;
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


            plot.Projection.InvertX = true;

            var fileName = String.Format("C:\\Data\\ebanyai\\0.jpg");
            plot.RenderToBitmap(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
        }
    }
}
