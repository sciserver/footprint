using Jhu.Graywulf.AccessControl;
using Jhu.Graywulf.Web.UI;

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