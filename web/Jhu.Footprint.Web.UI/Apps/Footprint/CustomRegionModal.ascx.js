function CustomRegionModal(control) {
    ModalBase.call(this, control);

    var me = this;

    $(this.control).find("#ok").on("click", function () {
        me.ok_click();
    });
}

CustomRegionModal.prototype = Object.create(ModalBase.prototype);
CustomRegionModal.prototype.constructor = CustomRegionModal;

CustomRegionModal.prototype.ok_click = function () {
    if (Page_ClientValidate("customRegionModal")) {
        if (this.events.ok) this.events.ok(this.getRegion());
    }
};

CustomRegionModal.prototype.getRegion = function () {
    var regionName = $(this.control).find("#customRegionName").val();
    var regionString = $(this.control).find("#customRegionString").val();
    var region = {
        fillFactor: 1.0,
        name: regionName,
        regionString: regionString
    };
    return region;
};