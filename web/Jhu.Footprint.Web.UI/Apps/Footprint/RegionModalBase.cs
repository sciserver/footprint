using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jhu.Footprint.Web.UI.Apps.Footprint
{
    public class RegionModalBase : ModalBase
    {
        protected RegularExpressionValidator regionNameFormatValidator;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            regionNameFormatValidator.ValidationExpression = Lib.Constants.NamePattern;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Page.ClientScript.RegisterClientScriptInclude(typeof(RegionModalBase).FullName, "RegionModalBase.js");
        }
    }
}