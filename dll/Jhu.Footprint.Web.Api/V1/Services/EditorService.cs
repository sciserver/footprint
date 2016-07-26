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

        public void New(FootprintRegionRequest request)
        {
            var region = request.Region.GetRegion();
            SessionRegion = region;
        }

        public void Union(FootprintRegionRequest request)
        {
            var region = request.Region.GetRegion();
            SessionRegion.SmartUnion(region);
            SessionRegion.Simplify();
        }

        public void Intersect(FootprintRegionRequest request)
        {
            var region = request.Region.GetRegion();
            SessionRegion.SmartIntersect(region, true);
            SessionRegion.Simplify();
        }

        public void Subtract(FootprintRegionRequest request)
        {
            var region = request.Region.GetRegion();
            SessionRegion.Difference(region);
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
            using (var context = CreateContext())
            {
                var footprint = new Lib.Footprint(context);
                footprint.Load(owner, name);

                var region = new Lib.FootprintRegion(footprint);
                region.Load(regionName);

                SessionRegion = region.Region;
            }
        }

        public void Save(string owner, string name, string regionName)
        {
            using (var context = CreateContext())
            {
                var footprint = new Lib.Footprint(context);
                if (footprint.CheckExists(owner, name))
                {
                    footprint.Load(owner, name);
                }
                else
                {
                    footprint.Owner = owner;
                    footprint.Name = name;
                    
                    footprint.CombinationMethod = Lib.CombinationMethod.Intersection;
                    footprint.Save();
                }

                var region = new Lib.FootprintRegion(footprint);
                if (region.CheckExists(regionName))
                {
                    region.Load(regionName);
                }
                else
                {
                    //region.FootprintId = footprint.Id;
                    region.Name = regionName;
                    region.Save();
                }

                // Parse region from posted data
                region.Region = SessionRegion;

                region.SaveRegion();
            }
        }

        public Spherical.Region GetShape()
        {
            return SessionRegion;
        }

        public Spherical.Outline GetOutline()
        {
            return SessionRegion.Outline;
        }

        public IEnumerable<Lib.EquatorialPoint> GetOutlinePoints(double resolution)
        {
            return Lib.FootprintFormatter.InterpolateOutlinePoints(SessionRegion.Outline, resolution);
        }

        public Spherical.Visualizer.Plot PlotUserFootprintRegion(string projection, string sys, string ra, string dec, string b, string l, float width, float height, string colorTheme)
        {
            var plot = Lib.FootprintPlot.GetDefaultPlot(new[] { SessionRegion });

            // TODO: change this part to use all parameters
            // Size is different for vector graphics!

            var plotParameters = new Plot()
            {
                Projection = projection,
                CoordinateSystem = sys,
                //Ra = ra,
                //Dec = dec
                //B = b,
                //L = l,
                Width = Math.Max(width, 1080),
                Height = Math.Max(height,600),
                ColorTheme = colorTheme,
                AutoRotate = true,
                AutoZoom = true,

            };

            return plotParameters.GetPlot(new[] { SessionRegion });
        }

        public Spherical.Visualizer.Plot PlotUserFootprintRegionAdvanced(Plot plotParameters)
        {
            //var plot = Lib.FootprintPlot.GetDefaultPlot(new[] { SessionRegion });
            
            return plotParameters.GetPlot(new[] { SessionRegion });
        }
    }
}
