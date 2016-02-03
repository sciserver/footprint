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

namespace Jhu.Footprint.Web.Api.V1
{
    [ServiceContract]
    [Description("TODO")]
    public interface IFootprintFolderService
    { 
        [OperationContract]
        [WebInvoke(UriTemplate = "*", Method = "OPTIONS")]
        void HandleHttpOptionsRequest();

        [OperationContract]
        [WebGet(UriTemplate = "/users/{userName}/footprints/{folderName}")]
        [Description("Load existing footprint folder.")]
        FootprintFolderListResponse GetUserFootprintFolder(string userName, string folderName);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = "/users/{userName}/footprints/{folderName}")]
        [Description("Create a new footprint folder.")]
        void CreateUserFootprintFolder(string userName, string folderName, FootprintFolderRequest footprint);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Put, UriTemplate = "/users/{userName}/footprints/{folderName}")]
        [Description("Modify an existing footprint folder.")]
        void ModifyUserFootprintFolder(string userName, string folderName, FootprintFolderRequest footprint);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Delete, UriTemplate = "/users/{userName}/footprints/{folderName}")]
        [Description("Delete footprint folder.")]
        void DeleteUserFootprintFolder(string userName, string folderName);
    }
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    [RestServiceBehavior]
    public class FootprintFolderService : RestServiceBase, IFootprintFolderService
    {
        #region Constructors and initializers

        public FootprintFolderService()
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
    }
}
