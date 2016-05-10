﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
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
        [WebInvoke(UriTemplate = "*", Method = "OPTIONS")]
        void HandleHttpOptionsRequest();

        #region Basic region operations

        [OperationContract]
        [WebGet(UriTemplate = "/reset")]
        [Description("Reset editor to an empty region")]
        void Reset();

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = "/new", BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Upload a new region")]
        void New(Stream stream);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = "/union", BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Union edited region with the posted one.")]
        void Union(Stream stream);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = "/intersect", BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Intersect edited region with the posted one.")]
        void Intersect(Stream stream);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = "/subtract", BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Subtract posted region from the edited one")]
        void Subtract(Stream stream);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = "/grow")]
        [Description("Grow edited region.")]
        void Grow(double arcmin);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = "/chull")]
        [Description("Compute the convex hull of the region.")]
        void CHull();

        #endregion
        #region Region load and save

        [OperationContract]
        [WebGet(UriTemplate = "/load?owner={owner}&name={name}&region={regionName}")]
        [Description("Load a region from the database")]
        void Load(string owner, string name, string regionName);

        [OperationContract]
        [WebGet(UriTemplate = "/save?owner={owner}&name={name}&region={regionName}")]
        [Description("Save a region to the database")]
        void Save(string owner, string name, string regionName);

        #endregion

        [OperationContract]
        [RegionFormatter]
        [WebGet(UriTemplate = "/shape?op={operation}", BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Returns the region.")]
        Spherical.Region GetShape(string operation);

        [OperationContract]
        [OutlineFormatter]
        [WebGet(UriTemplate = "/outline?op={operation}", BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Returns the outline of the region.")]
        Spherical.Outline GetOutline(string operation);

        [OperationContract]
        [TestJsonXmlFormat]
        [WebGet(UriTemplate = "/points?op={operation}&res={resolution}")]
        [Description("Returns the points of the outline of the region.")]
        IEnumerable<Lib.EquatorialPoint> GetOutlinePoints(string operation, double resolution);

        [OperationContract]
        [WebGet(UriTemplate = "/plot?op={operation}&proj={projection}&sys={sys}&ra={ra}&dec={dec}&b={b}&l={l}&width={width}&height={height}&theme={colorTheme}", BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Plots a footprint.")]
        Stream PlotUserFootprintRegion(
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
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = "/plot?op={operation}")]
        [Description("Plots a footprint.")]
        Stream PlotUserFootprintRegionAdvanced(string operation, Plot plot);

        // TODO: add HTM cover
    }
}
