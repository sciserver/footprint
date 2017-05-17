using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jhu.Footprint.Web.UI.Apps.Footprint
{
    public partial class Plot : CustomPageBase
    {

        public static string GetUrl()
        {
            return "~/Apps/Footprint/Plot.aspx";
        }

        #region Event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RefreshFootprintList();
            }
            else
            {
                string controlName = Page.Request.Params["__EVENTTARGET"];
                if (!String.IsNullOrEmpty(controlName) & Page.FindControl(controlName) != FootprintSelect)
                {
                    GeneratePlot();
                }

            }

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
        }

        protected void LoadRegionButton_OnClick(object sender, EventArgs e)
        {
            GeneratePlot();
        }

        protected void FootprintSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            int j;
            if (Int32.TryParse(FootprintSelect.SelectedValue, out j))
            {
                RefreshFootprintRegionList(j);
            }
        }
        #endregion

        protected void GeneratePlot()
        {
            // in early development phase    
            // Setup image url  
            var imgUrl = "http://localhost/footprint/api/v1/Footprint.svc/users/" + Page.User.Identity.Name + "/footprints/" + FootprintSelect.SelectedItem + "/regions/" + RegionSelect.SelectedItem + "/plot?";



            switch (plotDegreeStyle.SelectedValue)
            {
                default:
                case "Decimal":
                    imgUrl += "&degStyle=decimal";
                    break;
                case "Sexagesimal":
                    imgUrl += "&degStyle=hms";
                    break;
            }

            switch (plotSystem.SelectedValue)
            {
                default:
                case "Equatorial":
                    imgUrl += "&sys=equatorial";
                    break;
                case "Galactic":
                    imgUrl += "&sys=galactic";
                    break;

            }


            imgUrl += "&proj=" + plotProjectionStyle.SelectedValue;

            imgUrl += plotGrid.Selected ? "&grid=true" : "&grid=false";

            imgUrl += plotAutoRotate.Selected ? "&rotate=true" : "&rotate=false";

            imgUrl += plotAutoZoom.Selected ? "&zoom=true" : "&zoom=false";


            // TODO : zoom ( slider )

            // TODO : save as : jpg, pdf stb

            PlotCanvas.ImageUrl = imgUrl;

        }
        // TODO save png

        private void RefreshFootprintList()
        {
            FootprintSelect.Items.Clear();
            FootprintSelect.Items.Add(new ListItem("Select item...", ""));
            FootprintSelect.Items[0].Attributes.Add("disabled", "disabled");
            FootprintSelect.Items[0].Attributes.Add("selected", "True");

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

        private void RefreshFootprintRegionList(int id)
        {
            RegionSelect.Items.Clear();
            RegionSelect.Items.Add(new ListItem("Select item...", ""));
            RegionSelect.Items[0].Attributes.Add("disabled", "disabled");
            RegionSelect.Items[0].Attributes.Add("selected", "True");


            var s = new Lib.FootprintRegionSearch(FootprintContext)
            {
                Owner = Page.User.Identity.Name,
                FootprintId = id
            };

            foreach (var r in s.Find())
            {
                var it = new ListItem()
                {
                    Text = r.Name,
                    Value = r.Id.ToString()
                };

                RegionSelect.Items.Add(it);
            }


            /*
            protected string GetImageUrl()
            {
                var baseUrl = "http://" + Environment.MachineName + "/footprint/api/v1/Footprint.svc/users/" + Page.User.Identity.Name;
                var footprintName = FootprintSelect.SelectedIndex.Equals(0) ?  "undefined" : FootprintSelect.SelectedItem.ToString();
                var regionName = (  RegionSelect.Items.Count == 0 | RegionSelect.SelectedIndex.Equals(0)) ? "undefined" : RegionSelect.SelectedItem.ToString();
                if (regionName.Equals("undefined"))
                {
                    return baseUrl + "/footprints/" + footprintName + "/plot?";
                }
                else
                {
                    return baseUrl + "/footprints/" + footprintName + "/regions/" + RegionSelect.SelectedItem + "/plot?";

                }
            }
            */
        }
    }
}