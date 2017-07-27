function CircleModal(control) {
    var me = this;

    this.control = control;
    this.ok = null;

    // Event handlers

    $(this.control).find("#ok").on("click", function () {
        me.ok_click();
    });
}

CircleModal.prototype = {
    ok_click: function () {
        if (Page_ClientValidate("circleModal")) {
            if (this.ok) this.ok(this.getRegion());
            this.hide();
        }
    },

    on: function (name, handler) {
        this[name] = handler;
    },

    show: function () {
        $(this.control).modal("show");
    },

    hide: function () {
        $(this.control).modal("hide");
    },

    getRegion: function () {
        var regionName = $(this.control).find("#circleName").val();
        var regionString =
            "CIRCLE J2000 " +
            $(this.control).find("#circleCenterRa").val() + " " +
            $(this.control).find("#circleCenterDec").val() + " " +
            $(this.control).find("#circleRadius").val();
        var region = {
            fillFactor: 1.0,
            name: regionName,
            regionString: regionString
        };
        return region;
    }
}