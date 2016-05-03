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
using Jhu.Graywulf.Web.Services;
using Lib = Jhu.Footprint.Web.Lib;

namespace Jhu.Footprint.Web.Api.V1
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    [RestServiceBehavior]
    public class FootprintService : RestServiceBase, IFootprintService
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

        private Lib.Context CreateContext()
        {
            return CreateContext(false);
        }

        private Lib.Context CreateContext(bool autoDispose)
        {
            var context = new Lib.Context();

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

        #region Footprint CRUD operations

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
            return FindFootprints(owner, name, from, max);
        }

        public FootprintListResponse FindFootprints(string owner, string name, int from, int max)
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
                    Self = FootprintService.GetUrl("test")
                },
                Footprints = results.Select(f => new Footprint(f)),
            };
        }

        #endregion
        #region Region CRUD operations

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

                // TODO: update region cache

                return new FootprintRegionResponse(footprint, region);
            }
        }

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

        public FootprintRegionResponse ModifyUserFootprintRegion(string owner, string name, string regionName, FootprintRegionRequest request)
        {
            using (var context = CreateContext())
            {
                var footprint = new Lib.Footprint(context);

                footprint.Load(owner, name);

                var region = new Lib.FootprintRegion(footprint);

                region.FootprintId = footprint.Id;
                region.Name = regionName;
                region.Load();
                request.Region.GetValues(region);
                region.Save();

                // TODO: update region cache

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

                // TODO: update region cache
            }
        }

        #endregion

        public string GetUserFootprintShape(string owner, string name, string operation, double limit)
        {
            throw new NotImplementedException();
        }

        public string GetUserFootprintOutline(string owner, string name, string operation, double limit)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Lib.EquatorialPoint> GetUserFootprintOutlinePoints(string owner, string name, string operation, double limit, double resolution)
        {
            throw new NotImplementedException();
        }

        public Stream GetUserFootprintPlot(string owner, string name, string projection, float width, float height, string degStyle, bool grid, bool autoZoom, bool autoRotate)
        {
            throw new NotImplementedException();
        }

        public string GetUserFootprintRegionShape(string owner, string name, string regionName, string operation, double limit)
        {
            throw new NotImplementedException();
        }

        public string GetUserFootprintRegionOutline(string owner, string name, string regionName, string operation, double limit)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Lib.EquatorialPoint> GetUserFootprintRegionOutlinePoints(string owner, string name, string regionName, string operation, double limit, double resolution)
        {
            throw new NotImplementedException();
        }

        public Stream GetUserFootprintRegionPlot(string userName, string name, string regionName, string projection, float width, float height, string degStyle, bool grid, bool autoZoom, bool autoRotate)
        {
            throw new NotImplementedException();
        }

        public static Uri GetUrl(string relativeUri)
        {
            // TODO: where to take machine name from? HttpContext?
            return new Uri("http://" + Environment.MachineName + "/footprint/api/v1/Footprint.svc" + relativeUri);
        }

        public static Uri GetUrl(Lib.Footprint footprint)
        {
            // TODO: where to take machine name from? HttpContext?
            return new Uri("http://" + Environment.MachineName + "/footprint/api/v1/Footprint.svc/users" + footprint.Owner + "/" + footprint.Name);
        }

        public static Uri GetUrl(Lib.Footprint footprint, Lib.FootprintRegion region)
        {
            // TODO: where to take machine name from? HttpContext?
            return new Uri("http://" + Environment.MachineName + "/footprint/api/v1/Footprint.svc/users/" + footprint.Owner + "/" + footprint.Name + "/" + region.Name);
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

        #endregion

#endif
    }
}
