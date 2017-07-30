function SearchForm(control) {
    ControlBase.call(this, control);
    var me = this;
    $(this.control).find('a[data-toggle="tab"]').on("shown.bs.tab", function (event) {
        $(me.control).find("#selectedTab").val(event.target.id);
        var ok = $(me.control).find('input[type="submit"]')[0];
        ok.onclick = function () {
            WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions("ok", "", true, event.target.id, "", false, false));
        };
        if (me.events.change) me.events.change(event.target.id);
    });
}

SearchForm.prototype = Object.create(ControlBase.prototype);
SearchForm.prototype.constructor = SearchForm;

SearchForm.prototype.message = function (value) {
    var c = $(this.control).find("#message");
    if (value) c.text(value);
    return c.text();
}


/*
function keepLastTabActive() {

    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        // save the latest tab; use cookies if you like 'em better:
        localStorage.setItem('lastTab', $(this).attr('href'));
        localStorage.setItem("advSearch", true);
    });

    // go to the latest tab, if it exists:
    var lastTab = localStorage.getItem('lastTab');
    if (lastTab) {
        $('[href="' + lastTab + '"]').tab('show');
    }
}



function toggleAdvancedSearch() {
    var advSearch = localStorage.getItem("advSearch");
    if (advSearch === "true") {
        $("#AdvancedSearchOptionsToggle").attr('checked', true);
        $("#AdvancedSearchOptionsPanel").removeClass("hidden");
        $("#FastSearchPanel").addClass("hidden");
    }

    $("body").on("click", "#AdvancedSearchOptionsToggle", function () {
        if ($(this).is(":checked")) {
            $("#AdvancedSearchOptionsPanel").removeClass("hidden");
            $("#FastSearchPanel").addClass("hidden");
            localStorage.setItem("advSearch", true);
        }
        else {
            $("#AdvancedSearchOptionsPanel").addClass("hidden");
            $("#FastSearchPanel").removeClass("hidden");
            localStorage.setItem("advSearch", false);
        }
    })
}
*/