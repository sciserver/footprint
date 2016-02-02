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
    public interface IFootprintService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "*", Method = "OPTIONS")]
        void HandleHttpOptionsRequest();

        [OperationContract]
        [WebGet(UriTemplate = "/users/{userName}/footprints/{folderName}/{footprintName}")]
        [Description("Load existing footprint under an existing folder.")]
        FootprintListResponse GetUserFootprint(string userName, string folderName, string footprintName);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = "/users/{userName}/footprints/{folderName}/{footprintName}")]
        [Description("Create a new footprint under an existing folder.")]
        void CreateUserFootprint(string userName, string folderName, string footprintName, FootprintRequest footprint);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Put, UriTemplate = "/users/{userName}/footprints/{folderName}/{footprintName}")]
        [Description("Modify an existing footprint under an existing folder.")]
        void ModifyUserFootprint(string userName, string folderName, string footprintName, FootprintRequest footprint);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Delete, UriTemplate = "/users/{userName}/footprints/{folderName}/{footprintName}")]
        [Description("Delete footprint under an existing folder.")]
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

        [PrincipalPermission(SecurityAction.Assert, Authenticated=true)]
        public FootprintListResponse GetUserFootprint(string userName, string folderName, string footprintName)
        {
            Jhu.Footprint.Web.Lib.Footprint footprint;
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
        public void CreateUserFootprint(string userName, string folderName, string footprintName, FootprintRequest request) 
        {
            using (var context = new Jhu.Footprint.Web.Lib.Context())
            {
                var footprint = request.Footprint.GetValue();
                footprint.Context = context;
                footprint.Create();
            }
        }

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public void ModifyUserFootprint(string userName, string folderName, string footprintName, FootprintRequest request)
        {
            using (var context = new Jhu.Footprint.Web.Lib.Context())
            {
                var footprint = request.Footprint.GetValue();
                footprint.Context = context;
                footprint.Modify();
            
            }
        }

        [PrincipalPermission(SecurityAction.Assert, Authenticated = true)]
        public void DeleteUserFootprint(string userName, string folderName, string footprintName)
        {
            using (var context = new Jhu.Footprint.Web.Lib.Context())
            {
                var footprint = new Jhu.Footprint.Web.Lib.Footprint(context);
                footprint.Name = footprintName;
                footprint.FolderName = folderName;
                footprint.User = userName;

                footprint.Delete();
            }
        }
    }
}
