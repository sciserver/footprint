using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jhu.Graywulf.Web.Services;

namespace Jhu.Footprint.Web.Api.V1
{
    public class ServiceBase : RestServiceBase
    {
        #region Context creators

        protected Lib.FootprintContext CreateContext()
        {
            return CreateContext(false);
        }

        protected Lib.FootprintContext CreateContext(bool autoDispose)
        {
            var context = new Lib.FootprintContext()
            {
                AutoDispose = autoDispose
            };

            if (System.Threading.Thread.CurrentPrincipal is Jhu.Graywulf.AccessControl.Principal)
            {
                context.Principal = (Jhu.Graywulf.AccessControl.Principal)System.Threading.Thread.CurrentPrincipal;
            }
            else
            {
                context.Principal = Jhu.Graywulf.AccessControl.Principal.Guest;
            }

            return context;
        }

        #endregion
    }
}
