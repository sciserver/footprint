function CustomRegionModal(control) {
    RegionModalBase.call(this, control);
}

CustomRegionModal.prototype = Object.create(RegionModalBase.prototype);
CustomRegionModal.prototype.constructor = CustomRegionModal;

CustomRegionModal.prototype.regionString = function () {
    return $(this.control).find("#customRegionString").val();
}
