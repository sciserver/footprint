﻿using System;
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
        [WebGet(UriTemplate = "/{owner}/{name}/")]
        [Description("Returns the header information of a footprint.")]
        FootprintResponse GetUserFootprint(string owner, string name);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = "/{owner}/{name}")]
        [Description("Create new footprint.")]
        FootprintResponse CreateUserFootprint(string owner, string name, FootprintRequest request);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Put, UriTemplate = "/{owner}/{name}")]
        [Description("Modify existing footprint.")]
        void ModifyUserFootprint(string owner, string name, FootprintRequest request);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Delete, UriTemplate = "/{owner}/{name}")]
        [Description("Delete footprint.")]
        void DeleteUserFootprint(string owner, string name);

        [OperationContract]
        [WebGet(UriTemplate = "/{owner}?name={name}&from={from}&max={max}")]
        [Description("Returns the list of footprints of the user.")]
        FootprintListResponse FindUserFootprints(string owner, string name, int from, int max);

        #endregion
        #region Footprint region CRUD operations

        [OperationContract]
        [WebGet(UriTemplate = "/{owner}/{name}/{regionName}")]
        [Description("Returns the header information of a region.")]
        FootprintRegionResponse GetUserFootprintRegion(string owner, string name, string regionName);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = "/{owner}/{name}/{regionName}")]
        [Description("Create new footprint under an existing folder.")]
        FootprintRegionResponse CreateUserFootprintRegion(string owner, string name, string regionName, FootprintRegionRequest request);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Put, UriTemplate = "/{owner}/{name}/{regionName}")]
        [Description("Modify footprint under an existing folder.")]
        void ModifyUserFootprintRegion(string owner, string name, string regionName, FootprintRegionRequest request);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Delete, UriTemplate = "/{owner}/{name}/{regionName}")]
        [Description("Delete footprint under an existing folder.")]
        void DeleteUserFootprintRegion(string owner, string name, string regionName);

        #endregion
        #region Footprint region and outline retrieval

        [OperationContract]
        [WebGet(UriTemplate = "/{owner}/{name}/region?op={operation}&limit={limit}")]
        [Description("Returns a footprint.")]
        string GetUserFootprintShape(string owner, string name, string operation, double limit);

        [OperationContract]
        [WebGet(UriTemplate = "/{owner}/{name}/outline?op={operation}&limit={limit}")]
        [Description("Returns the outline a footprint.")]
        string GetUserFootprintOutline(string owner, string name, string operation, double limit);

        [OperationContract]
        [WebGet(UriTemplate = "/{owner}/{name}/points?op={operation}&limit={limit}&res={resolution}")]
        [Description("Returns the points of the outline of a footprint.")]
        IEnumerable<Lib.EquatorialPoint> GetUserFootprintOutlinePoints(string owner, string name, string operation, double limit, double resolution);

        [OperationContract]
        [WebGet(UriTemplate = "/{owner}/{name}/plot?proj={projection}&width={width}&height={height}&degStyle={degStyle}&grid={grid}&autoZoom={autoZoom}&autoRotate={autoRotate}")]
        [Description(@"Plot Footprint. There are several keywords to costumize a plot:
            proj -- set projection. Available values: Aitoff, Equirectangular, HammerAitoff (deafult), Mollweide, Orthographic, Stereographic
            width -- set width of plot.
            height -- set height of plot.
            degStyle -- set the grid and label style. Available values: hms - hexagecimal, dms (default) - degree.
            grid -- turn grid on/off. Values = true, false (default)
            autoZoom -- turn auto zoom on/off. Values = true, false (default)
            autoRotate -- turn auto rotate on/off. Values = true, false (default)")]
        Stream GetUserFootprintPlot(string owner, string name, string projection, float width, float height, string degStyle, bool grid, bool autoZoom, bool autoRotate);

        #endregion
        #region Footprint Operation Contracts

        [OperationContract]
        [WebGet(UriTemplate = "/{owner}/{name}/{regionName}/region?op={operation}&limit={limit}")]
        [Description("Returns a region.")]
        string GetUserFootprintRegionShape(string owner, string name, string regionName, string operation, double limit);

        [OperationContract]
        [WebGet(UriTemplate = "/{owner}/{name}/{regionName}/outline?op={operation}&limit={limit}")]
        [Description("Returns the outline a region.")]
        string GetUserFootprintRegionOutline(string owner, string name, string regionName, string operation, double limit);

        [OperationContract]
        [WebGet(UriTemplate = "/{owner}/{name}/{regionName}/points?op={operation}&limit={limit}&res={resolution}")]
        [Description("Returns the points of the outline of a region.")]
        IEnumerable<Lib.EquatorialPoint> GetUserFootprintRegionOutlinePoints(string owner, string name, string regionName, string operation, double limit, double resolution);

        [OperationContract]
        [WebGet(UriTemplate = "/users/{owner}/{name}/{regionName}/plot?proj={projection}&width={width}&height={height}&degStyle={degStyle}&grid={grid}&autoZoom={autoZoom}&autoRotate={autoRotate}")]
        [Description(@"Plot Footprint. There are several keywords to costumize a plot:
            proj -- set projection. Available values: Aitoff, Equirectangular, HammerAitoff (deafult), Mollweide, Orthographic, Stereographic
            width -- set width of plot.
            height -- set height of plot.
            degStyle -- set the grid and label style. Available values: hms - hexagecimal, dms (default) - degree.
            grid -- turn grid on/off. Values = true, false (default)
            autoZoom -- turn auto zoom on/off. Values = true, false (default)
            autoRotate -- turn auto rotate on/off. Values = true, false (default)")]
        Stream GetUserFootprintRegionPlot(string owner, string name, string regionName, string projection, float width, float height, string degStyle, bool grid, bool autoZoom, bool autoRotate);

        #endregion
    }
}