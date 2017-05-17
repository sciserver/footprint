using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jhu.Footprint.Web.UI.Apps.Footprint
{
    public partial class MyRegion : CustomPageBase
    {

        public static string GetUrl()
        {
            return "~/Apps/Footprint/MyRegion.aspx";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            var fName = HttpUtility.ParseQueryString(HttpContext.Current.Request.Url.Query)["FootprintName"];
            FootprintNameHeader.InnerText = fName;
            regionList.Visible = true;
            regionList.DataBind();
            }
        }
        protected void footprintRegionDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            var query = HttpUtility.ParseQueryString(Request.Url.Query);
            var search = new Lib.FootprintRegionSearch(FootprintContext);
            search.SearchType = Lib.SearchType.Region;
            search.SearchMethod = Lib.SearchMethod.Name;
            search.Owner = Page.User.Identity.Name;
            search.FootprintId = int.Parse(query["FootprintId"]);
            
            e.ObjectInstance = search;

        }

        protected void loadRegionToEditor_OnClick(object sender, EventArgs e)
        {
            LinkButton link = (LinkButton)sender;
            var footprintName = link.Attributes["footprintName"];
            var regionName = link.Attributes["regionName"];
            var regionId = link.Attributes["regionId"];
            var es = new Api.V1.EditorService();
            es.Load(Page.User.Identity.Name, footprintName, regionName);

            Response.Redirect(String.Format("Editor.aspx?footprintName={0}&regionName={1}&regionId={2}", footprintName, regionName, regionId));
        }
    }
}