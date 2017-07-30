using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jhu.Footprint.Web.UI.Apps.Footprint
{
    public partial class SaveFootprintModal : ModalBase
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            footprintNameFormatValidator.ValidationExpression = Lib.Constants.NamePattern;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Page.ClientScript.RegisterClientScriptInclude(typeof(SaveFootprintModal).FullName, "SaveFootprintModal.ascx.js");
        }
    }
}