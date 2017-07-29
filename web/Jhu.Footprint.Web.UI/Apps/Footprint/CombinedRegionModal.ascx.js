function CombinedRegionModal(control) {
    RegionModalBase.call(this, control);

    this.combineMethod = null;
}

CombinedRegionModal.prototype = Object.create(RegionModalBase.prototype);
CombinedRegionModal.prototype.constructor = CombinedRegionModal;

CombinedRegionModal.prototype.keepOriginal = function (value) {
    var c = $(this.control).find("#keepOriginal");
    if (value) c.prop("checked", value);
    return c.prop("checked");
}

CombinedRegionModal.prototype.firstRegionVisible = function (value) {
    var c = $(this.control).find("#firstRegionRow");
    if (value) c.css("display", value ? "" : "none");
    return c.css("display") == "none" ? false : true;
}

CombinedRegionModal.prototype.clearFirstRegionList = function () {
    var list = $(this.control).find("#firstRegion");
    list.empty();
}

CombinedRegionModal.prototype.refreshFirstRegionList = function(regions) {
    var list = $(this.control).find("#firstRegion");
    list.empty();
    for (i = 0; i < regions.length; i ++) {
        list.append('<option value="' + regions[i] + '">' + regions[i] + '</option>');
    }
}

CombinedRegionModal.prototype.firstRegion = function () {
    var options = $(this.control).find("#firstRegion option:selected");
    if (options.length > 0) {
        return $(options[0]).val();
    } else {
        return null;
    }
}