using System;
using Jhu.Spherical.Visualizer;

namespace Jhu.Footprint.Web.UI
{
    public partial class Editor : System.Web.UI.Page
    {
        public static string GetUrl()
        {
            return "~/Editor.aspx";
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            InitCanvas();
        }

        public void InitCanvas()
        {
            //PlotCanvas.Plot.Projection = new OrthographicProjection();
            //PlotCanvas.Width = 10 * 96;
            //PlotCanvas.Height = 7 * 96;
        }

        public void GetPlot()
        {
            return;
        }
    }
}