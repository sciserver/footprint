using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jhu.Footprint.Web.UI.Apps.Footprint
{
    public partial class MyFootprint : CustomPageBase
    {

        public static string GetUrl()
        {
            return "~/Apps/Footprint/MyFootprint.aspx";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
            regionList.Visible = true;
            regionList.DataBind();
            }
        }
        protected void footprintRegionDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            var search = new Lib.FootprintRegionSearch(FootprintContext);
            search.SearchType = Lib.SearchType.Footprint;
            search.SearchMethod = Lib.SearchMethod.Name;
            search.Owner = Page.User.Identity.Name;

            e.ObjectInstance = search;

        }

        protected void loadMyRegion(object sender, EventArgs e)
        {
            LinkButton link = (LinkButton)sender;
            var footprintName = link.Attributes["footprintName"];
            var footprintId = link.Attributes["footprintId"];

            Response.Redirect(String.Format("MyRegion.aspx?footprintId={0}&footprintName={1}",footprintId, footprintName));
        }
    }
}