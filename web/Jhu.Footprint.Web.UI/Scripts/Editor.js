// Declaration of the services' urls
// TODO: maybe should be done in a more proper way?
footprintSvcUrl = "";
editorSvcUrl = "http://localhost/footprint/api/v1/editor.svc";

// Setup cookie
$(document).one('ready', function () {
    $.ajax({
        url: editorSvcUrl + "/reset",
        type: "GET"
    })
});

$(document).ready(function () {
    // -------------> LOAD MODAL
    /*
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
    */
    // <------------ LOAD MODAL

    // ------------> GROW MODAL
    // <------------ GROW MODAL

    // -------------> ADD REGION MODAL

    // Show the associeted input form with the selected region type
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

    // Submit the form and create the recquired region
    $("body").on("click", "#AddRegionButton", function () {
        var selectedAdditionType = $("#AdditionTypeSelector input:radio:checked").val();
        var selectedRegionType = $("#RegionTypeSelector input:radio:checked").val();

        // TODO: real region string creator, now it's only in test phase.
        var regionString = createRegionString(selectedRegionType);

        console.log(regionString)
        addRegion(selectedAdditionType, regionString);
        getShape();

        $(".modal").modal("hide");
    })
    // <-- Add Region Modal

    // Centering modals
    $("body").on('show.bs.modal', ".modal", function () {
        centerModals($(this));
    });
    $(window).on('resize', centerModals);


});

// Reset Select options
/*
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


*/

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


// Create region strings
function createRegionString(type) {
    var regionStrings = []; 

    switch (type) {
        case "circle":
            regionStrings.push("CIRCLE J2000", $("#CircleRA").val(), $("#CircleDec").val(), $("#CircleRadius").val());
            break;
        case "polygon": 
            var re = /,\s+|\s\s+/g;
            var points = $("#PolygonPoints").val().replace(re," ");
            regionStrings.push("POLY J2000",points);
            break;
        case "costum":
            break;            
    }

    return regionStrings.join(" ");
}


// Building region creating ajax call
function addRegion(type, dataString) {
    var methodUrl = createUrl(editorSvcUrl, { type });

    $.ajax({
        url: methodUrl,
        type: "POST",
        contentType: "text/plain",
        data: dataString
    });
}

function growRegion(radius) {
    var methodUrl = editorSvcUrl + "/grow";

    $.ajax({
        url: methodUrl,
        type: "POST",
        contentType: "text/plain",
        data: radius
    });
}

// getShape
function getShape() {
    var methodUrl = editorSvcUrl + "/shape";
    $.ajax({
        url: methodUrl,
        type: "GET",
        headers: { Accept: "text/plain" },
        success: function (data, status, xhr) {
            // TODO: plot region
            $("#RegionStringTest").append(data);
        }
    });
}