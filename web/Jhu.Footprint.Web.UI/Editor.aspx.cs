using System;

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
            InitCanvas();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            GetPlot();
        }

        public void InitCanvas()
        {
            //PlotCanvas.Width = 10 * 96;
            //PlotCanvas.Height = 7 * 96;
        }

        public void GetPlot()
        {
            return;
        }
    }
}