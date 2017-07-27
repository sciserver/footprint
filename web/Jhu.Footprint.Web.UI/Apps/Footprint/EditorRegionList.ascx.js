function EditorRegionList(control) {
    ControlBase.call(this, control);

    var me = this;

    $(this.control).on("click", "input", function () {
        me.item_click(this);
    });
};

EditorRegionList.prototype = Object.create(ControlBase.prototype);
EditorRegionList.prototype.constructor = EditorRegionList;

EditorRegionList.prototype.item_click = function (item) {
    if (this.events.click) this.events.click(item);
}

EditorRegionList.prototype.refreshList = function () {
    var me = this;
    editorService.listFootprintRegions(
        function (regions) {
            var selection = me.getSelection();
            me.clearList();
            me.updateList(regions);
            me.applySelection(selection)
        });
}

EditorRegionList.prototype.clearList = function () {
    $(this.control).empty();
}

EditorRegionList.prototype.updateList = function (regions) {
    for (i = 0; i < regions.length; i++) {
        this.appendItem(regions[i]);
    };
}

EditorRegionList.prototype.appendItem = function (region) {
    var html = this.createItem(region);
    $(this.control).append(html);
}

EditorRegionList.prototype.createItem = function (region) {
    var html = '<div class="gw-list-item">';
    html += '<span style="width: 24px"><input type="checkbox" data-item="' + region.name + '" /></span>';
    html += '<span class="gw-list-span">' + region.name + '</span>';
    html += '</div>';
    return html;
}

EditorRegionList.prototype.getSelection = function () {
    var selection = [];
    $(this.control).find("input:checked").each(function (index) {
        selection.push($(this).data("item"));
    });
    return selection;
}

EditorRegionList.prototype.applySelection = function (selection) {
    $(this.control).find("input").prop('checked', false);
    for (i = 0; i < selection.length; i++) {
        $(this.control).find('input[data-item="' + selection[i] + '"]').prop('checked', true);
    }
}
