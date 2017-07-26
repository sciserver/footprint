using System;
using System.IO;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Linq;
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
        private const string SessionKeyEditorFootprint = "Jhu.Footprint.Web.Api.SessionFootprint";
        private const string SessionKeyEditorRegions = "Jhu.Footprint.Web.Api.SessionRegions";

        private Lib.Footprint sessionFootprint;
        private Dictionary<string, Lib.FootprintRegion> sessionRegions;

        private Lib.Footprint SessionFootprint
        {
            get { return sessionFootprint; }
            set { sessionFootprint = value; }
        }

        private Dictionary<string, Lib.FootprintRegion> SessionRegions
        {
            get { return sessionRegions; }
            set { sessionRegions = value; }
        }

        protected override void OnBeforeInvoke(RestOperationContext context)
        {
            sessionFootprint = (Lib.Footprint)context.Session[SessionKeyEditorFootprint];
            sessionRegions = (Dictionary<string, Lib.FootprintRegion>)context.Session[SessionKeyEditorRegions];

            if (sessionFootprint == null)
            {
                sessionFootprint = new Lib.Footprint()
                {
                    CombinationMethod = Lib.CombinationMethod.None,
                    Name = "new_footprint",
                    Owner = context.Principal.Identity.Name,
                };
            }

            if (sessionRegions == null)
            {
                sessionRegions = new Dictionary<string, Lib.FootprintRegion>();
            }
        }

        protected override void OnAfterInvoke(RestOperationContext context)
        {
            context.Session[SessionKeyEditorFootprint] = sessionFootprint;
            context.Session[SessionKeyEditorRegions] = sessionRegions;

            sessionFootprint = null;
            sessionRegions = null;
        }

        #region Region CRUD operations

        public FootprintResponse GetFootprint()
        {
            return new FootprintResponse(SessionFootprint);
        }

        public void DeleteFootprint()
        {
            sessionRegions.Clear();
            sessionRegions = null;
            sessionFootprint = null;
        }

        #endregion
        #region Footprint region CRUD operations

        public FootprintRegionResponse GetFootprintRegion(string regionName)
        {
            return new FootprintRegionResponse(SessionFootprint, SessionRegions[regionName]);
        }

        public FootprintRegionResponse CreateFootprintRegion(string regionName, FootprintRegionRequest request)
        {
            var region = new Lib.FootprintRegion()
            {
                Name = regionName
            };
            request.Region.GetValues(region);
            SessionRegions[regionName] = region;
            return new FootprintRegionResponse(SessionFootprint, region);
        }

        public FootprintRegionResponse ModifyFootprintRegion(string regionName, FootprintRegionRequest request)
        {
            return CreateFootprintRegion(regionName, request);
        }

        public void DeleteFootprintRegion(string regionName)
        {
            SessionRegions.Remove(regionName);
        }

        public FootprintRegionListResponse ListFootprintRegions()
        {
            return new FootprintRegionListResponse()
            {
                Regions = SessionRegions.Values
                    .Select(r => new FootprintRegion(SessionFootprint, r))
                    .ToArray()
            };
        }

        #endregion
        #region Footprint combined region get and plot

        public Spherical.Region GetFootprintShape()
        {
            throw new NotImplementedException();
        }

        public Spherical.Outline GetFootprintOutline()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Lib.EquatorialPoint> GetFootprintOutlinePoints(double resolution)
        {
            throw new NotImplementedException();
        }

        public Spherical.Visualizer.Plot PlotFootprint(
            string projection,
            string sys,
            string ra,
            string dec,
            string b,
            string l,
            float width,
            float height,
            string colorTheme,
            string autoZoom,
            string autoRotate,
            string grid,
            string degreeStyle)
        {
            var plotParameters = new Plot(projection, sys, ra, dec, b, l, width, height, colorTheme, autoZoom, autoRotate, grid, degreeStyle);
            return PlotFootprintAdvanced(plotParameters);
        }

        public Spherical.Visualizer.Plot PlotFootprintAdvanced(Plot plotParameters)
        {
            var plot = plotParameters.GetPlot(SessionRegions.Values.Select(r => r.Region));
            return plot;
        }

        public Stream GetFootprintThumbnail()
        {
            throw new NotImplementedException();
        }

        #endregion
        #region Individual region set, get and plot

        public void SetFootprintRegionShape(string regionName, Stream stream)
        {
            var region = new Lib.FootprintRegion()
            {
                Name = regionName,
                FootprintOwner = RestOperationContext.Current.Principal.Identity.Name,
                Region = new RegionAdapter().ReadFromStream(stream)
            };
            SessionRegions[regionName] = region;
        }

        public Spherical.Region GetFootprintRegionShape(string regionName)
        {
            return SessionRegions[regionName].Region;
        }

        public Spherical.Outline GetFootprintRegionOutline(string regionName)
        {
            return SessionRegions[regionName].Region.Outline;
        }

        public IEnumerable<Lib.EquatorialPoint> GetFootprintRegionOutlinePoints(string regionName, double resolution)
        {
            return Lib.FootprintFormatter.InterpolateOutlinePoints(
                SessionRegions[regionName].Region.Outline,
                resolution);
        }

        public Spherical.Visualizer.Plot PlotFootprintRegion(
            string regionName,
            string projection,
            string sys,
            string ra,
            string dec,
            string b,
            string l,
            float width,
            float height,
            string colorTheme,
            string autoZoom,
            string autoRotate,
            string grid,
            string degreeStyle)
        {
            var plotParameters = new Plot(projection, sys, ra, dec, b, l, width, height, colorTheme, autoZoom, autoRotate, grid, degreeStyle);
            return PlotFootprintRegionAdvanced(regionName, plotParameters);
        }

        public Spherical.Visualizer.Plot PlotFootprintRegionAdvanced(string regionName, Plot plotParameters)
        {
            return plotParameters.GetPlot(new[] { SessionRegions[regionName].Region });
        }

        public Stream GetFootprintRegionThumbnail(string regionName)
        {
            throw new NotImplementedException();
        }

        // TODO: add HTM cover

        #endregion

        /*

        [OperationContract]
        [WebGet(UriTemplate = "/load?owner={owner}&name={name}&region={regionName}")]
        [Description("Load a region from the database")]
        void Load(string owner, string name, string regionName);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = "/save?owner={owner}&name={name}&region={regionName}&method={combinationMethod}")]
        [Description("Save a region to the database")]
        void Save(string owner, string name, string regionName, string combinationMethod);

        */

#if false
        public void Reset()
        {
            SessionRegion = new Spherical.Region();
        }

        public void New(FootprintRegionRequest request)
        {
            SessionRegion = request.Region.GetRegion();
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

        public void Save(string owner, string name, string regionName, string combinationMethod)
        {

            Lib.CombinationMethod method;
            switch (combinationMethod)
            {
                case "intersect":
                    method = Lib.CombinationMethod.Intersection;
                    break;
                case "union":
                    method = Lib.CombinationMethod.Union;
                    break;
                default:
                    method = Lib.CombinationMethod.None;
                    break;
            }

            using (var context = CreateContext())
            {
                var footprint = new Lib.Footprint(context);
                if (footprint.CheckExists(owner, name))
                {
                    footprint.Load(owner, name);
                    footprint.CombinationMethod = method;
                    footprint.Save();
                }
                else
                {
                    footprint.Owner = owner;
                    footprint.Name = name;

                    footprint.CombinationMethod = method;
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

        public Spherical.Visualizer.Plot PlotUserFootprintRegion(string projection, string sys, string ra, string dec, string b, string l, float width, float height, string colorTheme, string autoZoom, string autoRotate, string grid, string degreeStyle)
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
                Height = Math.Max(height, 600),
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
#endif
    }
}
