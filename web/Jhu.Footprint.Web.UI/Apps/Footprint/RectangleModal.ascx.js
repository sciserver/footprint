function RectangleModal(control) {
    RegionModalBase.call(this, control);
}

RectangleModal.prototype = Object.create(RegionModalBase.prototype);
RectangleModal.prototype.constructor = RectangleModal;

RectangleModal.prototype.regionString = function () {
    var regionString =
        "RECT J2000 " +
        $(this.control).find("#rectangleRa1").val() + " " +
        $(this.control).find("#rectangleDec1").val() + " " +
        $(this.control).find("#rectangleRa2").val() + " " +
        $(this.control).find("#rectangleDec2").val();
    return regionString;
};