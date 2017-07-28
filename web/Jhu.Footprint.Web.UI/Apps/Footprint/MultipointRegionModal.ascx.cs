using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jhu.Footprint.Web.UI.Apps.Footprint
{
    public partial class MultipointRegionModal : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            multipointRegionNameFormatValidator.ValidationExpression = Lib.Constants.NamePattern;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterClientScriptInclude(typeof(UserControl).FullName, "ControlBase.js");
            Page.ClientScript.RegisterClientScriptInclude(typeof(Jhu.Graywulf.Web.UI.Controls.Form).FullName, "ModalBase.js");
            Page.ClientScript.RegisterClientScriptInclude(GetType().FullName, "MultipointRegionModal.ascx.js");
        }
    }
}