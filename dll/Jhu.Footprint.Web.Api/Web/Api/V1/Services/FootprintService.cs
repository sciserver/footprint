using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web;
using System.ServiceModel.Web;
using Jhu.Graywulf.Web.Services;

namespace Jhu.Footprint.Web.Api.V1
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    [RestServiceBehavior]
    public class FootprintService : ServiceBase, IFootprintService
    {
        #region Constructors and initializers

        public FootprintService()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
        }

        #endregion
        #region Footprint CRUD operations

        public FootprintResponse GetUserFootprint(string owner, string name)
        {
            using (var context = CreateContext())
            {
                var footprint = new Lib.Footprint(context)
                {
                    Owner = owner,
                    Name = name,
                };

                footprint.Load();

                return new FootprintResponse(footprint);
            }
        }

        public FootprintResponse CreateUserFootprint(string owner, string name, FootprintRequest request)
        {
            using (var context = CreateContext())
            {
                var footprint = new Lib.Footprint(context)
                {
                    Owner = owner,
                    Name = name
                };
                request.Footprint.GetValues(footprint);
                footprint.Save();

                return new FootprintResponse(footprint);
            }
        }

        public FootprintResponse ModifyUserFootprint(string owner, string name, FootprintRequest request)
        {
            using (var context = CreateContext())
            {
                var footprint = new Lib.Footprint(context)
                {
                    Owner = owner,
                    Name = name
                };
                footprint.Load();
                request.Footprint.GetValues(footprint);
                footprint.Save();

                return new FootprintResponse(footprint);
            }
        }

        public void DeleteUserFootprint(string owner, string name)
        {
            using (var context = CreateContext())
            {
                var footprint = new Lib.Footprint(context)
                {
                    Owner = owner,
                    Name = name
                };
                footprint.Load();
                footprint.Delete();
            }
        }

        #endregion
        #region Footprint search operations

        public FootprintListResponse FindUserFootprints(string owner, string name, int from, int max)
        {
            var context = CreateContext(true);
            var s = new Lib.FootprintSearch(context)
            {
                Owner = owner,
                Name = name
            };

            var results = s.Find(from, max, null);
            return new FootprintListResponse()
            {
                Links = new Links()
                {
                    Self = GetCurrentUrl()
                },
                Footprints = results.Select(f => new Footprint(f)),
            };
        }

        public FootprintListResponse FindFootprints(string owner, string name, int from, int max)
        {
            var context = CreateContext(true);
            var s = new Lib.FootprintSearch(context)
            {
                Owner = owner == null ? null : "%" + owner + "%",
                Name = name == null ? null : "%" + name + "%"
            };

            var results = s.Find(from, max, null);
            return new FootprintListResponse()
            {
                Links = new Links()
                {
                    Self = GetCurrentUrl()
                },
                Footprints = results.Select(f => new Footprint(f)),
            };
        }

        public FootprintRegionListResponse FindUserFootprintRegions(string owner, string footprintName, string name, int from, int max)
        {
            var context = CreateContext(true);
            var s = new Lib.FootprintRegionSearch(context)
            {
                Owner = owner,
                FootprintName = footprintName,
                Name = name == null ? null : "%" + name + "%"
            };


            var results = s.Find(from, max, null);

            var f = new Lib.Footprint(context)
            {
                Owner = owner,
                Name = footprintName,
            };

            f.Load();

            return new FootprintRegionListResponse()
            {
                Regions = results.Where(r => r.FootprintId == f.Id).Select(r => new FootprintRegion(f, r))
            };
        }
        #endregion
        #region Footprint region CRUD operations

        public FootprintRegionResponse GetUserFootprintRegion(string owner, string name, string regionName)
        {
            using (var context = CreateContext())
            {
                var footprint = new Lib.Footprint(context);

                footprint.Load(owner, name);

                var region = new Lib.FootprintRegion(footprint);

                region.FootprintId = footprint.Id;
                region.Name = regionName;
                region.Load();

                return new FootprintRegionResponse(footprint, region);
            }
        }

        public FootprintRegionResponse CreateUserFootprintRegion(string owner, string name, string regionName, FootprintRegionRequest request)
        {
            using (var context = CreateContext())
            {
                var footprint = new Lib.Footprint(context);
                footprint.Load(owner, name);

                var region = new Lib.FootprintRegion(footprint)
                {
                    Name = regionName,
                };
                request.Region.GetValues(region);
                region.Save();
                region.SaveRegion();

                return new FootprintRegionResponse(footprint, region);
            }
        }

        public FootprintRegionResponse ModifyUserFootprintRegion(string owner, string name, string regionName, FootprintRegionRequest request)
        {
            using (var context = CreateContext())
            {
                var footprint = new Lib.Footprint(context);

                footprint.Load(owner, name);

                var region = new Lib.FootprintRegion(footprint);
                region.Load(regionName);
                request.Region.GetValues(region);
                region.Save();

                return new FootprintRegionResponse(footprint, region);
            }
        }

        public void DeleteUserFootprintRegion(string owner, string name, string regionName)
        {
            using (var context = CreateContext())
            {
                var footprint = new Lib.Footprint(context);

                footprint.Load(owner, name);

                var region = new Lib.FootprintRegion(footprint);

                region.FootprintId = footprint.Id;
                region.Name = regionName;
                region.Load();
                region.Delete();
            }
        }

        #endregion
        #region Footprint combined region get and plot

        public Spherical.Region GetUserFootprintShape(string owner, string name)
        {
            using (var context = CreateContext())
            {
                var footprint = new Lib.Footprint(context);

                footprint.Load(owner, name);

                return footprint.CombinedRegion.Region;
            }
        }

        public Spherical.Outline GetUserFootprintOutline(string owner, string name)
        {
            using (var context = CreateContext())
            {
                var footprint = new Lib.Footprint(context);

                footprint.Load(owner, name);

                return footprint.CombinedRegion.Region.Outline;
            }
        }

        public IEnumerable<Lib.EquatorialPoint> GetUserFootprintOutlinePoints(string owner, string name, double resolution)
        {
            using (var context = CreateContext())
            {
                var footprint = new Lib.Footprint(context);

                footprint.Load(owner, name);

                return Lib.FootprintFormatter.InterpolateOutlinePoints(footprint.CombinedRegion.Region.Outline, resolution);
            }
        }

        public Spherical.Visualizer.Plot PlotUserFootprint(string owner, string name, string projection, string sys, string ra, string dec, string b, string l, float width, float height, string colorTheme, string autoZoom, string autoRotate, string grid, string degreeStyle)
        {

            //var plot = Lib.FootprintPlot.GetPlot(new[] { footprint.CombinedRegion.Region }, projection, sys, ra, dec, b, l, width, height, colorTheme);
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
                Width = width,
                Height = height,
                ColorTheme = colorTheme,
                AutoZoom = (autoZoom == "") ? true : Convert.ToBoolean(autoZoom),
                AutoRotate = (autoRotate == "") ? true : Convert.ToBoolean(autoRotate),
                GridVisible = (grid == "") ? true : Convert.ToBoolean(grid),
                DegreeStyle = degreeStyle
            };

            return PlotUserFootprintAdvanced(owner, name, plotParameters);

        }

        public Spherical.Visualizer.Plot PlotUserFootprintAdvanced(string owner, string name, Plot plotParameters)
        {
            using (var context = CreateContext())
            {
                var footprint = new Lib.Footprint(context);

                footprint.Load(owner, name);

                return plotParameters.GetPlot(new[] { footprint.CombinedRegion.Region });
            }
        }

        public Stream GetUserFootprintThumbnail(string owner, string name)
        {
            using (var context = CreateContext())
            {
                var footprint = new Lib.Footprint(context);
                footprint.Load(owner, name);

                WebOperationContext.Current.OutgoingResponse.ContentType = "image/jpeg";

                var ms = new MemoryStream(footprint.CombinedRegion.Thumbnail);
                return ms;
            }
        }


        #endregion
        #region Individual region get and plot

        public void SetUserFootprintRegionShape(string owner, string name, string regionName, Stream stream)
        {
            using (var context = CreateContext())
            {
                var footprint = new Lib.Footprint(context);

                footprint.Load(owner, name);

                var region = new Lib.FootprintRegion(footprint);
                region.Load(regionName);

                // Parse region from posted data
                region.Region = new RegionAdapter().ReadFromStream(stream);

                region.SaveRegion();
            }
        }

        public Spherical.Region GetUserFootprintRegionShape(string owner, string name, string regionName)
        {
            using (var context = CreateContext())
            {
                var footprint = new Lib.Footprint(context);
                footprint.Load(owner, name);

                var region = new Lib.FootprintRegion(footprint);
                region.Load(regionName);

                return region.Region;
            }

        }

        public Spherical.Outline GetUserFootprintRegionOutline(string owner, string name, string regionName)
        {
            using (var context = CreateContext())
            {
                var footprint = new Lib.Footprint(context);
                footprint.Load(owner, name);

                var region = new Lib.FootprintRegion(footprint);
                region.Load(regionName);

                return region.Region.Outline;
            }
        }

        public IEnumerable<Lib.EquatorialPoint> GetUserFootprintRegionOutlinePoints(string owner, string name, string regionName, double resolution)
        {
            using (var context = CreateContext())
            {
                var footprint = new Lib.Footprint(context);
                footprint.Load(owner, name);

                var region = new Lib.FootprintRegion(footprint);
                region.Load(regionName);

                return Lib.FootprintFormatter.InterpolateOutlinePoints(region.Region.Outline, resolution);
            }
        }

        public Spherical.Visualizer.Plot PlotUserFootprintRegion(string owner, string name, string regionName, string projection, string sys, string ra, string dec, string b, string l, float width, float height, string colorTheme, string autoZoom, string autoRotate, string grid, string degreeStyle)
        {
            var plotParameters = new Plot()
            {
                Projection = projection,
                CoordinateSystem = sys,
                //Ra = ra,
                //Dec = dec
                //B = b,
                //L = l,
                Width = width,
                Height = height,
                ColorTheme = colorTheme,
                AutoZoom = (autoZoom == "") ? true : Convert.ToBoolean(autoZoom),
                AutoRotate = (autoRotate == "") ? true : Convert.ToBoolean(autoRotate),
                GridVisible = (grid == "") ? true : Convert.ToBoolean(grid),
                DegreeStyle = degreeStyle
            };

            // TODO: change this part to use all parameters
            // Size is different for vector graphics!

            return PlotUserFootprintRegionAdvanced(owner, name, regionName, plotParameters);

        }

        public Spherical.Visualizer.Plot PlotUserFootprintRegionAdvanced(string owner, string name, string regionName, Plot plotParameters)
        {
            using (var context = CreateContext())
            {
                var footprint = new Lib.Footprint(context);
                footprint.Load(owner, name);

                var region = new Lib.FootprintRegion(footprint);
                region.Load(regionName);

                //var plot = Lib.FootprintPlot.GetDefaultPlot(new[] { region.Region });


                return plotParameters.GetPlot(new[] { region.Region });
            }
        }

        public Stream GetUserFootprintRegionThumbnail(string owner, string name, string regionName)
        {
            using (var context = CreateContext())
            {
                var footprint = new Lib.Footprint(context);
                footprint.Load(owner, name);

                var region = new Lib.FootprintRegion(footprint);
                region.Load(regionName);

                WebOperationContext.Current.OutgoingResponse.ContentType = "image/jpeg";

                var ms = new MemoryStream(region.Thumbnail);
                return ms;
            }
        }

        #endregion
        #region URI constructors

        private static Uri GetBaseUrl(string relativeUri)
        {
            // TODO: where to take machine name from? HttpContext?
            return new Uri("http://" + Environment.MachineName + "/footprint/api/v1/Footprint.svc" + relativeUri);
        }

        private static Uri GetCurrentUrl()
        {
            return GetBaseUrl(HttpContext.Current.Request.Url.PathAndQuery);
        }

        public static Uri GetUrl(Lib.Footprint footprint)
        {
            // TODO: where to take machine name from? HttpContext?
            return GetBaseUrl("/users" + footprint.Owner + "/" + footprint.Name);
        }

        public static Uri GetUrl(Lib.Footprint footprint, Lib.FootprintRegion region)
        {
            // TODO: where to take machine name from? HttpContext?
            return GetBaseUrl("/footprint/api/v1/Footprint.svc/users/" + footprint.Owner + "/" + footprint.Name + "/" + region.Name);
        }

        #endregion
    }
}




#if false

#region Private Methods
        private Lib.Footprint GetFootprint(string userName, string folderName, string footprintName)
        {
            using (var context = new Lib.Context())
            {
                // TODO : REAL AUTHENTICATION
                context.User = userName;

                var search = new Lib.FootprintSearch(context);
                search.User = userName;
                search.FolderName = folderName;
                search.FootprintName = footprintName;

                // load footprint
                var footprint = new Lib.Footprint(context);
                footprint.User = userName;
                footprint.Id = search.GetFootprintId();
                footprint.Load();
                footprint.Region.Simplify();

                return footprint;
            }

        }

        private Lib.Footprint GetFolderFootprint(string userName, string folderName)
        {
            using (var context = new Lib.Context())
            {
                // TODO : REAL AUTHENTICATION
                context.User = userName;

                Lib.FootprintFolder folder = new Lib.FootprintFolder(context);
                folder.Owner = userName;
                folder.Name = folderName;
                folder.Id = -1;
                folder.Load();

                var footprint = new Lib.Footprint(context);
                footprint.Id = folder.FootprintId;
                footprint.User = userName;
                footprint.Load();

                footprint.Region.Simplify();

                return footprint;
            }
        }

        private Spherical.Region GetFootprintConvexHull(string userName, string folderName, string footprintName)
        {
            var fp = GetFootprint(userName, folderName, footprintName);

            var chull = fp.Region.Outline.GetConvexHull();
            chull.Simplify();

            return chull;
        }

        private Spherical.Region GetFolderFootprintConvexHull(string userName, string folderName)
        {
            var fp = GetFolderFootprint(userName, folderName);

            var chull = fp.Region.Outline.GetConvexHull();
            chull.Simplify();

            return chull;
        }

        private void SetPlotProperties(Lib.Plot plot, Spherical.Region region, float width, float height, string projection, string degStyle, bool grid, bool autoZoom, bool autoRotate)
        {
            plot.Region = region;
            if (width > 0) plot.Width = width * 96;
            if (height > 0) plot.Height = height * 96;

            try
            {
                projection = "Jhu.Spherical.Visualizer." + projection + "Projection,Jhu.Spherical.Visualizer";
                var t = Type.GetType(projection);
                plot.Projection = (Jhu.Spherical.Visualizer.Projection)Activator.CreateInstance(t);
            }
            catch (Exception e)
            {
                plot.Projection = new Jhu.Spherical.Visualizer.AitoffProjection();
            }

            plot.DegStyle = degStyle;
            plot.Grid = grid;
            plot.AutoZoom = autoZoom;
            plot.AutoRotate = autoRotate;

        }


#endregion

#region FootprintFolder Methods
        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public FootprintListResponse GetUserFootprintFolderList(string userName)
        {
            IEnumerable<Lib.FootprintFolder> folders;

            using (var context = new Lib.Context())
            {
                var search = new Lib.FootprintFolderSearch(context);
                search.User = userName;
                search.Source = Lib.SearchSource.My;
                folders = search.Find();
            }

            var r = new FootprintListResponse(folders);
            return r;
        }


        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public FootprintResponse GetUserFootprintFolder(string userName, string folderName)
        {
            Lib.FootprintFolder folder;
            IEnumerable<Lib.Footprint> footprints;
            using (var context = new Lib.Context())
            {
                // TODO : REAL AUTHENTICATION
                context.User = userName;

                // load footprint folder info
                folder = new Lib.FootprintFolder(context);
                folder.Owner = userName;
                folder.Name = folderName;
                folder.Id = -1;
                folder.Load();

                //get footprints from folder
                var search = new Lib.FootprintSearch(context) { User = folder.Owner, FolderId = folder.Id };
                footprints = search.GetFootprintsByFolderId();
            }

            var f = new Footprint(folder);

            var list = footprints.Select(fp => new FootprintRegion(fp, folderName).Url).ToArray();
            var r = new FootprintResponse(f, list);
            return r;

        }


        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public string GetUserFootprintFolderRegion(string userName, string folderName)
        {
            var footprint = GetFolderFootprint(userName, folderName);
            var f = new FootprintRegion(footprint, folderName);
            return f.RegionString;
        }

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public string GetUserFootprintFolderRegionOutline(string userName, string folderName)
        {
            var footprint = GetFolderFootprint(userName, folderName);
            return footprint.Region.Outline.ToString();
        }

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public IEnumerable<Lib.EquatorialPoint> GetUserFootprintFolderRegionOutlinePoints(string userName, string folderName, double resolution)
        {
            var footprint = GetFolderFootprint(userName, folderName);
            return Lib.FootprintFormatter.InterpolateOutlinePoints(footprint.Region.Outline, resolution);
        }

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public string GetUserFootprintFolderRegionReducedOutline(string userName, string folderName, double limit)
        {
            var footprint = GetFolderFootprint(userName, folderName);
            footprint.Region.Outline.Reduce(limit / 648000.0 * Math.PI);
            return footprint.Region.Outline.ToString();
        }

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public IEnumerable<Lib.EquatorialPoint> GetUserFootprintFolderRegionReducedOutlinePoints(string userName, string folderName, double resolution, double limit)
        {
            var footprint = GetFolderFootprint(userName, folderName);
            footprint.Region.Outline.Reduce(limit / 648000.0 * Math.PI);
            return Lib.FootprintFormatter.InterpolateOutlinePoints(footprint.Region.Outline, resolution);
        }

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public string GetUserFootprintFolderRegionConvexHull(string userName, string folderName)
        {
            var chull = GetFolderFootprintConvexHull(userName, folderName);
            return chull.ToString();
        }

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public string GetUserFootprintFolderRegionConvexHullOutline(string userName, string folderName)
        {
            var chull = GetFolderFootprintConvexHull(userName, folderName);
            return chull.Outline.ToString();
        }

        public IEnumerable<Lib.EquatorialPoint> GetUserFootprintFolderRegionConvexHullOutlinePoints(string userName, string folderName, double resolution)
        {
            var chull = GetFolderFootprintConvexHull(userName, folderName);

            return Lib.FootprintFormatter.InterpolateOutlinePoints(chull.Outline, resolution);
        }
        public Stream GetUserFootprintFolderPlot(string userName, string folderName, string projection, float width, float height, string degStyle, bool grid, bool autoZoom, bool autoRotate)
        {
            var fp = GetFolderFootprint(userName, folderName);
            fp.Region.Simplify();

            MemoryStream ms = new MemoryStream();

            var plot = new Lib.Plot();
            SetPlotProperties(plot, fp.Region, width, height, projection, degStyle, grid, autoZoom, autoRotate);
            plot.PlotFootprint(ms);
            ms.Position = 0;

            WebOperationContext.Current.OutgoingResponse.ContentType = "image/jpeg";

            return ms;
        }

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public void CreateUserFootprintFolder(string userName, string folderName, FootprintRequest request)
        {
            using (var context = new Jhu.Footprint.Web.Lib.Context())
            {
                var folder = request.FootprintFolder.GetValue();
                folder.Context = context;
                folder.Save();
            }
        }

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public void ModifyUserFootprintFolder(string userName, string folderName, FootprintRequest request)
        {
            using (var context = new Jhu.Footprint.Web.Lib.Context())
            {
                // TODO : REAL AUTHENTICATION
                context.User = userName;

                var folder = request.FootprintFolder.GetValue();
                folder.Context = context;
                folder.Save();

            }
        }

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public void DeleteUserFootprintFolder(string userName, string folderName)
        {
            using (var context = new Jhu.Footprint.Web.Lib.Context())
            {
                // TODO : REAL AUTHENTICATION
                context.User = userName;

                var folder = new Jhu.Footprint.Web.Lib.FootprintFolder(context);
                folder.Name = folderName;
                folder.Owner = userName;
                folder.Id = -1;
                folder.Delete();
            }
        }
#endregion

#region Footprint Methods
        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public FootprintRegionResponse GetUserFootprint(string userName, string folderName, string footprintName)
        {
            var footprint = GetFootprint(userName, folderName, footprintName);
            var f = new FootprintRegion(footprint, folderName);
            var r = new FootprintRegionResponse(f);
            return r;
        }

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public string GetUserFootprintRegion(string userName, string folderName, string footprintName)
        {
            var fp = GetFootprint(userName, folderName, footprintName);
            return fp.Region.ToString();
        }

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public string GetUserFootprintRegionOutline(string userName, string folderName, string footprintName)
        {
            var fp = GetFootprint(userName, folderName, footprintName);
            return fp.Region.Outline.ToString();
        }

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public IEnumerable<Lib.EquatorialPoint> GetUserFootprintRegionOutlinePoints(string userName, string folderName, string footprintName, double resolution)
        {
            var fp = GetFootprint(userName, folderName, footprintName);
            return Lib.FootprintFormatter.InterpolateOutlinePoints(fp.Region.Outline, resolution);
        }

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public string GetUserFootprintRegionReducedOutline(string userName, string folderName, string footprintName, double limit)
        {
            var fp = GetFolderFootprint(userName, folderName);
            fp.Region.Outline.Reduce(limit / 648000.0 * Math.PI);
            return fp.Region.Outline.ToString();
        }

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public IEnumerable<Lib.EquatorialPoint> GetUserFootprintRegionReducedOutlinePoints(string userName, string folderName, string footprintName, double resolution, double limit)
        {
            var fp = GetFolderFootprint(userName, folderName);
            fp.Region.Outline.Reduce(limit / 648000.0 * Math.PI);
            return Lib.FootprintFormatter.InterpolateOutlinePoints(fp.Region.Outline, resolution);
        }

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public string GetUserFootprintRegionConvexHull(string userName, string folderName, string footprintName)
        {
            var chull = GetFootprintConvexHull(userName, folderName, footprintName);
            return chull.ToString();
        }

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public string GetUserFootprintRegionConvexHullOutline(string userName, string folderName, string footprintName)
        {
            var chull = GetFootprintConvexHull(userName, folderName, footprintName);
            return chull.Outline.ToString();
        }

        public IEnumerable<Lib.EquatorialPoint> GetUserFootprintRegionConvexHullOutlinePoints(string userName, string folderName, string footprintName, double resolution)
        {
            var chull = GetFootprintConvexHull(userName, folderName, footprintName);
            return Lib.FootprintFormatter.InterpolateOutlinePoints(chull.Outline, resolution);
        }

        public Stream GetUserFootprintPlot(string userName, string folderName, string footprintName, string projection, float width, float height, string degStyle, bool grid, bool autoZoom, bool autoRotate)
        {
            var fp = GetFootprint(userName, folderName, footprintName);
            fp.Region.Simplify();

            MemoryStream ms = new MemoryStream();

            var plot = new Lib.Plot();
            SetPlotProperties(plot, fp.Region, width, height, projection, degStyle, grid, autoZoom, autoRotate);
            plot.PlotFootprint(ms);
            ms.Position = 0;

            WebOperationContext.Current.OutgoingResponse.ContentType = "image/jpeg";

            return ms;
        }

#endregion
#region Create, modify, delete footprint

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public void CreateUserFootprint(string userName, string folderName, string footprintName, FootprintRegionRequest request)
        {
            using (var context = new Lib.Context())
            {
                var footprint = request.Region.GetValue();
                footprint.Context = context;
                footprint.Save();
            }
        }

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public void ModifyUserFootprint(string userName, string folderName, string footprintName, FootprintRegionRequest request)
        {
            using (var context = new Lib.Context())
            {
                // TODO : REAL AUTHENTICATION
                context.User = userName;

                var footprint = request.Region.GetValue();
                footprint.Context = context;
                footprint.Save();
            }
        }

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public void DeleteUserFootprint(string userName, string folderName, string footprintName)
        {
            using (var context = new Lib.Context())
            {

                // TODO : REAL AUTHENTICATION
                context.User = userName;

                var search = new Lib.FootprintSearch(context);
                search.FootprintName = footprintName;
                search.FolderName = folderName;
                search.User = userName;

                var footprint = new Lib.Footprint(context);
                footprint.User = userName;
                footprint.Id = search.GetFootprintId();
                footprint.Delete();
            }
        }
#endregion
    }
}

#endif