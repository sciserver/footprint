using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace Jhu.Footprint.Web.UI.Apps.Footprint
{
    public class ModalBase : UserControl
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Page.ClientScript.RegisterClientScriptInclude(typeof(UserControl).FullName, "ControlBase.js");
            Page.ClientScript.RegisterClientScriptInclude(typeof(ModalBase).FullName, "ModalBase.js");
        }
    }
}