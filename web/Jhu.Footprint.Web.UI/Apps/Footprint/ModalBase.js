function ModalBase(control) {
    ControlBase.call(this, control);
    var me = this;
    $(this.control).find("#ok").on("click", function () {
        me.ok_click();
    });
}

ModalBase.prototype = Object.create(ControlBase.prototype);
ModalBase.prototype.constructor = ModalBase;

ModalBase.prototype.ok_click = function () {
    var group = this.control.id;
    if (Page_ClientValidate(group)) {
        if (this.events.ok) this.events.ok(this);
    }
};

ModalBase.prototype.show = function (init) {
    var me = this;
    if (init) {
        $.each(init, function (key, value) {
            if (typeof me[key] === "function") {
                me[key](value);
            } else {
                me[key] = value;
            }
        });
    }
    $(this.control).modal("show");
};

ModalBase.prototype.hide = function () {
    $(this.control).modal("hide");
};