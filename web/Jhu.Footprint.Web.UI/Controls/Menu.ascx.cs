using System;

namespace Jhu.Footprint.Web.UI.Controls
{
    public partial class Menu : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Home.NavigateUrl = Jhu.Footprint.Web.UI.Default.GetUrl();
            Search.NavigateUrl = Jhu.Footprint.Web.UI.Search.GetUrl();
            Docs.NavigateUrl = Jhu.Footprint.Web.UI.Docs.Default.GetUrl();
        }
    }
}