function EditorCanvas(control, editorService) {
    ControlBase.call(this, control);

    this.editorService = editorService;
    this.timer = null;
};

EditorCanvas.prototype = Object.create(ControlBase.prototype);
EditorCanvas.prototype.constructor = EditorCanvas;

EditorCanvas.prototype.refresh = function (plot) {
    this.timer = null;
    plot.width = $(this.control).width();
    plot.height = $(this.control).height();
    plot.ts = new Date().getTime();
    var url = this.editorService.__createUrl(["footprint", "plot"], plot);
    $(this.control).attr("src", url);
};

EditorCanvas.prototype.delayedRefresh = function (plot) {
    var me = this;
    if (this.timer) clearTimeout(this.timer);
    this.timer = setTimeout(function () {
        me.refresh(plot);
    }, 2000);
};