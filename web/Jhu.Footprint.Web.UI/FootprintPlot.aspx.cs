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
            
            // in early develop phase           

            // TODO :  footprint request


            string imgUrl = "http://localhost/footprint/api/v1/Footprint.svc/users/evelin/SDSS.DR7/Stripe5/plot?";


            // Setup image url

            switch (plotDegreeStyle.SelectedValue)
            { 
                default:
                case "Decimal":
                    imgUrl += "degStyle=dms";
                    break;
                case "Sexagesimal":
                    imgUrl += "degStyle=hms";
                    break;
                case "Galactic":
                    imgUrl += "degStyle=galactic";
                    break;
            }

            switch (plotProjectionStyle.SelectedValue)
            { 
                default:
                case "Aitoff":
                    imgUrl += "&proj=Aitoff";
                    break;
                case "Equirectangular":
                    imgUrl += "&proj=Equirectangular";
                    break;
                case "HammerAitoff":
                    imgUrl += "&proj=HammerAitoff";
                    break;
                case "Mollweide":
                    imgUrl += "&proj=Mollweide";
                    break;
                case "Orthographic":
                    imgUrl += "&proj=Orthographic";
                    break;
                case "Stereographic":
                    imgUrl += "&proj=Stereographic";
                    break;
            }

            if (plotGrid.Checked) imgUrl += "&grid=true";

            if (plotAutoRotate.Checked) imgUrl += "&autoRotate=true";

            if (plotAutoZoom.Checked) imgUrl += "&autoZoom=true";


            // TODO : zoom ( slider )

            // TODO : save as : jpg, pdf stb

            PlotCanvas.ImageUrl = imgUrl;
            PlotCanvas.Width = 7 * 96;
            PlotCanvas.Height = 7 *96;

        }
        // TODO save png

        

    }
}