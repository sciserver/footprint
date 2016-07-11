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

namespace Jhu.Footprint.Web.UI
{
    public partial class Plot : CustomPageBase
    {

        public static string GetUrl()
        {
            return "~/Plot.aspx";
        }

        #region Event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RefreshFootprintList();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
        }

        protected void LoadRegionButton_OnClick(object sender, EventArgs e)
        {
            GeneratePlot();
        }
        #endregion

        //protected string GetPlotBaseUrl()
        //{
        //    // TODO: Import owners with javascript as well
        //    var selected = FootprintSelect.Value.Split('/');
        //    var owner = "undefined";
        //    var footprint = "undefined";
        //    if (selected.Length == 2)
        //    {
        //    owner = selected[0] ;
        //    footprint = selected[1] ;
        //    }
        //    var region = RegionSelect.Value;
            
        //    return "http://localhost/footprint/api/v1/Footprint.svc/users/" + owner + "/footprints/"+footprint+"/regions/"+region+"/plot?";
        //}

        protected void GeneratePlot()
        {
            // in early development phase    
            // Setup image url  
            var imgUrl = "http://localhost/footprint/api/v1/Footprint.svc/users/"+Page.User.Identity.Name+"/footprints/"+FootprintSelect.SelectedItem+"/Plot?";
     


            switch (plotDegreeStyle.SelectedValue)
            { 
                default:
                case "Decimal":
                    imgUrl += "sys=dms";
                    break;
                case "Sexagesimal":
                    imgUrl += "sys=hms";
                    break;
                case "Galactic":
                    imgUrl += "sys=galactic";
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

        }
        // TODO save png

        private void RefreshFootprintList()
        {
            FootprintSelect.Items.Clear();
            FootprintSelect.Items.Add(new ListItem("(select item)", ""));

            var s = new Lib.FootprintSearch(FootprintContext)
            {
                Owner = Page.User.Identity.Name
            };

            foreach (var f in s.Find())
            {
                var it = new ListItem()
                {
                    Text = f.Name,
                    Value = f.Id.ToString()
                };

                FootprintSelect.Items.Add(it);
            }
        }       

    }
}