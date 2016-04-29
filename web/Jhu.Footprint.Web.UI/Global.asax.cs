using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Routing;
using Jhu.Graywulf.AccessControl;
using Jhu.Graywulf.Web;
using Jhu.Graywulf.Web.UI;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.Install;

namespace Jhu.Footprint.Web.UI
{
    public class Global : FederationApplicationBase
    {
        protected override void OnUserArrived(GraywulfPrincipal principal)
        {
            base.OnUserArrived(principal);
        }

        protected override void OnUserLeft(GraywulfPrincipal principal)
        {
            base.OnUserLeft(principal);
        }
    }
}