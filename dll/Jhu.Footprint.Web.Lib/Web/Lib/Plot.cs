using System;
using System.IO;
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
    public class Plot : Jhu.Spherical.Visualizer.Plot
    {
        private Jhu.Spherical.Region region;
        private string degStyle;
        private bool grid;

        public Jhu.Spherical.Region Region
        {
            get { return region; }
            set { region = value; }
        }

        public string DegStyle
        {
            get { return degStyle; }
            set { degStyle = value; }
        }

        public bool Grid
        {
            get { return grid; }
            set { grid = value; }
        }


        public Plot()
            : base()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {

            this.region = null;

            Width = 7f * 96;
            Height = 7f * 96;
            //this.projection = "Aitoff";
            Projection = new AitoffProjection();
            this.degStyle = "dms";
            this.grid = true;
            AutoZoom = false;
            AutoRotate = false;

            Margins.Left = 50f;
            Margins.Right = 50f;
            Margins.Top = 50f;
            Margins.Bottom = 50f;
        }



        public void PlotFootprint(MemoryStream stream)
        {
            Layers.Add(new BorderLayer());

            RotateRegion(degStyle);
            AppendRegionsLayer();
            if (this.grid) AppendGridLayer();

            FinishPlot(stream);

        }

        private void AppendRegionsLayer()
        {

            var rl = new RegionsLayer();
            rl.DataSource = new ObjectListDataSource(new[] { this.region });

            // fill area of the region
            rl.Outline.Visible = false;
            rl.Fill.Brushes = new Brush[] { Brushes.LightYellow };

            // draw outline of the region
            var ol = new RegionsLayer();
            ol.DataSource = new ObjectListDataSource(new[] { this.region });
            ol.Outline.Pens = new Pen[] { Pens.Red };
            ol.Fill.Visible = false;

            Layers.Add(rl);
            Layers.Add(ol);

        }

        private void AppendGridLayer()
        {
            var grid = new GridLayer();
            grid.Line.Pen = Pens.LightGray;
            grid.DecScale.Density = 150f;

            switch (this.degStyle)
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

            Layers.Add(grid);
        }

        private void RotateRegion(string transform)
        {
            switch (transform)
            {
                case "galactic":
                    this.region.Rotate(Rotation.EquatorialToGalactic);
                    break;
                default:
                    this.region.Rotate(Rotation.Zero);
                    break;

            }

        }

        private void FinishPlot(MemoryStream stream)
        {
            var font = new Font("Consolas", 7.5f);

            var axes = new AxesLayer();
            axes.X1Axis.Title.Font = font;
            axes.X1Axis.Labels.Font = font;
            axes.X2Axis.Labels.Visible = false;
            axes.Y1Axis.Title.Font = font;
            axes.Y1Axis.Labels.Font = font;
            axes.Y1Axis.Title.Text = "Declination (deg)";
            axes.Y2Axis.Labels.Visible = false;

            axes.X1Axis.Scale.DegreeFormat.DegreeStyle = DegreeStyle.Decimal;
            axes.X2Axis.Scale.DegreeFormat.DegreeStyle = DegreeStyle.Decimal;
            axes.Y1Axis.Scale.DegreeFormat.DegreeStyle = DegreeStyle.Decimal;
            axes.Y2Axis.Scale.DegreeFormat.DegreeStyle = DegreeStyle.Decimal;

            if (this.degStyle != null) this.degStyle.ToLower();

            switch (this.degStyle)
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

            Layers.Add(axes);

            Projection.InvertX = true;


            RenderToBitmap(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        /* TODO: delete
        private void plotTestPoints()
        {
            var pl = new PointsLayer();
            var points = Lib.FootprintFormatter.InterpolateOutlinePoints(this.region.Outline, 0.1);
            var galPoints = points.Select<EquatorialPoint, GalacticPoint>(x => x).ToList();
            IEnumerable<Cartesian> cartPoints = galPoints.Select(x => new Cartesian(x.L, x.B)).ToList();
            pl.DataSource = new ObjectListDataSource(cartPoints);

            pl.Outline.Visible = true;

            pl.Outline.Pens = new[] { System.Drawing.Pens.Black };

            pl.Size = new System.Drawing.SizeF(25f, 25f);
            pl.Figure = FigureType.Cross;

            Layers.Add(pl);
        }
        */
    }
}
