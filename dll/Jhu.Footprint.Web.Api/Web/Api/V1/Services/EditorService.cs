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
        #region Web session access

        internal const string SessionKeyEditorFootprint = "Jhu.Footprint.Web.Api.SessionFootprint";
        internal const string SessionKeyEditorRegions = "Jhu.Footprint.Web.Api.SessionRegions";
        internal const string SessionKeyEditorCombinedRegion = "Jhu.Footprint.Web.Api.SessionCombinedRegion";

        private Lib.Footprint SessionFootprint
        {
            get
            {
                var footprint = (Lib.Footprint)Session[SessionKeyEditorFootprint];

                if (footprint == null)
                {
                    footprint = new Lib.Footprint()
                    {
                        CombinationMethod = Lib.CombinationMethod.None,
                        Name = "new_footprint",
                        Owner = User.Identity.Name,
                    };
                    Session[SessionKeyEditorFootprint] = footprint;
                }

                return footprint;
            }
        }

        private Dictionary<string, Lib.FootprintRegion> SessionRegions
        {
            get
            {
                var sessionRegions = (Dictionary<string, Lib.FootprintRegion>)Session[SessionKeyEditorRegions];

                if (sessionRegions == null)
                {
                    sessionRegions = new Dictionary<string, Lib.FootprintRegion>();
                    Session[SessionKeyEditorRegions] = sessionRegions;
                }

                return sessionRegions;
            }
        }

        private Lib.FootprintRegion SessionCombinedRegion
        {
            get
            {
                var region = (Lib.FootprintRegion)Session[SessionKeyEditorCombinedRegion];

                if (region == null)
                {
                    region = new Lib.FootprintRegion(SessionFootprint);
                    region.Region = new Spherical.Region();
                    Session[SessionKeyEditorCombinedRegion] = region;
                }

                return region;
            }
            set
            {
                Session[SessionKeyEditorCombinedRegion] = value;
            }
        }

        private void InvalidateCombinedRegion()
        {
            SessionCombinedRegion = null;
        }

        private void UpdateCombinedRegion()
        {
            if (SessionCombinedRegion.Region.ConvexList.Count == 0 &&
                SessionRegions.Count > 0 &&
                SessionFootprint.CombinationMethod != Lib.CombinationMethod.None)
            {
                RefreshCombinedRegion();
            }
        }

        private void UpdateCombinedRegion(Spherical.Region region)
        {
            var footprint = SessionFootprint;
            var combined = SessionCombinedRegion.Region;

            if (combined.ConvexList.Count != 0)
            {
                // Update existing
                switch (footprint.CombinationMethod)
                {
                    case Lib.CombinationMethod.Intersect:
                        combined = combined.SmartIntersect(region, true);
                        break;
                    case Lib.CombinationMethod.Union:
                        combined.SmartUnion(region, true, 1000);
                        break;
                    case Lib.CombinationMethod.None:
                        // no op
                        break;
                    default:
                        throw new NotImplementedException();
                }

                SessionCombinedRegion.Region = combined;
            }
            else if (footprint.CombinationMethod != Lib.CombinationMethod.None)
            {
                RefreshCombinedRegion();
            }
        }

        private void RefreshCombinedRegion()
        {
            var footprint = SessionFootprint;
            Spherical.Region combined = null;

            // From scratch
            int q = 0;
            foreach (var r in SessionRegions.Values)
            {
                if (q == 0)
                {
                    combined = r.Region;
                }
                else
                {
                    switch (footprint.CombinationMethod)
                    {
                        case Lib.CombinationMethod.Intersect:
                            combined = combined.SmartIntersect(r.Region, true);
                            break;
                        case Lib.CombinationMethod.Union:
                            combined.SmartUnion(r.Region, true, 1000);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }

                q++;
            }

            SessionCombinedRegion.Region = combined;
        }

        #endregion
        #region Footprint CRUD operations

        public FootprintResponse GetFootprint()
        {
            return new FootprintResponse(SessionFootprint);
        }

        public FootprintResponse ModifyFootprint(FootprintRequest footprint)
        {
            footprint.Footprint.GetValues(SessionFootprint);
            InvalidateCombinedRegion();
            return new FootprintResponse(SessionFootprint);
        }

        public void DeleteFootprint()
        {
            Session[SessionKeyEditorFootprint] = null;
            Session[SessionKeyEditorRegions] = null;
            Session[SessionKeyEditorCombinedRegion] = null;
        }

        public Spherical.Region DownloadFootprint()
        {
            UpdateCombinedRegion();
            return SessionCombinedRegion.Region;
        }

        public Spherical.Outline DownloadFootprintOutline()
        {
            UpdateCombinedRegion();
            return SessionCombinedRegion.Region.Outline;
        }

        public IEnumerable<Point> GetFootprintOutlinePoints(CoordinateSystem? sys, CoordinateRepresentation? rep, double? resolution, OutlineReduction? reduce, double? limit)
        {
            UpdateCombinedRegion();
            return GetRegionOutlinePointsImpl(SessionCombinedRegion.Region, sys, rep, resolution, reduce, limit);
        }

        public Spherical.Visualizer.Plot PlotFootprint(
            Projection? projection,
            CoordinateSystem? sys,
            double? lon,
            double? lat,
            float? width,
            float? height,
            ColorTheme? colorTheme,
            bool? autoZoom,
            bool? autoRotate,
            bool? grid,
            DegreeStyle? degreeStyle)
        {
            var plot = new Plot()
            {
                Projection = projection,
                CoordinateSystem = sys,
                Lon = lon,
                Lat = lat,
                Width = width,
                Height = height,
                ColorTheme = colorTheme,
                AutoZoom = autoZoom,
                AutoRotate = autoRotate,
                GridVisible = grid,
                DegreeStyle = degreeStyle
            };

            UpdateCombinedRegion();
            return PlotRegionImpl(plot, new[] { SessionCombinedRegion });
        }

        public Spherical.Visualizer.Plot PlotFootprintAdvanced(PlotRequest plot)
        {
            UpdateCombinedRegion();
            return PlotRegionImpl(plot.Plot, SessionRegions.Values);
        }

        #endregion
        #region Footprint region CRUD operations

        private void ApplyRotation(Spherical.Region region, Spherical.Rotation r)
        {
            region.Rotate(r);
        }

        private void ApplyCoordinateSystem(Spherical.Region region, CoordinateSystem sys)
        {
            Spherical.Rotation r;

            switch (sys)
            {
                case CoordinateSystem.EqJ2000:
                    return;
                case CoordinateSystem.GalJ2000:
                    r = Spherical.Rotation.GalacticToEquatorial;
                    break;
                default:
                    throw new NotImplementedException();
            }

            region.Rotate(r);
        }

        public RegionResponse CreateRegion(string regionName, RegionRequest request)
        {
            if (!Lib.Constants.NamePatternRegex.Match(regionName).Success)
            {
                throw Lib.Error.InvalidName(regionName);
            }

            if (SessionRegions.ContainsKey(regionName))
            {
                throw Lib.Error.DuplicateRegionName("editor", "editor", regionName);
            }

            var region = new Lib.FootprintRegion();
            request.Region.GetValues(region, true);
            region.Name = regionName;

            if (request.Rotation != null)
            {
                ApplyRotation(region.Region, request.Rotation.GetRotation());
            }

            if (request.CoordinateSystem.HasValue)
            {
                ApplyCoordinateSystem(region.Region, request.CoordinateSystem.Value);
            }

            SessionRegions[regionName] = region;
            UpdateCombinedRegion(region.Region);

            return new RegionResponse(new Region(SessionFootprint, SessionRegions[regionName]));
        }

        public RegionResponse GetRegion(string regionName)
        {
            return new RegionResponse(new Region(SessionFootprint, SessionRegions[regionName]));
        }

        public RegionResponse ModifyRegion(string regionName, RegionRequest request)
        {
            var region = SessionRegions[regionName];
            string name;

            if (request.Region.Name != null && StringComparer.InvariantCultureIgnoreCase.Compare(request.Region.Name, regionName) != 0)
            {
                // Renaming, so remove old one
                SessionRegions.Remove(regionName);
                name = request.Region.Name;
            }
            else
            {
                name = regionName;
            }

            request.Region.GetValues(region, false);

            if (request.Rotation != null)
            {
                ApplyRotation(region.Region, request.Rotation.GetRotation());
            }

            if (request.CoordinateSystem.HasValue)
            {
                ApplyCoordinateSystem(region.Region, request.CoordinateSystem.Value);
            }

            SessionRegions[name] = region;
            InvalidateCombinedRegion();

            return new RegionResponse(new Region(SessionFootprint, region));
        }

        public void DeleteRegion(string regionName)
        {
            SessionRegions.Remove(regionName);
            InvalidateCombinedRegion();
        }

        public void DeleteRegions(string regionName)
        {
            var keys = GetMatchingKeys(SessionRegions.Keys, regionName, null, null, out var hasBefore, out var hasAfter);

            foreach (var key in keys)
            {
                SessionRegions.Remove(key);
            }

            InvalidateCombinedRegion();
        }

        public RegionListResponse ListRegions(string regionName, int? from, int? max)
        {
            var res = new List<Region>();
            var keys = GetMatchingKeys(SessionRegions.Keys, regionName, from, max, out var hasBefore, out var hasAfter);

            foreach (var key in keys)
            {
                res.Add(new Region(SessionFootprint, SessionRegions[key]));
            }

            return new RegionListResponse()
            {
                Regions = res
            };

            // TODO: get paging links
        }

        public Spherical.Region DownloadRegion(string regionName)
        {
            return SessionRegions[regionName].Region;
        }

        public void UploadRegion(string regionName, Spherical.Region region)
        {
            Lib.FootprintRegion r;

            if (SessionRegions.ContainsKey(regionName))
            {
                r = SessionRegions[regionName];
            }
            else
            {
                r = new Lib.FootprintRegion(SessionFootprint);
            }

            r.Region = region;
            SessionRegions[regionName] = r;

            UpdateCombinedRegion(region);
        }

        public Spherical.Outline DownloadRegionOutline(string regionName)
        {
            return SessionRegions[regionName].Region.Outline;
        }

        public IEnumerable<Point> GetRegionOutlinePoints(string regionName, CoordinateSystem? sys, CoordinateRepresentation? rep, double? resolution, OutlineReduction? reduce, double? limit)
        {
            var region = SessionRegions[regionName];
            return GetRegionOutlinePointsImpl(region.Region, sys, rep, resolution, reduce, limit);
        }

        private List<Point> GetRegionOutlinePointsImpl(Spherical.Region region, CoordinateSystem? sys, CoordinateRepresentation? rep, double? resolution, OutlineReduction? reduce, double? limit)
        {
            var outline = region.Outline;

            if (reduce.HasValue && reduce.Value == OutlineReduction.Dp)
            {
                outline = (Spherical.Outline)outline.Clone();
                outline.Reduce((limit ?? 1) * SharpAstroLib.Coords.Constants.ArcMin2Radian);
            }
            if (reduce.HasValue && reduce.Value == OutlineReduction.CHull)
            {
                var chull = outline.GetConvexHull();
                outline = chull.Outline;
            }

            // Figure out coordinate system transformation

            SharpAstroLib.Coords.Transformation t;

            switch (sys ?? CoordinateSystem.EqJ2000)
            {
                case CoordinateSystem.EqJ2000:
                    t = SharpAstroLib.Coords.Transformation.Identity;
                    break;
                case CoordinateSystem.GalJ2000:
                    t = SharpAstroLib.Coords.Transformation.Eq2GalJ2000;
                    break;
                default:
                    throw new NotImplementedException();
            }

            var res = new List<Point>();

            foreach (var p in outline.Interpolate((resolution ?? 10)))
            {
                var pp = SharpAstroLib.Coords.Point.FromBoth(p.RA, p.Dec, p.X, p.Y, p.Z);
                pp = t.Apply(pp);

                var pr = new Point();

                switch (rep ?? CoordinateRepresentation.Dec)
                {
                    case CoordinateRepresentation.Dec:
                        pr.Lon = pp.Lon;
                        pr.Lat = pp.Lat;
                        break;
                    case CoordinateRepresentation.Sexa:
                        pr.RA = new SharpAstroLib.Coords.Angle(pp.Lon).ToString(SharpAstroLib.Coords.AngleFormatInfo.DefaultHours);
                        pr.Dec = new SharpAstroLib.Coords.Angle(pp.Lat).ToString(SharpAstroLib.Coords.AngleFormatInfo.DefaultHours);
                        break;
                    case CoordinateRepresentation.Cart:
                        pr.Cx = pp.X;
                        pr.Cy = pp.Y;
                        pr.Cz = pp.Z;
                        break;
                    default:
                        throw new NotImplementedException();
                }

                res.Add(pr);
            }

            return res;
        }

        public Spherical.Visualizer.Plot PlotRegion(
            string regionName,
            Projection? projection,
            CoordinateSystem? sys,
            double? lon,
            double? lat,
            float? width,
            float? height,
            ColorTheme? colorTheme,
            bool? autoZoom,
            bool? autoRotate,
            bool? grid,
            DegreeStyle? degreeStyle)
        {
            var plot = new Plot()
            {
                Projection = projection,
                CoordinateSystem = sys,
                Lon = lon,
                Lat = lat,
                Width = width,
                Height = height,
                ColorTheme = colorTheme,
                AutoZoom = autoZoom,
                AutoRotate = autoRotate,
                GridVisible = grid,
                DegreeStyle = degreeStyle
            };

            // TODO: multiple plot doesn't conform to URI scheme

            var keys = GetMatchingKeys(SessionRegions.Keys, regionName, null, null, out var hasBefore, out var hasAfter);
            var regions = SessionRegions.Join(keys, r => r.Key, k => k, (a, b) => a.Value);

            return PlotRegionImpl(plot, regions);
        }

        public Spherical.Visualizer.Plot PlotRegionAdvanced(string regionName, PlotRequest plot)
        {
            // TODO: highlights, etc.

            // TODO: multiple plot doesn't conform to URI scheme

            var keys = GetMatchingKeys(SessionRegions.Keys, regionName, null, null, out var hasBefore, out var hasAfter);
            var regions = SessionRegions.Join(keys, r => r.Key, k => k, (a, b) => a.Value);

            return PlotRegionImpl(plot.Plot, new[] { SessionRegions[regionName] });
        }

        private Spherical.Visualizer.Plot PlotRegionImpl(Plot plot, IEnumerable<Lib.FootprintRegion> regions)
        {
            // TODO: plot combined region

            return plot.GetPlot(regions);
        }

        #endregion
#if false
        #region XXX
        
        #endregion
        #region Boolean operations

        public FootprintRegionResponse CombineFootprintRegions(string regionName, string operation, bool keepOriginal, RegionRequest request)
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

            var newregion = new Region()
            {
                Name = regionName,
                Region = combined,
            };

            SessionRegions.Add(regionName, newregion);
            return new FootprintRegionResponse(newregion);
        }

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
#endif
#endif

    }
}
