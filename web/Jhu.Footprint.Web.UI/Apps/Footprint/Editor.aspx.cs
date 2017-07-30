using System;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Jhu.Footprint.Web.UI.Apps.Footprint
{
    public partial class Editor : CustomPageBase
    {
        public static string GetUrl()
        {
            return "~/Apps/Footprint/Editor.aspx";
        }

        public override string SelectedButton
        {
            get { return App.ButtonKeyEditor; }
        }

        #region Event handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                owner.Value = User.Identity.Name;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
        }

        #endregion
        
    }
}