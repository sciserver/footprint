using System;
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

        // TODO: save, load should go into the footprint service

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Delete, UriTemplate = Urls.EditorFootprint)]
        [Description("Delete footprint and reset the editor.")]
        void DeleteFootprint();

        [OperationContract]
        [RegionFormatter]
        [WebGet(UriTemplate = Urls.EditorFootprint + Urls.Raw)]
        [Description("Returns the footprint in raw format text or binary.")]
        [return: Description("The combined footprint in string or binary representation.")]
        Spherical.Region DownloadFootprint();

        [OperationContract]
        [OutlineFormatter]
        [WebGet(UriTemplate = Urls.EditorFootprint + Urls.Outline, BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Returns the outline of the footprint.")]
        [return: Description("The outline of the combined footprint in string representation.")]
        Spherical.Outline GetFootprintOutline();

        [OperationContract]
        [TextJsonXmlFormat]
        [WebGet(UriTemplate = Urls.EditorFootprint + Urls.OutlinePoints)]
        [Description("Returns the points of the outline of the footprint.")]
        [return: Description("The points of the outline of the combined footprint.")]
        IEnumerable<Point> GetFootprintOutlinePoints(
            [Description("Representation system of the points")]
            string sys,
            [Description("Sampling resolution")]
            double? resolution,
            [Description("Outline complexity reduction method")]
            string reduce,
            [Description("Outline complexity reduction limit")]
            double? limit);

        [OperationContract]
        [PlotFormatter]
        [WebGet(UriTemplate = Urls.EditorFootprint + Urls.Plot + Urls.PlotHightlighs, BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Plots the footprint")]
        [return: Description("The plot as an image in various formats.")]
        Spherical.Visualizer.Plot PlotFootprint(
            [Description("Projection")]
            string projection,
            [Description("Coordinate system")]
            string sys,
            [Description("Center longitude")]
            string lon,
            [Description("Center latitude")]
            string lat,
            [Description("Image width")]
            float width,
            [Description("Image height")]
            float height,
            [Description("Color theme")]
            string colorTheme,
            [Description("Zoom in on the region automatically.")]
            string autoZoom,
            [Description("Rotate region to the origin of projection automatically")]
            string autoRotate,
            [Description("Plot grid")]
            string grid,
            [Description("Format of numeric values")]
            string degreeStyle,
            [Description("Highlights")]
            string highlights);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = Urls.EditorFootprint + Urls.PlotAdvanced, BodyStyle = WebMessageBodyStyle.Bare)]
        [PlotFormatter]
        [Description("Plots the footprint, with advanced parameters")]
        [return: Description("The plot as an image in various formats.")]
        Spherical.Visualizer.Plot PlotFootprintAdvanced(
            [Description("Detailed parameters of the plot")]
            Plot plotParameters);

        #endregion
        #region Footprint region CRUD operations

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Put, UriTemplate = Urls.EditorRegion + Urls.Circle)]
        [Description("Create a new region.")]
        RegionResponse CreateRegion(string regionName, RegionRequest request);

        [OperationContract]
        [WebGet(UriTemplate = Urls.EditorRegion)]
        [Description("Returns the header information of a region.")]
        RegionResponse GetRegion(string regionName);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Get, UriTemplate = Urls.EditorRegions + Urls.RegionSearchParams + Urls.Paging)]
        [Description("List all regions.")]
        RegionListResponse ListRegions(string regionName, int? from, int? max);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Patch, UriTemplate = Urls.EditorRegion)]
        [Description("Modify a region.")]
        RegionResponse ModifyRegion(string regionName, RegionRequest request);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Delete, UriTemplate = Urls.EditorRegion)]
        [Description("Delete a region.")]
        void DeleteRegion(string regionName);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Delete, UriTemplate = Urls.EditorRegions)]
        [Description("Deletes multiple regions.")]
        void DeleteRegions(
            [Description("A region request containing an array of region names to delete. Specify a single * to delete everything.")]
            RegionRequest region);



        #endregion
        #region Boolean operations

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = Urls.EditorRegion + Urls.Combine)]
        [Description("Compute union, intersection or difference of regions.")]
        RegionResponse CombineFootprintRegions(
            [Description("Name of the newly created region.")]
            string regionName,
            [Description("Boolean operation, one of 'union', 'intersect' or 'subtract'.")]
            string operation,
            [Description("Keep all original regions.")]
            bool keepOriginal,
            RegionRequest request);

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

        #endregion

        #region Individual region set, get and plot

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = Urls.EditorRegion + Urls.Raw, BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Upload a region shape binary or other representation")]
        void SetFootprintRegionShape(string regionName, Stream stream);

        [OperationContract]
        [RegionFormatter]
        [WebGet(UriTemplate = Urls.EditorRegion + Urls.Raw, BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Returns the shape description of the footprint region.")]
        Spherical.Region GetFootprintRegionShape(string regionName);

        [OperationContract]
        [OutlineFormatter]
        [WebGet(UriTemplate = Urls.EditorRegion + Urls.Outline, BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Returns the outline of the footprint.")]
        Spherical.Outline GetFootprintRegionOutline(string regionName);

        [OperationContract]
        [TextJsonXmlFormat]
        [WebGet(UriTemplate = Urls.EditorRegion + Urls.OutlinePoints)]
        [Description("Returns the points of the outline of the footprint.")]
        IEnumerable<Point> GetFootprintRegionOutlinePoints(string regionName, double resolution);

        [OperationContract]
        [WebGet(UriTemplate = Urls.EditorRegion + Urls.Plot, BodyStyle = WebMessageBodyStyle.Bare)]
        [PlotFormatter]
        [Description("Plots a footprint region.")]
        Spherical.Visualizer.Plot PlotFootprintRegion(
            string regionName,
            string projection,
            string sys,
            string ra,
            string dec,
            string b,
            string l,
            float width,
            float height,
            string colorTheme,
            string autoZoom,
            string autoRotate,
            string grid,
            string degreeStyle);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = Urls.EditorRegion + Urls.PlotAdvanced, BodyStyle = WebMessageBodyStyle.Bare)]
        [PlotFormatter]
        [Description("Plots a footprint region.")]
        Spherical.Visualizer.Plot PlotFootprintRegionAdvanced(string regionName, Plot plotParameters);


        [OperationContract]
        [WebGet(UriTemplate = Urls.EditorRegion + Urls.Thumbnail)]
        [Description("Gets the thumbnail of a footprint region.")]
        Stream GetFootprintRegionThumbnail(string regionName);

        // TODO: add HTM cover

        #endregion
    }
}
