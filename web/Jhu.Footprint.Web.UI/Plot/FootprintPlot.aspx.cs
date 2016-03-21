using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using Jhu.Spherical;
using Jhu.Spherical.Visualizer;
using Jhu.Spherical.Web.Controls;

namespace Jhu.Footprint.Web.UI.Plot
{
    public partial class FootprintPlot : System.Web.UI.Page
    {
        #region Event handlers
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            GeneratePlot();
        }



        #endregion

        protected void GeneratePlot()
        {
            // TODO : minimalis canvas + grid + footprint


            // Create plot
            canvas.Plot.Projection = new OrthographicProjection();
            canvas.Plot.Projection.InvertX = true;

            var grid = new GridLayer();
            grid.RaScale.Density = 100;
            grid.DecScale.Density = 100;

            //if (plotGrid.Checked)
            //{
            canvas.Plot.Layers.Add(grid);
            canvas.Plot.Layers.Add(new BorderLayer());
            //}


            var axes = new AxesLayer();
            canvas.Plot.Layers.Add(axes);
        }

        //TODO save png

    }
}