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

        private Footprint sessionFootprint;
        private Dictionary<string, FootprintRegion> sessionRegions;

        private Footprint SessionFootprint
        {
            get { return sessionFootprint; }
            set { sessionFootprint = value; }
        }

        private Dictionary<string, FootprintRegion> SessionRegions
        {
            get { return sessionRegions; }
            set { sessionRegions = value; }
        }

        protected override void OnBeforeInvoke(RestOperationContext context)
        {
            sessionFootprint = (Footprint)context.Session[SessionKeyEditorFootprint];
            sessionRegions = (Dictionary<string, FootprintRegion>)context.Session[SessionKeyEditorRegions];

            if (sessionFootprint == null)
            {
                sessionFootprint = new Footprint()
                {
                    CombinationMethod = Lib.CombinationMethod.None,
                    Name = "new_footprint",
                    Owner = context.Principal.Identity.Name,
                };
            }

            if (sessionRegions == null)
            {
                sessionRegions = new Dictionary<string, FootprintRegion>();
            }
        }

        protected override void OnAfterInvoke(RestOperationContext context)
        {
            context.Session[SessionKeyEditorFootprint] = sessionFootprint;
            context.Session[SessionKeyEditorRegions] = sessionRegions;

            sessionFootprint = null;
            sessionRegions = null;
        }

        private void ValidateRegion(FootprintRegion region)
        {
            // This simply fails if incorrect region string is uploaded
            region.Region.Simplify();
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

        public FootprintRegionResponse CreateFootprintRegion(string regionName, FootprintRegionRequest request)
        {
            if (!Lib.Constants.NamePatternRegex.Match(regionName).Success)
            {
                throw Lib.Error.InvalidName(regionName);
            }

            if (SessionRegions.ContainsKey(regionName))
            {
                throw Lib.Error.DuplicateRegionName("editor", "editor", regionName);
            }

            ValidateRegion(request.Region);
            SessionRegions[regionName] = request.Region;
            return new FootprintRegionResponse(request.Region);
        }

        public FootprintRegionResponse ModifyFootprintRegion(string regionName, FootprintRegionRequest request)
        {
            if (!SessionRegions.ContainsKey(regionName))
            {
                throw Lib.Error.RegionNotFound("editor", "editor", regionName);
            }

            ValidateRegion(request.Region);
            SessionRegions[regionName] = request.Region;
            return new FootprintRegionResponse(request.Region);
        }

        public void DeleteFootprintRegion(string regionName)
        {
            SessionRegions.Remove(regionName);
        }

        public void DeleteFootprintRegions(string[] regionNames)
        {
            foreach (var item in regionNames)
            {
                if (SessionRegions.ContainsKey(item))
                {
                    SessionRegions.Remove(item);
                }
            }
        }

        public FootprintRegionResponse GetFootprintRegion(string regionName)
        {
            return new FootprintRegionResponse(SessionRegions[regionName]);
        }

        public FootprintRegionListResponse ListFootprintRegions()
        {
            return new FootprintRegionListResponse()
            {
                Regions = SessionRegions.Values.ToArray()
            };
        }

        #endregion
        #region Boolean operations

        public FootprintRegionResponse CombineFootprintRegions(string regionName, string operation, bool keepOriginal, FootprintRegionRequest request)
        {
            if (request.Sources == null)
            {
                throw new ArgumentNullException("regionNames", "The region list cannot be null.");
            }

            if (request.Sources.Length < 2)
            {
                throw new ArgumentException("At least two regions must be specified.", "regionNames");
            }

            if (SessionRegions.ContainsKey(regionName))
            {
                throw Lib.Error.DuplicateRegionName("editor", "editor", regionName);
            }

            Lib.CombinationMethod method;
            if (!Enum.TryParse(operation, true, out method))
            {
                throw new ArgumentException("Invalid boolean operation.");
            }

            Spherical.Region combined = null;

            for (int i = 0; i < request.Sources.Length; i++)
            {
                var r = SessionRegions[request.Sources[i]].Region;

                if (i == 0)
                {
                    combined = (Spherical.Region)r.Clone();
                }
                else
                {
                    switch (method)
                    {
                        case Lib.CombinationMethod.Union:
                            combined.SmartUnion(r);
                            break;
                        case Lib.CombinationMethod.Intersect:
                            combined = combined.SmartIntersect(r, true);
                            break;
                        case Lib.CombinationMethod.Subtract:
                            combined = combined.SmartDifference(r);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }

                if (!keepOriginal)
                {
                    SessionRegions.Remove(request.Sources[i]);
                }
            }

            var newregion = new FootprintRegion()
            {
                Name = regionName,
                Region = combined,
            };

            SessionRegions.Add(regionName, newregion);
            return new FootprintRegionResponse(newregion);
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
            string degreeStyle,
            string highlights)
        {
            // Set highlights
            HashSet<string> hl = null;

            if (highlights != null && highlights.Length > 0)
            {
                hl = new HashSet<string>(highlights.Split(','));
            }

            foreach (var name in SessionRegions.Keys)
            {
                var r = SessionRegions[name];
                r.BrushIndex = r.PenIndex = (hl != null && hl.Contains(name)) ? 1 : 0;
            }

            var plotParameters = new Plot(projection, sys, ra, dec, b, l, width, height, colorTheme, autoZoom, autoRotate, grid, degreeStyle);
            return PlotFootprintAdvanced(plotParameters);
        }

        public Spherical.Visualizer.Plot PlotFootprintAdvanced(Plot plotParameters)
        {
            return plotParameters.GetPlot(SessionRegions.Values);
        }

        public Stream GetFootprintThumbnail()
        {
            throw new NotImplementedException();
        }

        #endregion
        #region Individual region set, get and plot

        public void SetFootprintRegionShape(string regionName, Stream stream)
        {
            var region = new FootprintRegion()
            {
                Name = regionName,
                Owner = RestOperationContext.Current.Principal.Identity.Name,
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
            string degreeStyle,
            string highlights)
        {
            var plotParameters = new Plot(projection, sys, ra, dec, b, l, width, height, colorTheme, autoZoom, autoRotate, grid, degreeStyle);
            return PlotFootprintRegionAdvanced(regionName, plotParameters);
        }

        public Spherical.Visualizer.Plot PlotFootprintRegionAdvanced(string regionName, Plot plotParameters)
        {
            return plotParameters.GetPlot(new[] { SessionRegions[regionName] });
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
