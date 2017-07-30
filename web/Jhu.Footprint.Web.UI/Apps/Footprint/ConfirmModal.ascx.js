function ConfirmModal(control) {
    ModalBase.call(this, control);
}

ConfirmModal.prototype = Object.create(ModalBase.prototype);
ConfirmModal.prototype.constructor = ConfirmModal;

ConfirmModal.prototype.message = function (value) {
    var c = $(this.control).find("#message");
    if (value) c.text(value);
    return c.text();
}
