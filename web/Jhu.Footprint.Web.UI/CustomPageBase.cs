using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jhu.Graywulf.Web.Security;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Web.UI;

namespace Jhu.Footprint.Web.UI
{
    public class CustomPageBase : Jhu.Graywulf.Web.UI.PageBase
    {
        protected override void OnPreRender(EventArgs e)
        {
            Page.DataBind();

            base.OnPreRender(e);
        }
    }
}