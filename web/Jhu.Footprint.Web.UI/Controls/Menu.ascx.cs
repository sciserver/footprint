using System;

namespace Jhu.Footprint.Web.UI.Controls
{
    public partial class Menu : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Home.NavigateUrl = Jhu.Footprint.Web.UI.Default.GetUrl();
            Search.NavigateUrl = Jhu.Footprint.Web.UI.Search.GetUrl();
            Edit.NavigateUrl = Jhu.Footprint.Web.UI.Editor.GetUrl();
            Plot.NavigateUrl = Jhu.Footprint.Web.UI.Plot.GetUrl();
            Docs.NavigateUrl = Jhu.Footprint.Web.UI.Docs.Default.GetUrl();
        }
    }
}