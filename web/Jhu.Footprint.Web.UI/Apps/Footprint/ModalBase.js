function ModalBase(control) {
    ControlBase.call(this, control);
}

ModalBase.prototype = Object.create(ControlBase.prototype);
ModalBase.prototype.constructor = ModalBase;

ModalBase.prototype.show = function () {
    $(this.control).modal("show");
};

ModalBase.prototype.hide = function () {
    $(this.control).modal("hide");
};