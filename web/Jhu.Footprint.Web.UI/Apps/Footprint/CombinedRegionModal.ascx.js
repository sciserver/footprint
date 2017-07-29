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
