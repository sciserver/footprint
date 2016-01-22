using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jhu.Footprint.Web.UI
{
    public partial class Search : CustomPageBase
    {
        public static string GetUrl()
        {
            return "~/Search.aspx";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void ok_Click(object sender, EventArgs e)
        {
            footprintList.Visible = true;
            footprintList.DataBind();
        }

        protected void footprintDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            //return new Jhu.Footprint.Web.Lib.Search();
            //RegistryUser.Name

            var search = new Jhu.Footprint.Web.Lib.FootprintFolderSearch(FootprintContext)
            {
                SearchMethod = Lib.FootprintSearchMethod.Name,
                User = RegistryUser.Name,
                Name = name.Text,
            };

            e.ObjectInstance = search;

        }
    }
}