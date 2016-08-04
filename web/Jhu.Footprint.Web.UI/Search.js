$(document).ready(function () {
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var target = $(e.target).attr("id") // activated tab
        console.info(target);
        var searchMethod = document.getElementById("SearchMethod");
        searchMethod.value = target.replace("Search","");
        console.info(searchMethod);

    });

    keepLastTabActive();
});

function keepLastTabActive() {

    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        // save the latest tab; use cookies if you like 'em better:
        localStorage.setItem('lastTab', $(this).attr('href'));
    });

    // go to the latest tab, if it exists:
    var lastTab = localStorage.getItem('lastTab');
    if (lastTab) {
        $('[href="' + lastTab + '"]').tab('show');
    }
}