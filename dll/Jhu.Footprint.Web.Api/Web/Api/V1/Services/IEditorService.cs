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
        FootprintResponse GetFootprint();

        // TODO: save, load should go into the footprint service

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Delete, UriTemplate = Urls.EditorFootprint)]
        [Description("Delete footprint and reset the editor.")]
        void DeleteFootprint();

        [OperationContract]
        [RegionFormatter]
        [WebGet(UriTemplate = Urls.EditorFootprint + Urls.Raw)]
        [Description("Returns the footprint in raw format text or binary.")]
        Spherical.Region GetFootprintRaw();

        [OperationContract]
        [OutlineFormatter]
        [WebGet(UriTemplate = Urls.EditorFootprint + Urls.Outline, BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Returns the outline of the footprint.")]
        Spherical.Outline GetFootprintOutline();

        [OperationContract]
        [TextJsonXmlFormat]
        [WebGet(UriTemplate = Urls.EditorFootprint + Urls.OutlinePoints)]
        [Description("Returns the points of the outline of the footprint.")]
        IEnumerable<Lib.EquatorialPoint> GetFootprintOutlinePoints(double resolution);

        [OperationContract]
        [PlotFormatter]
        [WebGet(UriTemplate = Urls.EditorFootprint + Urls.Plot + Urls.PlotHightlighs, BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Plots the footprint")]
        Spherical.Visualizer.Plot PlotFootprint(
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
            string degreeStyle,
            string highlights);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = Urls.EditorFootprint + Urls.PlotAdvanced, BodyStyle = WebMessageBodyStyle.Bare)]
        [PlotFormatter]
        [Description("Plots the footprint, with advanced parameters")]
        Spherical.Visualizer.Plot PlotFootprintAdvanced(Plot plotParameters);

        [OperationContract]
        [WebGet(UriTemplate = Urls.EditorFootprint + Urls.Thumbnail)]
        [Description("Gets the pre-generated thumbnail of the footprint.")]
        Stream GetFootprintThumbnail();
        
        #endregion
        #region Footprint region CRUD operations

        [OperationContract]
        [WebGet(UriTemplate = Urls.EditorFootprintRegion)]
        [Description("Returns the header information of a region.")]
        FootprintRegionResponse GetFootprintRegion(string regionName);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Put, UriTemplate = Urls.EditorFootprintRegion)]
        [Description("Create new region.")]
        FootprintRegionResponse CreateFootprintRegion(string regionName, FootprintRegionRequest request);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Patch, UriTemplate = Urls.EditorFootprintRegion)]
        [Description("Modify a region.")]
        FootprintRegionResponse ModifyFootprintRegion(string regionName, FootprintRegionRequest request);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Delete, UriTemplate = Urls.EditorFootprintRegion)]
        [Description("Delete a region.")]
        void DeleteFootprintRegion(string regionName);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Delete, UriTemplate = Urls.EditorFootprintRegions)]
        [Description("Deletes multiple regions.")]
        void DeleteFootprintRegions(
            [Description("An array of region names to delete. Specify a single * to delete everything.")]
            string[] regionNames);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Get, UriTemplate = Urls.EditorFootprintRegions)]
        [Description("List all regions.")]
        FootprintRegionListResponse ListFootprintRegions();

        #endregion
        #region Boolean operations

        [OperationContract]
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = Urls.EditorFootprintRegion + Urls.Combine)]
        [Description("Compute union, intersection or difference of regions.")]
        FootprintRegionResponse CombineFootprintRegions(
            [Description("Name of the newly created region.")]
            string regionName,
            [Description("Boolean operation, one of 'union', 'intersect' or 'subtract'.")]
            string operation,
            [Description("Keep all original regions.")]
            bool keepOriginal,
            FootprintRegionRequest request);

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
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = Urls.EditorFootprintRegion + Urls.Raw, BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Upload a region shape binary or other representation")]
        void SetFootprintRegionShape(string regionName, Stream stream);

        [OperationContract]
        [RegionFormatter]
        [WebGet(UriTemplate = Urls.EditorFootprintRegion + Urls.Raw, BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Returns the shape description of the footprint region.")]
        Spherical.Region GetFootprintRegionShape(string regionName);

        [OperationContract]
        [OutlineFormatter]
        [WebGet(UriTemplate = Urls.EditorFootprintRegion + Urls.Outline, BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Returns the outline of the footprint.")]
        Spherical.Outline GetFootprintRegionOutline(string regionName);

        [OperationContract]
        [TextJsonXmlFormat]
        [WebGet(UriTemplate = Urls.EditorFootprintRegion + Urls.OutlinePoints)]
        [Description("Returns the points of the outline of the footprint.")]
        IEnumerable<Lib.EquatorialPoint> GetFootprintRegionOutlinePoints(string regionName, double resolution);

        [OperationContract]
        [WebGet(UriTemplate = Urls.EditorFootprintRegion + Urls.Plot, BodyStyle = WebMessageBodyStyle.Bare)]
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
        [WebInvoke(Method = HttpMethod.Post, UriTemplate = Urls.EditorFootprintRegion + Urls.PlotAdvanced, BodyStyle = WebMessageBodyStyle.Bare)]
        [PlotFormatter]
        [Description("Plots a footprint region.")]
        Spherical.Visualizer.Plot PlotFootprintRegionAdvanced(string regionName, Plot plotParameters);


        [OperationContract]
        [WebGet(UriTemplate = Urls.EditorFootprintRegion + Urls.Thumbnail)]
        [Description("Gets the thumbnail of a footprint region.")]
        Stream GetFootprintRegionThumbnail(string regionName);

        // TODO: add HTM cover

        #endregion
    }
}
