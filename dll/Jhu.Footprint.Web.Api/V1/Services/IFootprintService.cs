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
        [WebGet(UriTemplate = "/users/{owner}/footprints/{name}/")]
        [Description("Returns the header information of a footprint.")]
        FootprintResponse GetUserFootprint(string owner, string name);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = "/users/{owner}/footprints/{name}")]
        [Description("Create new footprint.")]
        FootprintResponse CreateUserFootprint(string owner, string name, FootprintRequest request);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Patch, UriTemplate = "/users/{owner}/footprints/{name}")]
        [Description("Modify existing footprint.")]
        FootprintResponse ModifyUserFootprint(string owner, string name, FootprintRequest request);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Delete, UriTemplate = "/users/{owner}/footprints/{name}")]
        [Description("Delete footprint.")]
        void DeleteUserFootprint(string owner, string name);

        #endregion
        #region Footprint search operations

        // TODO: expose additional search criteria
        [OperationContract]
        [WebGet(UriTemplate = "/users/{owner}/footprints?name={name}&from={from}&max={max}", BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Returns the list of footprints of the user.")]
        FootprintListResponse FindUserFootprints(string owner, string name, int from, int max);

        // TODO: expose additional search criteria
        [OperationContract]
        [StreamingListFormat]
        [WebGet(UriTemplate = "/footprints?owner={owner}&name={name}&from={from}&max={max}", BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Returns the list of footprints of the user.")]
        FootprintListResponse FindFootprints(string owner, string name, int from, int max);

        // TODO: implement detailed search with POST

        #endregion
        #region Footprint region CRUD operations

        [OperationContract]
        [WebGet(UriTemplate = "/users/{owner}/footprints/{name}/regions/{regionName}")]
        [Description("Returns the header information of a region.")]
        FootprintRegionResponse GetUserFootprintRegion(string owner, string name, string regionName);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = "/users/{owner}/footprints/{name}/regions/{regionName}")]
        [Description("Create new footprint under an existing folder.")]
        FootprintRegionResponse CreateUserFootprintRegion(string owner, string name, string regionName, FootprintRegionRequest request);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Put, UriTemplate = "/users/{owner}/footprints/{name}/regions/{regionName}")]
        [Description("Modify footprint under an existing folder.")]
        FootprintRegionResponse ModifyUserFootprintRegion(string owner, string name, string regionName, FootprintRegionRequest request);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Delete, UriTemplate = "/users/{owner}/footprints/{name}/regions/{regionName}")]
        [Description("Delete footprint under an existing folder.")]
        void DeleteUserFootprintRegion(string owner, string name, string regionName);

        // TODO: add HTM cover

        #endregion
        #region Footprint combined region get and plot

        [OperationContract]
        [RegionFormatter]
        [WebGet(UriTemplate = "/users/{owner}/footprints/{name}/shape?op={operation}", BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Returns the shape description of a footprint.")]
        Spherical.Region GetUserFootprintShape(string owner, string name, string operation);

        [OperationContract]
        [OutlineFormatter]
        [WebGet(UriTemplate = "/users/{owner}/footprints/{name}/outline?op={operation}", BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Returns the outline a footprint.")]
        Spherical.Outline GetUserFootprintOutline(string owner, string name, string operation);

        [OperationContract]
        [TestJsonXmlFormat]
        [WebGet(UriTemplate = "/users/{owner}/footprints/{name}/outline/points?op={operation}&res={resolution}")]
        [Description("Returns the points of the outline of a footprint.")]
        IEnumerable<Lib.EquatorialPoint> GetUserFootprintOutlinePoints(string owner, string name, string operation, double resolution);

        [OperationContract]
        [WebGet(UriTemplate = "/users/{owner}/footprints/{name}/plot?op={operation}&proj={projection}&sys={sys}&ra={ra}&dec={dec}&b={b}&l={l}&width={width}&height={height}&theme={colorTheme}", BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Returns the points of the outline of a footprint.")]
        Stream PlotUserFootprint(
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
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = "/users/{owner}/footprints/{name}/plot?op={operation}")]
        Stream PlotUserFootprintAdvanced(string owner, string name, string operation, Plot plot);

        #endregion
        #region Individual region get and plot

        [OperationContract]
        [RegionFormatter]
        [WebGet(UriTemplate = "/users/{owner}/footprints/{name}/regions/{regionName}/shape?op={operation}", BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Returns the shape description of a footprint.")]
        Spherical.Region GetUserFootprintRegionShape(string owner, string name, string regionName, string operation);

        [OperationContract]
        [OutlineFormatter]
        [WebGet(UriTemplate = "/users/{owner}/footprints/{name}/regions/{regionName}/outline?op={operation}", BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Returns the outline a footprint.")]
        Spherical.Outline GetUserFootprintRegionOutline(string owner, string name, string regionName, string operation);

        [OperationContract]
        [TestJsonXmlFormat]
        [WebGet(UriTemplate = "/users/{owner}/footprints/{name}/regions/{regionName}/outline/points?op={operation}&res={resolution}")]
        [Description("Returns the points of the outline of a footprint.")]
        IEnumerable<Lib.EquatorialPoint> GetUserFootprintRegionOutlinePoints(string owner, string name, string regionName, string operation, double resolution);

        [OperationContract]
        [WebGet(UriTemplate = "/users/{owner}/footprints/{name}/regions/{regionName}/plot?op={operation}&proj={projection}&sys={sys}&ra={ra}&dec={dec}&b={b}&l={l}&width={width}&height={height}&theme={colorTheme}", BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Plots a footprint.")]
        Stream PlotUserFootprintRegion(
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
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = "/users/{owner}/footprints/{name}/regions/{regionName}/plot?op={operation}")]
        [Description("Plots a footprint.")]
        Stream PlotUserFootprintRegionAdvanced(string owner, string name, string regionName, string operation, Plot plot);

        // TODO: add HTM cover

        #endregion
    }
}
