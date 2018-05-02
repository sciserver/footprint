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
        public const string EditorRegions = EditorFootprint + "/regions";
        public const string EditorRegion = EditorRegions + "/{regionName}";

        public const string OwnerSearchParam = "owner={owner}";
        public const string FootprintSearchParams = "name={name}";
        public const string RegionSearchParams = "regionName={regionName}";

        public const string Copy = "?op=copy";
        public const string Combine = "?op={operation}&keepOrig={keepOriginal}";

        public const string Raw = "/raw";
        public const string Outline = "/outline";
        public const string OutlinePoints = "/outline/points?sys={sys}&res={resolution}&reduce={reduce}&limit={limit}";
        public const string Plot = "/plot?proj={projection}&sys={sys}&ra={ra}&dec={dec}&b={b}&l={l}&width={width}&height={height}&theme={colorTheme}&zoom={autoZoom}&rotate={autoRotate}&grid={grid}&degStyle={degreeStyle}";
        public const string PlotHightlighs = "&highlights={highlights}";
        public const string PlotAdvanced = "/plot";
        public const string Thumbnail = "/thumbnail";
        public const string Circle = "/circle?ra={ra}&dec={dec}&cx={cx}&cy={cy}&cz={cz}&rad={rad}&cos0={cos0}";
        public const string Rect = "/rect?ra1={ra1}&dec1={dec1}&cx1={cx1}&cy1={cy1}&cz1={cz1}&ra2={ra2}&dec2={dec2}&cx2={cx2}&cy2={cy2}&cz2={cz2}";
        public const string Poly = "/poly";
        public const string CHull = "/chull";

        public const string Paging = "&from={from}&max={max}";
    }
}
