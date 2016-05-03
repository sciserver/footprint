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
    public interface IEditorService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/new")]
        void New();

        [OperationContract]
        [DynamicFormat(typeof(RegionMessageFormatter))]
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = "/new", BodyStyle = WebMessageBodyStyle.Bare)]
        void NewRegion(Spherical.Region region);

        /*
        [WebGet(UriTemplate = "/load")]
        void Load(string owner, string footprintName);

        [WebGet(UriTemplate = "/load")]
        void Load(string owner, string footprintName, string regionName);

        [WebGet(UriTemplate = "/save")]
        void Load(string owner, string footprintName, string regionName);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = "/union")]
        void Union();

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = "/intersect")]
        void Intersect();

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = "/subtract")]
        void Subtract();

        [WebInvoke(Method = HttpMethod.Post, UriTemplate = "/grow")]
        void Grow(double arcmin);

        */

        [OperationContract]
        [DynamicFormat(typeof(RegionMessageFormatter))]
        [WebGet(UriTemplate = "/region", BodyStyle = WebMessageBodyStyle.Bare)]
        Spherical.Region GetRegion();

        /*
        [WebGet(UriTemplate = "/outline")]
        Stream GetOutline();

        [WebGet(UriTemplate = "/plot")]
        Stream Plot();

        */

    }
}
