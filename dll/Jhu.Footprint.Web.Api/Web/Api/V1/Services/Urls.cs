using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.Footprint.Web.Api.V1
{
    class Urls
    {
        public const string AllFootprints = "/footprints";
        public const string UserFootprints = "/users/{owner}/footprints";
        public const string UserFootprint = UserFootprints + "/{name}";
        public const string UserFootprintRegions = UserFootprint + "/regions";
        public const string UserFootprintRegion = UserFootprintRegions + "/{regionName}";

        public const string EditorFootprint = "/footprint";
        public const string EditorFootprintRegions = EditorFootprint + "/regions";
        public const string EditorFootprintRegion = EditorFootprintRegions + "/{regionName}";

        public const string OwnerSearchParam = "owner={owner}";
        public const string FootprintSearchParams = "name={name}&from={from}&max={max}";
        public const string RegionSearchParams = "regionName={regionName}&from={from}&max={max}";

        public const string Combine = "?op={operation}&keepOrig={keepOriginal}";

        public const string Shape = "/shape";
        public const string Outline = "/outline";
        public const string OutlinePoints = "/outline/points?res={resolution}";
        public const string Plot = "/plot?proj={projection}&sys={sys}&ra={ra}&dec={dec}&b={b}&l={l}&width={width}&height={height}&theme={colorTheme}&zoom={autoZoom}&rotate={autoRotate}&grid={grid}&degStyle={degreeStyle}&highlights={highlights}";
        public const string PlotAdvanced = "/plot?";
        public const string Thumbnail = "/thumbnail";
    }
}
