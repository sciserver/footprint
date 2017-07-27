function EditorRegionList(control) {
    this.control = control;

    this.refreshList = function () {
        var me = this;
        editorService.listFootprintRegions(
            function (regions) {
                me.clearList();
                me.updateList(regions);
            });
    };

    this.clearList = function () {
        $(this).empty();
    };

    this.updateList = function (regions) {
        var me = this;
        for (i = 0; i < regions.length; i++) {
            me.appendItem(regions[i]);
        };
    };

    this.appendItem = function (region) {
        var html = this.createItem(region);
        $(this.control).append(html);
    }

    this.createItem = function (region) {
        var html = '<div class="gw-list-item">';
        html += '<span style="width: 24px"><input type="checkbox" data-item="' + region.name + '" /></span>';
        html += '<span class="gw-list-span">' + region.name + '</span>';
        html += '</div>';
        return html;
    };

    this.getSelection = function () {
        var selection = [];
        $("#" + this.id + " input:checked").each(function (index) {
            selection.push($(this).data("item"));
        });
        return selection;
    }

    this.error = function (message) {
        alert(message);
    };
};