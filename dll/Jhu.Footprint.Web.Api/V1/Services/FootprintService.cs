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

        [OperationContract]
        [WebGet(UriTemplate = "/users/{userName}/{folderName}/")]
        [Description("Returns the details of an existing folder and the list of the footprints in it.")]
        FootprintFolderListResponse GetUserFootprintFolder(string userName, string folderName);

        [OperationContract]
        [WebGet(UriTemplate = "/users/{userName}/{folderName}/{footprintName}")]
        [Description("Returns the details of a footprint under an existing folder.")]
        FootprintListResponse GetUserFootprint(string userName, string folderName, string footprintName);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = "/users/{userName}/footprints/{folderName}")]
        [Description("Create a new footprint folder.")]
        void CreateUserFootprintFolder(string userName, string folderName, FootprintFolderRequest request);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = "/users/{userName}/{folderName}/{footprintName}")]
        [Description("Create a new footprint under an existing folder.")]
        void CreateUserFootprint(string userName, string folderName, string footprintName, FootprintRequest request);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Put, UriTemplate = "/users/{userName}/footprints/{folderName}")]
        [Description("Modify an existing footprint folder.")]
        void ModifyUserFootprintFolder(string userName, string folderName, FootprintFolderRequest request);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Put, UriTemplate = "/users/{userName}/{folderName}/{footprintName}")]
        [Description("Modify an existing folder or footprint under an existing folder.")]
        void ModifyUserFootprint(string userName, string folderName, string footprintName, FootprintRequest request);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Delete, UriTemplate = "/users/{userName}/footprints/{folderName}")]
        [Description("Delete footprint folder.")]
        void DeleteUserFootprintFolder(string userName, string folderName);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Delete, UriTemplate = "/users/{userName}/{folderName}/{footprintName}")]
        [Description("Delete folder or footprint under an existing folder.")]
        void DeleteUserFootprint(string userName, string folderName, string footprintName);
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

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public FootprintFolderListResponse GetUserFootprintFolder(string userName, string folderName)
        {
            Jhu.Footprint.Web.Lib.FootprintFolder folder;
            using (var context = new Lib.Context())
            {
                folder = new Lib.FootprintFolder(context);
                folder.User = userName;
                folder.Name = folderName;
                folder.Load();
                // load footprint folder
            }

            var f = new FootprintFolder(folder);
            var r = new FootprintFolderListResponse(f);
            return r;

        }

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public FootprintListResponse GetUserFootprint(string userName, string folderName, string footprintName)
        {
            Lib.Footprint footprint;
            using (var context = new Lib.Context())
            {
                footprint = new Lib.Footprint(context);
                footprint.User = userName;
                footprint.FolderName = folderName;
                footprint.Name = footprintName;
                footprint.Load();
                // load footprint
            }

            var f = new Footprint(footprint);
            var r = new FootprintListResponse(f);
            return r;
        }

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public void CreateUserFootprintFolder(string userName, string folderName, FootprintFolderRequest request)
        {
            using (var context = new Jhu.Footprint.Web.Lib.Context())
            {
                var folder = request.FootprintFolder.GetValue();
                folder.Context = context;
                folder.Create();
            }
        }

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public void CreateUserFootprint(string userName, string folderName, string footprintName, FootprintRequest request)
        {
            using (var context = new Lib.Context())
            {
                var footprint = request.Footprint.GetValue();
                footprint.Context = context;
                footprint.Create();
            }
        }

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public void ModifyUserFootprintFolder(string userName, string folderName, FootprintFolderRequest request)
        {
            using (var context = new Jhu.Footprint.Web.Lib.Context())
            {
                var folder = request.FootprintFolder.GetValue();
                folder.Context = context;
                folder.Modify();

            }
        }

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public void ModifyUserFootprint(string userName, string folderName, string footprintName, FootprintRequest request)
        {
            using (var context = new Lib.Context())
            {
                var footprint = request.Footprint.GetValue();
                footprint.Context = context;
                footprint.Modify();
            }
        }

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public void DeleteUserFootprintFolder(string userName, string folderName)
        {
            using (var context = new Jhu.Footprint.Web.Lib.Context())
            {
                var folder = new Jhu.Footprint.Web.Lib.FootprintFolder(context);
                folder.Name = folderName;
                folder.User = userName;

                folder.Delete();
            }
        }

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public void DeleteUserFootprint(string userName, string folderName, string footprintName)
        {
            using (var context = new Lib.Context())
            {
                var footprint = new Lib.Footprint(context);
                footprint.Name = footprintName;
                footprint.FolderName = folderName;
                footprint.User = userName;
                footprint.Delete();
            }
        }
    }
}
