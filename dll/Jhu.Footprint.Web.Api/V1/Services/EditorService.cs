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


#if false
        // TODO: delete
        public void Reset()
        {
            SessionRegion = new Spherical.Region();
        }

        public void NewRegion(Stream stream)
        {
            SessionRegion = new RegionAdapter().ReadFromStream(stream);
        }

        public Spherical.Region GetRegion()
        {
            return SessionRegion;
        }
#endif




        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void New(Stream stream)
        {
            throw new NotImplementedException();
        }

        public void Union(Stream stream)
        {
            throw new NotImplementedException();
        }

        public void Intersect(Stream stream)
        {
            throw new NotImplementedException();
        }

        public void Subtract(Stream stream)
        {
            throw new NotImplementedException();
        }

        public void Grow(double arcmin)
        {
            throw new NotImplementedException();
        }

        public void CHull()
        {
            throw new NotImplementedException();
        }

        public void Load(string owner, string name, string regionName)
        {
            throw new NotImplementedException();
        }

        public void Save(string owner, string name, string regionName)
        {
            throw new NotImplementedException();
        }

        public Spherical.Region GetShape(string operation)
        {
            throw new NotImplementedException();
        }

        public Spherical.Outline GetOutline(string operation)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Lib.EquatorialPoint> GetOutlinePoints(string operation, double resolution)
        {
            throw new NotImplementedException();
        }

        public Stream PlotUserFootprintRegion(string operation, string projection, string sys, string ra, string dec, string b, string l, float width, float height, string colorTheme)
        {
            throw new NotImplementedException();
        }

        public Stream PlotUserFootprintRegionAdvanced(string operation, Plot plot)
        {
            throw new NotImplementedException();
        }
    }
}
