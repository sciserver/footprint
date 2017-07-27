function EditorService() {
    ServiceBase.call(this);
    this.serviceUrl = "../../api/v1/editor.svc";
}

EditorService.prototype = Object.create(ServiceBase.prototype);
EditorService.prototype.constructor = EditorService;

EditorService.prototype.getFootprintRegion = function () {
}

EditorService.prototype.createFootprintRegion = function (region, success) {
        var url = this.createUrl(["footprint", "regions", region.name]);
        var request = {
            contentType: "application/json",
            data: JSON.stringify({
                region: region
            })
        };
        this.callService(url, "POST", request,
            function (result, status, xhr) {
                success(result.region);
            });
    },

EditorService.prototype.listFootprintRegions = function (success) {
        var url = this.createUrl(["footprint", "regions"]);
        this.callService(url, "GET", null,
            function (result, status, xhr) {
                success(result.regions);
            });
}

var editorService = new EditorService();