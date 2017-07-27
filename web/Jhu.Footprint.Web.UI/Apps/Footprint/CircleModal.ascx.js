function CircleModal(control) {
    ModalBase.call(this, control);

    var me = this;

    $(this.control).find("#ok").on("click", function () {
        me.ok_click();
    });
}

CircleModal.prototype = Object.create(ModalBase.prototype);
CircleModal.prototype.constructor = CircleModal;

CircleModal.prototype.ok_click = function () {
    if (Page_ClientValidate("circleModal")) {
        if (this.events.ok) this.events.ok(this.getRegion());
    }
};

CircleModal.prototype.getRegion = function () {
    var regionName = $(this.control).find("#circleName").val();
    var regionString =
        "CIRCLE J2000 " +
        $(this.control).find("#circleCenterRa").val() + " " +
        $(this.control).find("#circleCenterDec").val() + " " +
        $(this.control).find("#circleRadius").val();
    var region = {
        fillFactor: 1.0,
        name: regionName,
        regionString: regionString
    };
    return region;
};