function CircleModal(control) {
    RegionModalBase.call(this, control);
}

CircleModal.prototype = Object.create(RegionModalBase.prototype);
CircleModal.prototype.constructor = CircleModal;

CircleModal.prototype.regionString = function () {
    var regionString =
        "CIRCLE J2000 " +
        $(this.control).find("#circleCenterRa").val() + " " +
        $(this.control).find("#circleCenterDec").val() + " " +
        $(this.control).find("#circleRadius").val();
    return regionString;
};