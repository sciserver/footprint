$(document).ready(function () {
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var target = $(e.target).attr("id") // activated tab
        console.info(target);
        var searchMethod = document.getElementById("SearchMethod");
        searchMethod.value = target.replace("Search","");
        console.info(searchMethod);

    });

    keepLastTabActive();

    toggleAdvancedSearch();
});

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
    if ( advSearch === "true")
    {
        $("#AdvancedSearchOptionsToggle").attr('checked', true);
        $("#AdvancedSearchOptionsPanel").removeClass("hidden");
        $("#FastSearchPanel").addClass("hidden");
    }

    $("body").on("click", "#AdvancedSearchOptionsToggle", function () {
        if ($(this).is(":checked") ) {
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