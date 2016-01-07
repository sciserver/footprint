using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Web.UI;

namespace Jhu.Footprint.Web.UI
{
    public class CustomUserControlBase : Jhu.Graywulf.Web.UI.UserControlBase
    {
        public new CustomPageBase Page
        {
            get { return (CustomPageBase)base.Page; }
        }
    }
}