using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jhu.Footprint.Web.UI
{
    public partial class MyFootprint : CustomPageBase
    {

        public static string GetUrl()
        {
            return "~/MyFootprint.aspx";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            regionList.Visible = true;
            regionList.DataBind();
        }
        protected void footprintRegionDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            var search = new Lib.FootprintRegionSearch(FootprintContext);
            search.SearchType = Lib.SearchType.Footprint;
            search.SearchMethod = Lib.SearchMethod.Name;
            search.Owner = Page.User.Identity.Name;

            e.ObjectInstance = search;

        }
    }
}