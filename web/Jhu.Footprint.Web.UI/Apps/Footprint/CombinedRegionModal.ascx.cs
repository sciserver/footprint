using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jhu.Footprint.Web.UI.Apps.Footprint
{
    public partial class CombinedRegionModal : RegionModalBase
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Page.ClientScript.RegisterClientScriptInclude(typeof(CombinedRegionModal).FullName, "CombinedRegionModal.ascx.js");
        }
        
    }
}