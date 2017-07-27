function EditorCanvas(control) {
    var me = this;

    this.control = control;
};

EditorCanvas.prototype = {

    refresh: function (plot) {
        plot.width = $(this.control).width();
        plot.height = $(this.control).height();
        plot.ts = new Date().getTime();

        var url = editorService.createUrl(["footprint", "plot"], plot);
        $(this.control).attr("src", url);
    }

};