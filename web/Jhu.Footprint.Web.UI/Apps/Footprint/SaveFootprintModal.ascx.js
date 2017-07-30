function SaveFootprintModal(control) {
    ModalBase.call(this, control);
}

SaveFootprintModal.prototype = Object.create(ModalBase.prototype);
SaveFootprintModal.prototype.constructor = SaveFootprintModal;

SaveFootprintModal.prototype.footprintName = function (value) {
    var c = $(this.control).find("#footprintName");
    if (value) c.val(value);
    return c.val();
}
