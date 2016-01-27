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
        [Description("TODO")]
        FootprintListResponse GetUserFootprint(string userName, string folderName, string footprintName);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "/users/{userName}/footprints/{folderName}/{footprintName}")]
        [Description("Create a new footprint under an existing folder.")]
        void CreateUserFootprint(string userName, string folderName, string footprintName, FootprintRequest footprint);
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
        public void CreateUserFootprint(string userName, string folderName, string footprintName, FootprintRequest footprint) 
        {
            throw new NotImplementedException();
        }
    }
}
