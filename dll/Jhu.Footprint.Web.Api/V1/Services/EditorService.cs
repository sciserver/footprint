using System;
using System.IO;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.SessionState;
using Jhu.Graywulf.Web.Services;

namespace Jhu.Footprint.Web.Api.V1
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    [RestServiceBehavior]
    public class EditorService : ServiceBase, IEditorService
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
            SessionRegion.Simplify();
        }

        public void Union(Stream stream)
        {
            var r = new RegionAdapter().ReadFromStream(stream);
            r.Simplify();
            SessionRegion.SmartUnion(r);
            SessionRegion.Simplify();
        }

        public void Intersect(Stream stream)
        {
            var r = new RegionAdapter().ReadFromStream(stream);
            r.Simplify();
            SessionRegion.SmartIntersect(r,true);
            SessionRegion.Simplify();
        }

        public void Subtract(Stream stream)
        {
            var r = new RegionAdapter().ReadFromStream(stream);
            r.Simplify();
            SessionRegion.Difference(r);
            SessionRegion.Simplify();
        }

        public void Grow(double arcmin)
        {
            SessionRegion.Grow(arcmin);
            SessionRegion.Simplify();
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
            using (var context = CreateContext())
            {
                var footprint = new Lib.Footprint(context);
                footprint.Load(owner, name);

                var region = new Lib.FootprintRegion(footprint);
                region.Load(regionName);

                // Parse region from posted data
                region.Region = SessionRegion;

                region.SaveRegion();
            }
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

        public Spherical.Visualizer.Plot PlotUserFootprintRegion(string operation, string projection, string sys, string ra, string dec, string b, string l, float width, float height, string colorTheme)
        {
            var plot = Lib.FootprintPlot.GetDefaultPlot(new[] { SessionRegion });

            // TODO: change this part to use all parameters
            // Size is different for vector graphics!
            plot.Width = Math.Max(width, 640);
            plot.Height = Math.Max(height, 480);

            return plot;
        }

        public Spherical.Visualizer.Plot PlotUserFootprintRegionAdvanced(string operation, Plot plotParameters)
        {
            var plot = Lib.FootprintPlot.GetDefaultPlot(new [] { SessionRegion });

            plotParameters.GetValues(plot);

            return plot;
        }
    }
}
