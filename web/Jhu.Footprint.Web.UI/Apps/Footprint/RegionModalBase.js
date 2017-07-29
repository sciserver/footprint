function RegionModalBase(control) {
    ModalBase.call(this, control);
}

RegionModalBase.prototype = Object.create(ModalBase.prototype);
RegionModalBase.prototype.constructor = RegionModalBase;

RegionModalBase.prototype.regionName = function (value) {
    var c = $(this.control).find("#regionName");
    if (value) c.val(value);
    return c.val();
}

RegionModalBase.prototype.regionString = function () {
    return null;
}

RegionModalBase.prototype.region = function () {
    return {
        fillFactor: 1.0,
        name: this.regionName(),
        regionString: this.regionString()
    };
}