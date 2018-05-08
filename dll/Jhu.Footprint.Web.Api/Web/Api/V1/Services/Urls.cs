﻿using System;
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
        public const string EditorRegionSearch = EditorRegions + "?" + RegionSearchParams;


        public const string OwnerSearchParam = "owner={owner}";
        public const string FootprintSearchParams = "name={name}";
        public const string RegionSearchParams = "regionName={regionName}";

        public const string Copy = "/copy";
        public const string Move = "/move";
        public const string Union = "/union" + KeepOriginal;
        public const string Intersect = "/intersect" + KeepOriginal;
        public const string Subtract = "/subtract" + KeepOriginal;
        public const string CHull = "/chull" + KeepOriginal;
        public const string Grow = "/grow" + KeepOriginal + "&radius={radius}";
        
        public const string Outline = "/outline";
        public const string OutlinePoints = "/outline/points?sys={sys}&rep={rep}&res={resolution}&reduce={reduce}&limit={limit}";
        public const string Plot = "/plot";
        public const string Thumbnail = "/thumbnail";

        public const string Raw = "/raw";
        public const string KeepOriginal = "?keepOrig={keepOriginal}";
        public const string Paging = "&from={from}&max={max}";
        public const string PlotDetails = "?proj={projection}&sys={sys}&lon={lon}&lat={lat}&width={width}&height={height}&theme={colorTheme}&zoom={autoZoom}&rotate={autoRotate}&grid={grid}&degStyle={degreeStyle}";
    }
}
