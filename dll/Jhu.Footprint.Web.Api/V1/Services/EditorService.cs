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

        public void Reset()
        {
            SessionRegion = new Spherical.Region();
        }

        public void New(Stream stream)
        {
            SessionRegion = new RegionAdapter().ReadFromStream(stream);
        }

        public void Union(Stream stream)
        {
            var r = new RegionAdapter().ReadFromStream(stream);
            SessionRegion.SmartUnion(r);
        }

        public void Intersect(Stream stream)
        {
            var r = new RegionAdapter().ReadFromStream(stream);
            SessionRegion.SmartIntersect(r,false);
        }

        public void Subtract(Stream stream)
        {
            var r = new RegionAdapter().ReadFromStream(stream);
            SessionRegion.Difference(r);
            // TODO
        }

        public void Grow(double arcmin)
        {
            SessionRegion.Grow(arcmin);
        }

        public void CHull()
        {
            SessionRegion = SessionRegion.Outline.GetConvexHull();
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
            return SessionRegion;
        }

        public Spherical.Outline GetOutline(string operation)
        {
            return SessionRegion.Outline;
        }

        public IEnumerable<Lib.EquatorialPoint> GetOutlinePoints(string operation, double resolution)
        {
            return Lib.FootprintFormatter.InterpolateOutlinePoints(SessionRegion.Outline, resolution);
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
