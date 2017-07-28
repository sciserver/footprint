function MultipointRegionModal(control) {
    ModalBase.call(this, control);

    var me = this;

    $(this.control).find("#ok").on("click", function () {
        me.ok_click();
    });
}

MultipointRegionModal.prototype = Object.create(ModalBase.prototype);
MultipointRegionModal.prototype.constructor = MultipointRegionModal;

MultipointRegionModal.prototype.ok_click = function () {
    if (Page_ClientValidate("multipointRegionModal")) {
        if (this.events.ok) this.events.ok(this.getRegion());
    }
};

MultipointRegionModal.prototype.getRegion = function () {
    var regionName = $(this.control).find("#multipointRegionName").val();
    var regionString;
    if ($(this.control).find("#multipointRegionPoly").prop("checked")) {
        regionString = "POLY J2000 ";
    } else {
        regionString = "CHULL J2000 ";
    }
    regionString += $(this.control).find("#multipointRegionCoordinates").val();
    var region = {
        fillFactor: 1.0,
        name: regionName,
        regionString: regionString
    };
    return region;
};