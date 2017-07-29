function EditorRegionList(control, editorService) {
    ControlBase.call(this, control);

    var me = this;
    this.editorService = editorService;

    $(this.control).on("click", "input", function (event) {
        me.item_click(this, event);
    });
};

EditorRegionList.prototype = Object.create(ControlBase.prototype);
EditorRegionList.prototype.constructor = EditorRegionList;

EditorRegionList.prototype.item_click = function (item, event) {
    // Move selection to new item, except when shift or ctrl is pressed
    if (!event.ctrlKey && !event.shiftKey) {
        this.clearSelection();
        $(item).prop("checked", true);
    }
    if (this.events.click) this.events.click(item);
}

EditorRegionList.prototype.refreshList = function () {
    var me = this;
    this.editorService.listFootprintRegions(
        function (result) {
            var selection = me.getSelection();
            me.clearList();
            me.updateList(result.regions);
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

EditorRegionList.prototype.count = function () {
    return $(this.control).find("input").length;
};

EditorRegionList.prototype.generateUniqueName = function (prefix) {
    var cnt = $(this.control).find('input[data-item^="' + prefix + '"]').length;
    return prefix + "_" + (cnt + 1);
};

EditorRegionList.prototype.getSelection = function () {
    var selection = [];
    $(this.control).find("input:checked").each(function (index) {
        selection.push($(this).data("item"));
    });
    return selection;
}

EditorRegionList.prototype.clearSelection = function () {
    $(this.control).find("input").prop('checked', false);
}


EditorRegionList.prototype.applySelection = function (selection) {
    this.clearSelection();
    for (i = 0; i < selection.length; i++) {
        $(this.control).find('input[data-item="' + selection[i] + '"]').prop('checked', true);
    }
}
