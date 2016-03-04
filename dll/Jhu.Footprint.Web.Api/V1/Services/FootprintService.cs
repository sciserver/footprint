using System;
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
        [Description("Returns a list of all user created folders.")]
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
        [Description("Returns the outline of a footprint.")]
        string GetUserFootprintFolderRegionOutline(string userName, string folderName);
        
        [OperationContract]
        [WebGet(UriTemplate = "/users/{userName}/{folderName}/footprint/outline/points?res={resolution}")]
        [Description("Returns the points of the outline of a footprint.")]
        IEnumerable<Lib.Point> GetUserFootprintFolderRegionOutlinePoints(string userName, string folderName, double resolution);
        
        [OperationContract]
        [WebGet(UriTemplate = "/users/{userName}/{folderName}/footprint/convexhull")]
        [Description("Returns the convex hull of the folder footprint.")]
        string GetUserFootprintFolderRegionConvexHull(string userName, string folderName);


        [OperationContract]
        [WebGet(UriTemplate = "/users/{userName}/{folderName}/footprint/convexhull/outline")]
        [Description("Returns the outline of the convex hull of the folder footprint.")]
        string GetUserFootprintFolderRegionConvexHullOutline(string userName, string folderName);

        [OperationContract]
        [WebGet(UriTemplate = "/users/{userName}/{folderName}/footprint/convexhull/outline/points?res={resolution}")]
        [Description("Returns the points of the outline of the convex hull of the folder footprint.")]
        IEnumerable<Lib.Point> GetUserFootprintFolderRegionConvexHullOutlinePoints(string userName, string folderName, double resolution);

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
        [Description("Returns the footprint of a footprint.")]
        string GetUserFootprintRegion(string userName, string folderName, string footprintName);

        [OperationContract]
        [WebGet(UriTemplate = "/users/{userName}/{folderName}/{footprintName}/footprint/outline")]
        [Description("Returns the points of the outline of a footprint.")]
        string GetUserFootprintRegionOutline(string userName, string folderName, string footprintName);

        [OperationContract]
        [WebGet(UriTemplate = "/users/{userName}/{folderName}/{footprintName}/footprint/outline/points?res={resolution}")]
        [Description("Returns the outline of a footprint.")]
        IEnumerable<Lib.Point> GetUserFootprintRegionOutlinePoints(string userName, string folderName, string footprintName, double resolution);
        
        [OperationContract]
        [WebGet(UriTemplate = "/users/{userName}/{folderName}/{footprintName}/footprint/convexhull")]
        [Description("Returns the convex hull of the footprint.")]
        string GetUserFootprintRegionConvexHull(string userName, string folderName, string footprintName);

        [OperationContract]
        [WebGet(UriTemplate = "/users/{userName}/{folderName}/{footprintName}/footprint/convexhull/outline")]
        [Description("Returns the outline of the convex hull of the footprint.")]
        string GetUserFootprintRegionConvexHullOutline(string userName, string folderName, string footprintName);

        [OperationContract]
        [WebGet(UriTemplate = "/users/{userName}/{folderName}/{footprintName}/footprint/convexhull/outline/points?res={resolution}")]
        [Description("Returns the points of the outline of the convex hull of the footprint.")]
        IEnumerable<Lib.Point> GetUserFootprintRegionConvexHullOutlinePoints(string userName, string folderName, string footprintName, double resolution);

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
                folder.User = userName;
                folder.Name = folderName;
                folder.Id = -1;
                folder.Load();

                var footprint = new Lib.Footprint(context);
                footprint.Id = folder.FootprintId;
                footprint.User = userName;
                footprint.Load();

                return footprint;
            }
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
                folder.User = userName;
                folder.Name = folderName;
                folder.Id = -1; 
                folder.Load();

                //get footprints from folder
                var search = new Lib.FootprintSearch(context) { User = folder.User, FolderId = folder.Id };
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
            footprint.Region.Simplify();
            return footprint.Region.Outline.ToString();
        }

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public string GetUserFootprintFolderRegionConvexHull(string userName, string folderName)
        {
            var fp = GetFolderFootprint(userName, folderName);
            fp.Region.Simplify();

            var chull = fp.Region.Outline.GetConvexHull();
            chull.Simplify();

            return chull.Outline.ToString();
        }

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public string GetUserFootprintFolderRegionConvexHullOutline(string userName, string folderName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Lib.Point> GetUserFootprintFolderRegionConvexHullOutlinePoints(string userName, string folderName, double resolution)
        {
            throw new NotImplementedException();
        }

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public IEnumerable<Lib.Point> GetUserFootprintFolderRegionOutlinePoints(string userName, string folderName, double resolution)
        {
            var footprint = GetFolderFootprint(userName, folderName);
            footprint.Region.Simplify();
            return Lib.FootprintFormatter.InterpolateOutlinePoints(footprint.Region.Outline, resolution);
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
                folder.User = userName;
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
            fp.Region.Simplify();
            return fp.Region.Outline.ToString();
        }

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public IEnumerable<Lib.Point> GetUserFootprintRegionOutlinePoints(string userName, string folderName, string footprintName, double resolution)
        {
            var fp = GetFootprint(userName, folderName, footprintName);
            fp.Region.Simplify();
            return Lib.FootprintFormatter.InterpolateOutlinePoints(fp.Region.Outline, resolution);
        }

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public string GetUserFootprintRegionConvexHull(string userName, string folderName, string footprintName)
        {
            throw new NotImplementedException();
        }

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public string GetUserFootprintRegionConvexHullOutline(string userName, string folderName, string footprintName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Lib.Point> GetUserFootprintRegionConvexHullOutlinePoints(string userName, string folderName, string footprintName, double resolution)
        {
            throw new NotImplementedException();
        }

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
    }
}
