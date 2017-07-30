using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jhu.Footprint.Web.UI.Apps.Footprint
{
    public partial class Search : CustomPageBase
    {

        public static string GetUrl()
        {
            return "~/Apps/Footprint/Search.aspx";
        }

        public override string SelectedButton
        {
            get { return App.ButtonKeySearch; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void searchForm_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                searchForm.Visible = false;

                switch (searchForm.SearchType)
                {
                    case Lib.SearchType.Footprint:
                        footprintList.Visible = true;
                        footprintList.SearchObject = (Lib.FootprintSearch)searchForm.GetSearchObject();
                        footprintList.DataBind();
                        break;
                    case Lib.SearchType.Region:
                        footprintRegionList.Visible = true;
                        footprintRegionList.SearchObject = (Lib.FootprintRegionSearch)searchForm.GetSearchObject();
                        footprintRegionList.DataBind();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }
    }
}