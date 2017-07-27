function EditorService() { 
    this.serviceUrl = "../../api/v1/editor.svc";
}

EditorService.prototype = {
    getFootprintRegion: function () { },

    createFootprintRegion: function (region, success) {
        var url = createUrl(editorServiceUrl, ["footprint", "regions", region.name]);
        var request = {
            contentType: "application/json",
            data: JSON.stringify({
                region: region
            })
        };
        callService(url, "POST", request,
            function (result, status, xhr) {
                success(result.region);
            },
            this.error);
    },

    listFootprintRegions: function (success) {
        var url = createUrl(this.serviceUrl, ["footprint", "regions"]);
        callService(url, "GET", null,
            function (result, status, xhr) {
                success(result.regions);
            },
            this.error);
    },

    error: function (xhr, status, message) {
        alert(error);
    }
}

var editorService = new EditorService();