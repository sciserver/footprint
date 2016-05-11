
$(document).ready(function () {

    // TODO: userName +/ baseUrl declaration in a more proper way
    var userName = "test";
    var baseUrl = "http://localhost/footprint/api/v1/Footprint.svc/users";
    var requestedUrl = "";

    // --> Load Modal
    $("body").on("click", "#LaunchLoadModalButton", function () {
        $("#LoadModal").modal({ backdrop: "static" });
        LoadFootprintFolderList(baseUrl + "/" + userName);
    });

    $("body").on("change", "#FolderSelect", function () {
        ResetSelectList("#FootprintSelect");
        var folderName = $("#FolderSelect option:selected").html();
        LoadFootprintList(baseUrl + "/" + userName + "/" + folderName);
    });

    $("body").on("hidden.bs.modal", "#LoadModal", function () {
        ResetSelectList("#FolderSelect");
        ResetSelectList("#FootprintSelect");
    });

    $("body").on("click", "#LoadFootprintButton", function () {
        var footprint = $("#FootprintSelect option:selected").text();
        var folder = $("#FolderSelect option:selected").text();
        plotUrl = createUrl(baseUrl, [userName, folder, footprint, "plot?"]);

        $("#PlotCanvas").attr("src", plotUrl);
        $("#LoadModal").modal("hide");
    });
    // <--  Load Modal

    // --> Grow Modal
    $("body").on("click", "#LaunchGrowModalButton", function () {
        $("#GrowModal").modal({ backdrop: "static" });
    });
    // <-- Grow Modal

    // --> Add Region Modal
    $("body").on("click", "#LaunchAddRegionModalButton", function () {
        $("#AddRegionModal").modal({ backdrop: "static" });
    });

    $("body").on("change", "#AdditionTypeSelector input:radio", function () {
        $("#RegionTypeSelector").removeClass("hidden");

    })

    $("body").on("change", "#RegionTypeSelector input:radio", function () {
        selectedButton = $("#RegionTypeSelector input:radio:checked").val();
                $(".AddRegionForms").addClass("hidden");
        switch (selectedButton) {
            case "circle":
                $("#CircleRegionForm").removeClass("hidden");
                break;
            case "polygon":
                $("#PolygonRegionForm").removeClass("hidden");
                break;
            case "costum":
                $("#CostumRegionForm").removeClass("hidden");
                break;
            default:
                break;
        }

    });
    // <-- Add Region Modal

    // Centering modals
    $("body").on('show.bs.modal', ".modal", function () {
        centerModals($(this));
    });
    $(window).on('resize', centerModals);


});

// Reset Select options
function ResetSelectList(selectId) {
    $(selectId).empty()
        .append("<option>Please select...</option>");
    $(selectId + " option:selected").attr('disabled', 'disabled');

}

// GET Footprint folder list of User
function LoadFootprintFolderList(serviceUrl) {

    $.get(serviceUrl, function (data, status) {
        $(data).find("footprintfolder").each(function () {
            $("#FolderSelect").append($('<option>').append($(this).find("name").text()));
        })
    });
}

// GET selected folder and the footprints within
function LoadFootprintList(serviceUrl) {
    $.get(serviceUrl, function (data, status) {
        var nameList = $(data).find("footprintNameList");
        nameList.find("string").each(function () {
            $("#FootprintSelect").append($("<option>").append($(this).text()));
        })
    });
}

// Centering modals
function centerModals($element) {
    var $modals;
    if ($element.length) {
        $modals = $element;
    } else {
        $modals = $(".modal" + ':visible');
    }
    $modals.each(function () {
        var $clone = $(this).clone().css('display', 'block').appendTo('body');
        var top = Math.round(($clone.height() - $clone.find('.modal-content').height()) / 2);
        top = top > 0 ? top : 0;
        $clone.remove();
        $(this).find('.modal-content').css("margin-top", top);
    });
}

function createUrl(baseUrl, sourcePathParts) {
    var finalUrl = baseUrl;
    $.each(sourcePathParts, function (i, part) {
        finalUrl += "/" + part;
    });
    return finalUrl;
}