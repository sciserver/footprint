function ModalBase(control) {
    ControlBase.call(this, control);
}

ModalBase.prototype = Object.create(ControlBase.prototype);
ModalBase.prototype.constructor = ModalBase;

ModalBase.prototype.show = function (init) {
    var me = this;
    if (init) {
        $.each(init, function (key, value) {
            if (typeof value == "boolean") {
                $(me.control).find("#" + key).prop("checked", value);
            }
            else {
                $(me.control).find("#" + key).val(value);
            }
        });
    }

    $(this.control).modal("show");
};

ModalBase.prototype.hide = function () {
    $(this.control).modal("hide");
};