function RectangleModal(control) {
    ModalBase.call(this, control);

    var me = this;

    $(this.control).find("#ok").on("click", function () {
        me.ok_click();
    });
}

RectangleModal.prototype = Object.create(ModalBase.prototype);
RectangleModal.prototype.constructor = RectangleModal;

RectangleModal.prototype.ok_click = function () {
    if (Page_ClientValidate("rectangleModal")) {
        if (this.events.ok) this.events.ok(this.getRegion());
    }
};

RectangleModal.prototype.getRegion = function () {
    var regionName = $(this.control).find("#rectangleName").val();
    var regionString =
        "RECT J2000 " +
        $(this.control).find("#rectangleRa1").val() + " " +
        $(this.control).find("#rectangleDec1").val() + " " +
        $(this.control).find("#rectangleRa2").val() + " " +
        $(this.control).find("#rectangleDec2").val();
    var region = {
        fillFactor: 1.0,
        name: regionName,
        regionString: regionString
    };
    return region;
};