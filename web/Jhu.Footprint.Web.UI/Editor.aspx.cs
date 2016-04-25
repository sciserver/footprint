using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jhu.Footprint.Web.UI
{
    public partial class Editor : System.Web.UI.Page
    {
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
            PlotCanvas.Width = 10 * 96;
            PlotCanvas.Height = 7 * 96;
        }

        public void GetPlot()
        {
            return;
        }
    }
}