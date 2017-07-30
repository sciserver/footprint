var searchForm;

$(document).ready(function () {

    if ($("#searchForm").length > 0)
    searchForm = new SearchForm($("#searchForm")[0]);


    /*$('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var target = $(e.target).attr("id") // activated tab
        console.info(target);
        var searchMethod = document.getElementById("SearchMethod");
        searchMethod.value = target.replace("Search", "");
        console.info(searchMethod);

    });

    keepLastTabActive();

    toggleAdvancedSearch();*/
});