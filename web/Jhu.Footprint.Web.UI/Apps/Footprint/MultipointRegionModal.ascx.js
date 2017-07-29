function MultipointRegionModal(control) {
    RegionModalBase.call(this, control);
}

MultipointRegionModal.prototype = Object.create(RegionModalBase.prototype);
MultipointRegionModal.prototype.constructor = MultipointRegionModal;

MultipointRegionModal.prototype.poly = function (value) {
    var c = $(this.control).find("#multipointRegionPoly")
    if (value) c.prop("checked", value)
    return c.prop("checked");
}

MultipointRegionModal.prototype.chull = function (value) {
    var c = $(this.control).find("#multipointRegionCHull")
    if (value) c.prop("checked", value)
    return c.prop("checked");
}

MultipointRegionModal.prototype.mode = function (value) {
    if (value)
    {
        switch (value) {
            case "poly":
                this.poly(true);
                break;
            case "chull":
                this.chull(true);
                break;
            default:
                throw "unknown mode"
        }
        if (this.poly()) {
            return "poly";
        }
        else {
            return "chull";
        }
    }
}

MultipointRegionModal.prototype.regionString = function () {
    var regionString;
    if (this.poly()) {
        regionString = "POLY J2000 ";
    } else {
        regionString = "CHULL J2000 ";
    }
    regionString += $(this.control).find("#multipointRegionCoordinates").val();
    return regionString;
};