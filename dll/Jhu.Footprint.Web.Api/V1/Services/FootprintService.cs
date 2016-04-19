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
    [ServiceContract]
    [Description("TODO")]
    public interface IFootprintService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "*", Method = "OPTIONS")]
        void HandleHttpOptionsRequest();

        #region FootprintFolder Operation Contracts
        [OperationContract]
        [WebGet(UriTemplate = "/users/{userName}")]
        [Description("Returns the list of all user created folders.")]
        FootprintFolderListResponse GetUserFootprintFolderList(string userName);

        [OperationContract]
        [WebGet(UriTemplate = "/users/{userName}/{folderName}/")]
        [Description("Returns the details of an existing folder and the list of the footprints in it.")]
        FootprintFolderResponse GetUserFootprintFolder(string userName, string folderName);

        [OperationContract]
        [WebGet(UriTemplate = "/users/{userName}/{folderName}/footprint")]
        [Description("Returns the footprint of a folder")]
        string GetUserFootprintFolderRegion(string userName, string folderName);

        [OperationContract]
        [WebGet(UriTemplate = "/users/{userName}/{folderName}/footprint/outline")]
        [Description("Returns the outline of a folder footprint.")]
        string GetUserFootprintFolderRegionOutline(string userName, string folderName);

        [OperationContract]
        [WebGet(UriTemplate = "/users/{userName}/{folderName}/footprint/outline/points?res={resolution}")]
        [Description("Returns the points of the outline of a footprint.")]
        IEnumerable<Lib.EquatorialPoint> GetUserFootprintFolderRegionOutlinePoints(string userName, string folderName, double resolution);

        [OperationContract]
        [WebGet(UriTemplate = "/users/{userName}/{folderName}/footprint/outline/reduced?limit={limit}")]
        [Description("Returns the reduced outline of a folder footprint.")]
        string GetUserFootprintFolderRegionReducedOutline(string userName, string folderName, double limit);

        [OperationContract]
        [WebGet(UriTemplate = "/users/{userName}/{folderName}/footprint/outline/points/reduced?res={resolution}&limit={limit}")]
        [Description("Returns the points of the reduced outline of a footprint.")]
        IEnumerable<Lib.EquatorialPoint> GetUserFootprintFolderRegionReducedOutlinePoints(string userName, string folderName, double resolution, double limit);

        [OperationContract]
        [WebGet(UriTemplate = "/users/{userName}/{folderName}/footprint/convexhull")]
        [Description("Returns the convex hull of a folder footprint.")]
        string GetUserFootprintFolderRegionConvexHull(string userName, string folderName);


        [OperationContract]
        [WebGet(UriTemplate = "/users/{userName}/{folderName}/footprint/convexhull/outline")]
        [Description("Returns the outline of the convex hull of a folder footprint.")]
        string GetUserFootprintFolderRegionConvexHullOutline(string userName, string folderName);

        [OperationContract]
        [WebGet(UriTemplate = "/users/{userName}/{folderName}/footprint/convexhull/outline/points?res={resolution}")]
        [Description("Returns the points of the outline of the convex hull of a folder footprint.")]
        IEnumerable<Lib.EquatorialPoint> GetUserFootprintFolderRegionConvexHullOutlinePoints(string userName, string folderName, double resolution);

        [OperationContract]
        [WebGet(UriTemplate = "/users/{userName}/{folderName}/plot?proj={projection}&width={width}&height={height}&degStyle={degStyle}&grid={grid}&autoZoom={autoZoom}&autoRotate={autoRotate}")]
        [Description(@"Plot Footprint. There are several keywords to costumize a plot:
            proj -- set projection. Available values: Aitoff, Equirectangular, HammerAitoff (deafult), Mollweide, Orthographic, Stereographic
            width -- set width of plot.
            height -- set height of plot.
            degStyle -- set the grid and label style. Available values: hms - hexagecimal, dms (default) - degree.
            grid -- turn grid on/off. Values = true, false (default)
            autoZoom -- turn auto zoom on/off. Values = true, false (default)
            autoRotate -- turn auto rotate on/off. Values = true, false (default)")]
        Stream GetUserFootprintFolderPlot(string userName, string folderName, string projection, float width, float height, string degStyle, bool grid, bool autoZoom, bool autoRotate);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = "/users/{userName}/{folderName}")]
        [Description("Create new footprint folder.")]
        void CreateUserFootprintFolder(string userName, string folderName, FootprintFolderRequest request);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Put, UriTemplate = "/users/{userName}/{folderName}")]
        [Description("Modify existing footprint folder.")]
        void ModifyUserFootprintFolder(string userName, string folderName, FootprintFolderRequest request);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Delete, UriTemplate = "/users/{userName}/{folderName}")]
        [Description("Delete footprint folder.")]
        void DeleteUserFootprintFolder(string userName, string folderName);
        #endregion

        #region Footprint Operation Contracts
        [OperationContract]
        [WebGet(UriTemplate = "/users/{userName}/{folderName}/{footprintName}")]
        [Description("Returns the details of a footprint under an existing folder.")]
        FootprintResponse GetUserFootprint(string userName, string folderName, string footprintName);


        [OperationContract]
        [WebGet(UriTemplate = "/users/{userName}/{folderName}/{footprintName}/footprint")]
        [Description("Return the footprint of a footprint.")]
        string GetUserFootprintRegion(string userName, string folderName, string footprintName);

        [OperationContract]
        [WebGet(UriTemplate = "/users/{userName}/{folderName}/{footprintName}/footprint/outline")]
        [Description("Return the outline of a footprint.")]
        string GetUserFootprintRegionOutline(string userName, string folderName, string footprintName);

        [OperationContract]
        [WebGet(UriTemplate = "/users/{userName}/{folderName}/{footprintName}/footprint/outline/points?res={resolution}")]
        [Description("Return the points of the outline of a footprint.")]
        IEnumerable<Lib.EquatorialPoint> GetUserFootprintRegionOutlinePoints(string userName, string folderName, string footprintName, double resolution);

        [OperationContract]
        [WebGet(UriTemplate = "/users/{userName}/{folderName}/{footprintName}/footprint/outline/reduced?limit={limit}")]
        [Description("Returns the reduced outline of a folder footprint.")]
        string GetUserFootprintRegionReducedOutline(string userName, string folderName, string footprintName, double limit);

        [OperationContract]
        [WebGet(UriTemplate = "/users/{userName}/{folderName}/{footprintName}/footprint/outline/points/reduced?res={resolution}&limit={limit}")]
        [Description("Returns the points of the reduced outline of a footprint.")]
        IEnumerable<Lib.EquatorialPoint> GetUserFootprintRegionReducedOutlinePoints(string userName, string folderName, string footprintName, double resolution, double limit);

        [OperationContract]
        [WebGet(UriTemplate = "/users/{userName}/{folderName}/{footprintName}/footprint/convexhull")]
        [Description("Return the convex hull of the footprint.")]
        string GetUserFootprintRegionConvexHull(string userName, string folderName, string footprintName);

        [OperationContract]
        [WebGet(UriTemplate = "/users/{userName}/{folderName}/{footprintName}/footprint/convexhull/outline")]
        [Description("Return the outline of the convex hull of the footprint.")]
        string GetUserFootprintRegionConvexHullOutline(string userName, string folderName, string footprintName);

        [OperationContract]
        [WebGet(UriTemplate = "/users/{userName}/{folderName}/{footprintName}/footprint/convexhull/outline/points?res={resolution}")]
        [Description("Return the points of the outline of the convex hull of the footprint.")]
        IEnumerable<Lib.EquatorialPoint> GetUserFootprintRegionConvexHullOutlinePoints(string userName, string folderName, string footprintName, double resolution);

        [OperationContract]
        [WebGet(UriTemplate = "/users/{userName}/{folderName}/{footprintName}/plot?proj={projection}&width={width}&height={height}&degStyle={degStyle}&grid={grid}&autoZoom={autoZoom}&autoRotate={autoRotate}")]
        [Description(@"Plot Footprint. There are several keywords to costumize a plot:
            proj -- set projection. Available values: Aitoff, Equirectangular, HammerAitoff (deafult), Mollweide, Orthographic, Stereographic
            width -- set width of plot.
            height -- set height of plot.
            degStyle -- set the grid and label style. Available values: hms - hexagecimal, dms (default) - degree.
            grid -- turn grid on/off. Values = true, false (default)
            autoZoom -- turn auto zoom on/off. Values = true, false (default)
            autoRotate -- turn auto rotate on/off. Values = true, false (default)")]
        Stream GetUserFootprintPlot(string userName, string folderName, string footprintName, string projection, float width, float height, string degStyle, bool grid, bool autoZoom, bool autoRotate);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = "/users/{userName}/{folderName}/{footprintName}")]
        [Description("Create new footprint under an existing folder.")]
        void CreateUserFootprint(string userName, string folderName, string footprintName, FootprintRequest request);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Put, UriTemplate = "/users/{userName}/{folderName}/{footprintName}")]
        [Description("Modify footprint under an existing folder.")]
        void ModifyUserFootprint(string userName, string folderName, string footprintName, FootprintRequest request);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Delete, UriTemplate = "/users/{userName}/{folderName}/{footprintName}")]
        [Description("Delete footprint under an existing folder.")]
        void DeleteUserFootprint(string userName, string folderName, string footprintName);
        #endregion
    }

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
        public FootprintFolderListResponse GetUserFootprintFolderList(string userName)
        {
            IEnumerable<Lib.FootprintFolder> folders;

            using (var context = new Lib.Context())
            {
                var search = new Lib.FootprintFolderSearch(context);
                search.User = userName;
                search.Source = Lib.SearchSource.My;
                folders = search.Find();
            }

            var r = new FootprintFolderListResponse(folders);
            return r;
        }


        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public FootprintFolderResponse GetUserFootprintFolder(string userName, string folderName)
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

            var f = new FootprintFolder(folder);

            var list = footprints.Select(fp => new Footprint(fp, folderName).Url).ToArray();
            var r = new FootprintFolderResponse(f, list);
            return r;

        }


        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public string GetUserFootprintFolderRegion(string userName, string folderName)
        {
            var footprint = GetFolderFootprint(userName, folderName);
            var f = new Footprint(footprint, folderName);
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
        public void CreateUserFootprintFolder(string userName, string folderName, FootprintFolderRequest request)
        {
            using (var context = new Jhu.Footprint.Web.Lib.Context())
            {
                var folder = request.FootprintFolder.GetValue();
                folder.Context = context;
                folder.Save();
            }
        }

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public void ModifyUserFootprintFolder(string userName, string folderName, FootprintFolderRequest request)
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
        public FootprintResponse GetUserFootprint(string userName, string folderName, string footprintName)
        {
            var footprint = GetFootprint(userName, folderName, footprintName);
            var f = new Footprint(footprint, folderName);
            var r = new FootprintResponse(f);
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
        public void CreateUserFootprint(string userName, string folderName, string footprintName, FootprintRequest request)
        {
            using (var context = new Lib.Context())
            {
                var footprint = request.Footprint.GetValue();
                footprint.Context = context;
                footprint.Save();
            }
        }

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public void ModifyUserFootprint(string userName, string folderName, string footprintName, FootprintRequest request)
        {
            using (var context = new Lib.Context())
            {
                // TODO : REAL AUTHENTICATION
                context.User = userName;

                var footprint = request.Footprint.GetValue();
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
    }
}
