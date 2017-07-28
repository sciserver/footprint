using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jhu.Footprint.Web.UI.Apps.Footprint
{
    public partial class EditorCanvas : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterClientScriptInclude(typeof(UserControl).FullName, "ControlBase.ascx.js");
            Page.ClientScript.RegisterClientScriptInclude(GetType().FullName, "EditorCanvas.ascx.js");
        }
    }
}