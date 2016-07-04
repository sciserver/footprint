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

        #region Footprint CRUD operations

        [OperationContract]
        [WebGet(UriTemplate = Urls.UserFootprint)]
        [Description("Returns the header information of a footprint.")]
        FootprintResponse GetUserFootprint(string owner, string name);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = Urls.UserFootprint)]
        [Description("Create new footprint.")]
        FootprintResponse CreateUserFootprint(string owner, string name, FootprintRequest request);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Patch, UriTemplate = Urls.UserFootprint)]
        [Description("Modify existing footprint.")]
        FootprintResponse ModifyUserFootprint(string owner, string name, FootprintRequest request);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Delete, UriTemplate = Urls.UserFootprint)]
        [Description("Delete footprint.")]
        void DeleteUserFootprint(string owner, string name);

        #endregion
        #region Footprint search operations

        // TODO: expose additional search criteria
        [OperationContract]
        [WebGet(UriTemplate = Urls.UserFootprints + "?" + Urls.FootprintSearchParams, BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Returns the list of footprints of the user.")]
        FootprintListResponse FindUserFootprints(string owner, string name, int from, int max);

        // TODO: expose additional search criteria
        [OperationContract]
        [StreamingListFormat]
        [WebGet(UriTemplate = Urls.AllFootprints + "?" + Urls.OwnerSearchParam + "&" + Urls.FootprintSearchParams, BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Returns the list of footprints of the user.")]
        FootprintListResponse FindFootprints(string owner, string name, int from, int max);

        // TODO: implement detailed search with POST


        [OperationContract]
        [WebGet(UriTemplate = Urls.UserFootprintRegions + "?" + Urls.RegionSearchParams, BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Returns the list of the regions of a footprint.")]
        FootprintRegionListResponse FindUserFootprintRegions(string owner, string name, string regionName, int from, int max);

        #endregion
        #region Footprint region CRUD operations

        [OperationContract]
        [WebGet(UriTemplate = Urls.UserFootprintRegion)]
        [Description("Returns the header information of a region.")]
        FootprintRegionResponse GetUserFootprintRegion(string owner, string name, string regionName);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = Urls.UserFootprintRegion)]
        [Description("Create new region under an existing footprint.")]
        FootprintRegionResponse CreateUserFootprintRegion(string owner, string name, string regionName, FootprintRegionRequest request);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Put, UriTemplate = Urls.UserFootprintRegion)]
        [Description("Modify a region under an existing footprint.")]
        FootprintRegionResponse ModifyUserFootprintRegion(string owner, string name, string regionName, FootprintRegionRequest request);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Delete, UriTemplate = Urls.UserFootprintRegion)]
        [Description("Delete a region under an existing footprint.")]
        void DeleteUserFootprintRegion(string owner, string name, string regionName);

        // TODO: add HTM cover

        #endregion
        #region Footprint combined region get and plot

        [OperationContract]
        [RegionFormatter]
        [WebGet(UriTemplate = Urls.UserFootprint + Urls.Shape, BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Returns the shape description of a footprint.")]
        Spherical.Region GetUserFootprintShape(string owner, string name);

        [OperationContract]
        [OutlineFormatter]
        [WebGet(UriTemplate = Urls.UserFootprint + Urls.Outline, BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Returns the outline a footprint.")]
        Spherical.Outline GetUserFootprintOutline(string owner, string name);

        [OperationContract]
        [TestJsonXmlFormat]
        [WebGet(UriTemplate = Urls.UserFootprint + Urls.OutlinePoints)]
        [Description("Returns the points of the outline of a footprint.")]
        IEnumerable<Lib.EquatorialPoint> GetUserFootprintOutlinePoints(string owner, string name, double resolution);

        [OperationContract]
        [PlotFormatter]
        [WebGet(UriTemplate = Urls.UserFootprint + Urls.Plot, BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Returns the points of the outline of a footprint.")]
        Spherical.Visualizer.Plot PlotUserFootprint(
            string owner,
            string name,
            string operation,
            string projection,
            string sys,
            string ra,
            string dec,
            string b,
            string l,
            float width,
            float height,
            string colorTheme);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = Urls.UserFootprint + Urls.PlotAdvanced)]
        Spherical.Visualizer.Plot PlotUserFootprintAdvanced(string owner, string name, string operation, Plot plotParameters);

        #endregion
        #region Individual region set, get and plot

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = Urls.UserFootprintRegion + Urls.Shape, BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Upload region shape binary or other representation")]
        void SetUserFootprintRegionShape(string owner, string name, string regionName, Stream stream);

        [OperationContract]
        [RegionFormatter]
        [WebGet(UriTemplate = Urls.UserFootprintRegion + Urls.Shape, BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Returns the shape description of a footprint.")]
        Spherical.Region GetUserFootprintRegionShape(string owner, string name, string regionName);

        [OperationContract]
        [OutlineFormatter]
        [WebGet(UriTemplate = Urls.UserFootprintRegion + Urls.Outline, BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Returns the outline a footprint.")]
        Spherical.Outline GetUserFootprintRegionOutline(string owner, string name, string regionName);

        [OperationContract]
        [TestJsonXmlFormat]
        [WebGet(UriTemplate = Urls.UserFootprintRegion + Urls.OutlinePoints)]
        [Description("Returns the points of the outline of a footprint.")]
        IEnumerable<Lib.EquatorialPoint> GetUserFootprintRegionOutlinePoints(string owner, string name, string regionName, double resolution);

        [OperationContract]
        [WebGet(UriTemplate = Urls.UserFootprintRegion + Urls.Plot, BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Plots a footprint.")]
        Spherical.Visualizer.Plot PlotUserFootprintRegion(
            string owner,
            string name,
            string regionName,
            string operation,
            string projection,
            string sys,
            string ra,
            string dec,
            string b,
            string l,
            float width,
            float height,
            string colorTheme);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = Urls.UserFootprintRegion + Urls.PlotAdvanced)]
        [Description("Plots a footprint.")]
        Spherical.Visualizer.Plot PlotUserFootprintRegionAdvanced(string owner, string name, string regionName, string operation, Plot plotParameters);

        // TODO: add HTM cover

        #endregion
    }
}
