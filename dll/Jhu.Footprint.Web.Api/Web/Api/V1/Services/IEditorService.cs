using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.ServiceModel;
using System.ServiceModel.Web;
using Jhu.Graywulf.Web.Services;
using Jhu.Graywulf.Web.Services.Serialization;
using Lib = Jhu.Footprint.Web.Lib;

namespace Jhu.Footprint.Web.Api.V1
{
    [ServiceContract]
    [ServiceName(Name = "Editor", Version = "V1")]
    [Description("Create and edit observation footprints.")]
    public interface IEditorService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "*", Method = "OPTIONS")]
        void HandleHttpOptionsRequest();

        [OperationContract]
        [WebInvoke(UriTemplate = "/proxy.{lang}", Method = HttpMethod.Get)]
        Stream GenerateProxy(string lang);

        #region Footprint CRUD operations

        [OperationContract]
        [WebGet(UriTemplate = Urls.EditorFootprint)]
        [Description("Returns the header information of the edited footprint")]
        [return: Description("An object conveying information on the footprint in the editor.")]
        FootprintResponse GetFootprint();

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Patch, UriTemplate = Urls.EditorFootprint)]
        [Description("Modified the properties of the footprint in the editor.")]
        [return: Description("An object conveying information on the modified footprint.")]
        FootprintResponse ModifyFootprint(
            [Description("An object conveyings the footprint properties to be updated")]
            FootprintRequest footprint);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Delete, UriTemplate = Urls.EditorFootprint)]
        [Description("Delete footprint and reset the editor.")]
        void DeleteFootprint();

        [OperationContract]
        [RawFormat(typeof(RegionFormatter))]
        [WebGet(UriTemplate = Urls.EditorFootprint + Urls.Raw)]
        [Description("Returns the footprint in raw format text or binary.")]
        [return: Description("The combined footprint in string or binary representation.")]
        Spherical.Region DownloadFootprint();

        // TODO: outline as xml or json?

        [OperationContract]
        [RawFormat(typeof(OutlineFormatter))]
        [WebGet(UriTemplate = Urls.EditorFootprint + Urls.Outline + Urls.Raw, BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Returns the outline of the footprint.")]
        [return: Description("The outline of the combined footprint in string representation.")]
        Spherical.Outline DownloadFootprintOutline();

        [OperationContract]
        [TextJsonXmlFormat]
        [WebGet(UriTemplate = Urls.EditorFootprint + Urls.OutlinePoints)]
        [Description("Returns the points of the outline of the footprint.")]
        [return: Description("The points of the outline of the combined footprint.")]
        IEnumerable<Point> GetFootprintOutlinePoints(
            [Description("Coordinate system of the points")]
            CoordinateSystem? sys,
            [Description("Representation of the coordinates")]
            CoordinateRepresentation? rep,
            [Description("Sampling resolution in arc sec")]
            double? resolution,
            [Description("Outline complexity reduction method")]
            OutlineReduction? reduce,
            [Description("Outline complexity reduction limit in arc min")]
            double? limit);

        [OperationContract]
        [RawFormat(typeof(PlotFormatter))]
        [WebGet(UriTemplate = Urls.EditorFootprint + Urls.Plot + Urls.PlotDetails, BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Plots the footprint")]
        [return: Description("The plot as an image in various formats.")]
        Spherical.Visualizer.Plot PlotFootprint(
            [Description("Projection")]
            Projection? projection,
            [Description("Coordinate system")]
            CoordinateSystem? sys,
            [Description("Center longitude")]
            double? lon,
            [Description("Center latitude")]
            double? lat,
            [Description("Image width")]
            float? width,
            [Description("Image height")]
            float? height,
            [Description("Color theme")]
            ColorTheme? colorTheme,
            [Description("Zoom in on the region automatically.")]
            bool? autoZoom,
            [Description("Rotate region to the origin of projection automatically")]
            bool? autoRotate,
            [Description("Plot grid")]
            bool? grid,
            [Description("Format of numeric values")]
            DegreeStyle? degreeStyle);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = Urls.EditorFootprint + Urls.Plot, BodyStyle = WebMessageBodyStyle.Bare)]
        [RawFormat(typeof(PlotFormatter))]
        [Description("Plots the footprint, with advanced parameters")]
        [return: Description("The plot as an image in various formats.")]
        Spherical.Visualizer.Plot PlotFootprintAdvanced(
            [Description("Detailed parameters of the plot")]
            PlotRequest plot);

        #endregion
        #region Footprint region CRUD operations

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Put, UriTemplate = Urls.EditorRegion)]
        [Description("Create a new region.")]
        RegionResponse CreateRegion(string regionName, RegionRequest request);

        [OperationContract]
        [WebGet(UriTemplate = Urls.EditorRegion)]
        [Description("Returns the header information of a region.")]
        RegionResponse GetRegion(string regionName);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Patch, UriTemplate = Urls.EditorRegion)]
        [Description("Modify a region.")]
        RegionResponse ModifyRegion(string regionName, RegionRequest request);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Delete, UriTemplate = Urls.EditorRegion)]
        [Description("Delete a region.")]
        void DeleteRegion(string regionName);

        [OperationContract]
        [WebGet(UriTemplate = Urls.EditorRegionSearch + Urls.Paging)]
        [Description("List regions.")]
        RegionListResponse ListRegions(
            [Description("Name pattern")]
            string regionName,
            [Description("List items from")]
            int? from,
            [Description("Max number of items")]
            int? max);

        [OperationContract]
        [RawFormat(typeof(RegionFormatter))]
        [WebGet(UriTemplate = Urls.EditorRegion + Urls.Raw, BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Returns the shape description of the footprint region.")]
        Spherical.Region DownloadRegion(string regionName);

        [OperationContract]
        [RawFormat(typeof(RegionFormatter))]
        [WebInvoke(Method = HttpMethod.Put, UriTemplate = Urls.EditorRegion + Urls.Raw, BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Upload a region binary or other representation")]
        [return: Description("The header information of the newly created region.")]
        RegionResponse UploadRegion(string regionName, Spherical.Region region);

        [OperationContract]
        [RawFormat(typeof(OutlineFormatter))]
        [WebGet(UriTemplate = Urls.EditorRegion + Urls.Outline + Urls.Raw, BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Returns the outline of the footprint.")]
        Spherical.Outline DownloadRegionOutline(string regionName);

        [OperationContract]
        [TextJsonXmlFormat]
        [WebGet(UriTemplate = Urls.EditorRegion + Urls.OutlinePoints)]
        [Description("Returns the points of the outline of the region.")]
        [return: Description("The points of the outline of the region.")]
        IEnumerable<Point> GetRegionOutlinePoints(
            string regionName,
            [Description("Coordinate system of the points")]
            CoordinateSystem? sys,
            [Description("Representation of the coordinates")]
            CoordinateRepresentation? rep,
            [Description("Sampling resolution in arc sec")]
            double? resolution,
            [Description("Outline complexity reduction method")]
            OutlineReduction? reduce,
            [Description("Outline complexity reduction limit in arc min")]
            double? limit);

        [OperationContract]
        [RawFormat(typeof(PlotFormatter))]
        [WebGet(UriTemplate = Urls.EditorRegion + Urls.Plot + Urls.PlotDetails, BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Plots the region")]
        [return: Description("The plot as an image in various formats.")]
        Spherical.Visualizer.Plot PlotRegion(
            string regionName,
            [Description("Projection")]
            Projection? projection,
            [Description("Coordinate system")]
            CoordinateSystem? sys,
            [Description("Center longitude")]
            double? lon,
            [Description("Center latitude")]
            double? lat,
            [Description("Image width")]
            float? width,
            [Description("Image height")]
            float? height,
            [Description("Color theme")]
            ColorTheme? colorTheme,
            [Description("Zoom in on the region automatically.")]
            bool? autoZoom,
            [Description("Rotate region to the origin of projection automatically")]
            bool? autoRotate,
            [Description("Plot grid")]
            bool? grid,
            [Description("Format of numeric values")]
            DegreeStyle? degreeStyle);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = Urls.EditorRegion + Urls.Plot, BodyStyle = WebMessageBodyStyle.Bare)]
        [RawFormat(typeof(PlotFormatter))]
        [Description("Plots the footprint, with advanced parameters")]
        [return: Description("The plot as an image in various formats.")]
        Spherical.Visualizer.Plot PlotRegionAdvanced(
            string regionName,
            [Description("Detailed parameters of the plot")]
            PlotRequest plot);

        #endregion
        #region Operations

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Put, UriTemplate = Urls.EditorRegion + Urls.Copy)]
        [Description("Copy a region.")]
        RegionResponse CopyRegion(
            [Description("Name of the target region.")]
            string regionName,
            RegionRequest request);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Put, UriTemplate = Urls.EditorRegion + Urls.Rename)]
        [Description("Renames a region.")]
        RegionResponse RenameRegion(
            [Description("Name of the target region.")]
            string regionName,
            RegionRequest request);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Put, UriTemplate = Urls.EditorRegion + Urls.Grow)]
        [Description("Grow region.")]
        RegionResponse GrowRegion(
            [Description("Name of the newly created region.")]
            string regionName,
            [Description("Growth radius in arc minutes.")]
            double radius,
            RegionRequest request);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Put, UriTemplate = Urls.EditorRegion + Urls.CHull)]
        [Description("Generate the convex hull of the regions.")]
        RegionResponse CHullRegions(
            [Description("Name of the newly created region.")]
            string regionName,
            RegionRequest request);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Put, UriTemplate = Urls.EditorRegion + Urls.Union)]
        [Description("Compute the union of regions.")]
        RegionResponse UnionRegions(
            [Description("Name of the newly created region.")]
            string regionName,
            RegionRequest request);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Put, UriTemplate = Urls.EditorRegion + Urls.Intersect)]
        [Description("Compute the intersection of regions.")]
        RegionResponse IntersectRegions(
            [Description("Name of the newly created region.")]
            string regionName,
            RegionRequest request);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Put, UriTemplate = Urls.EditorRegion + Urls.Subtract)]
        [Description("Compute the difference of regions.")]
        RegionResponse SubtractRegions(
            [Description("Name of the newly created region.")]
            string regionName,
            RegionRequest request);

        #endregion

        // TODO: save, load should go into the footprint service?

        /*

        [OperationContract]
        [WebGet(UriTemplate = "/load?owner={owner}&name={name}&region={regionName}")]
        [Description("Load a region from the database")]
        void Load(string owner, string name, string regionName);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = "/save?owner={owner}&name={name}&region={regionName}&method={combinationMethod}")]
        [Description("Save a region to the database")]
        void Save(string owner, string name, string regionName, string combinationMethod);

        */

        // TODO: add HTM cover
    }
}
