using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.ServiceModel.Security;
using System.Security.Permissions;
using System.Web;
using System.Web.SessionState;
using Jhu.Graywulf.Web.Services;

namespace Jhu.Footprint.Web.Api.V1
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    [RestServiceBehavior]
    public class EditorService : RestServiceBase, IEditorService
    {
        private const string SessionKeyEditorRegion = "Jhu.Footprint.Web.Api.SessionRegion";

        private HttpSessionState Session
        {
            get { return HttpContext.Current.Session; }
        }

        private Spherical.Region SessionRegion
        {
            get
            {
                var region = (Spherical.Region)Session[SessionKeyEditorRegion];

                if (region == null)
                {
                    region = new Spherical.Region();
                }

                return region;
            }
            set
            {
                Session[SessionKeyEditorRegion] = value;
            }
        }

        public void New()
        {
            SessionRegion = new Spherical.Region();
        }

        public void NewRegion(Spherical.Region region)
        {
            SessionRegion = region;
        }

        public Spherical.Region GetRegion()
        {
            return SessionRegion;
        }
    }
}
