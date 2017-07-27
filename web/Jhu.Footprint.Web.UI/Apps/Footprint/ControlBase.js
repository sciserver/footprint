function ControlBase(control) {
    var me = this;

    this.control = control;
    this.events = {};
}

ControlBase.prototype.on = function (name, handler) {
    this.events[name] = handler;
};