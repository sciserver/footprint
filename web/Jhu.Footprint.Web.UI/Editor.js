// Declaration of the services' urls
// TODO: maybe should be done in a more proper way?
var footprintSvcUrl = "http://localhost/footprint/api/v1/footprint.svc";
var editorSvcUrl = "http://localhost/footprint/api/v1/editor.svc";
var authSvcUrl = "http://localhost/auth/api/v1/auth.svc";

$(document).one('ready', function () {
    // Setup cookie
    setCookies();

    // Set owner
    // TODO: include group
    setOwner();
});

$(document).ready(function () {


    // refresh Canvas with button
    $("body").on("click", "#refreshCanvasButton", function () {
        refreshCanvas();
    })

    // Centering modals
    $("body").on('show.bs.modal', ".modal", function () {
        centerModals($(this));
    });
    $(window).on('resize', centerModals);

    // -------------> MODALS :
    // -------------> ADD REGION MODAL

    // Show the associeted input form with the selected region type
    $("body").on("change", "#RegionTypeSelector input:radio", function () {
        var selectedButton = $("#RegionTypeSelector input:radio:checked").val();
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

        var regionString = createRegionString(selectedRegionType);
        var json = createFootprintRegionRequest(regionString);

        console.info(regionString)

        addRegion(selectedAdditionType, json);

        // TODO: figure is not showing in Chrome
        refreshCanvas();

        $(".modal").modal("hide");
    })

    // -------------> LOAD MODAL
    // TODO: VERY IMPORTANT TO LOAD OWNER NAMES ASWELL!!!
    $("body").on("show.bs.modal", "#LoadModal", function () {
        ResetSelectList("#RegionSelect");
        ResetSelectList("#FootprintSelect");
        setFootprintList();
    });

    $("body").on("change", "#FootprintSelect", function () {
        ResetSelectList("#RegionSelect");
        setRegionList();
    });

    $("body").on("click", "#LoadRegionButton", function () {
        loadRegion();
        refreshCanvas();

        $("#LoadModal").modal("hide");
    });


    // ------------> GROW MODAL
    $("body").on("click", "#GrowButton", function () {
        var radius = $("#GrowRadius").val();
        growRegion(radius);
        refreshCanvas();
        $(".modal").modal("hide");
    });

    // ------------> SAVE MODAL

    $("body").on("click", "#SaveRegionButton", function () {
        saveRegion();
        $("#SaveModal").modal("hide");
    });
});

// Reset Select options
function ResetSelectList(selectId) {
    $(selectId).empty()
        .append("<option>Please select...</option>");
    $(selectId + " option:selected").attr('disabled', 'disabled');
}
/*


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

// Create service urls

function createUrl(baseUrl, pathParts, optQueryParts) {
    var finalUrl = baseUrl;
    $.each(pathParts, function (i, part) {
        finalUrl += "/" + part;
    });

    if (typeof optQueryParts != "undefined") {
        finalUrl += "?";
        $.each(optQueryParts, function (key, value) {
            finalUrl += key + "=" + value + "&";
        });
    }
    console.info(finalUrl);
    return finalUrl;
}


// Create region strings
function createRegionString(type) {
    var regionStrings = [];

    switch (type) {
        case "circle":
            var ra = hms_to_deg($("#CircleRA").val());
            var dec = dms_to_deg($("#CircleDec").val());
            var radius = $("#CircleRadius").val();
            regionStrings.push("CIRCLE J2000", ra, dec, radius);
            break;
        case "polygon":
            var pointPairs = $("#PolygonPoints").val().trim().split(/\n/);
            var points = ""
            $.each(pointPairs, function (n, pair) {
                pair = pair.split(",");
                ra = hms_to_deg(pair[0]);
                dec = dms_to_deg(pair[1]);
                points = [points, ra, dec].join(" ");
            })
            regionStrings.push("POLY J2000", points);
            break;
        case "costum":
            break;
    }

    return regionStrings.join(" ");
}

function createFootprintRegionRequest(regionString) {
    return {
        region: {
            fillFactor: 1.0,
            regionString: regionString
        }
    };
}

// refresh PlotCanvas
function refreshCanvas() {
    var d = new Date();
    $("#PlotCanvas").attr("src", createUrl(editorSvcUrl, ["plot?ts=" + d.getTime()]));
}

// SERVICE CALLS
// Setup Cookies
function setCookies() {
    $.ajax({
        url: createUrl(editorSvcUrl, ["reset"]),
        type: "GET",
        mimeType: 'text/html'
    });
}


// Building region creating ajax call
function addRegion(type, json) {
    var methodUrl = createUrl(editorSvcUrl, [type]);

    $.ajax({
        url: methodUrl,
        type: "POST",
        mimeType: 'text/html',
        contentType: "application/json",
        data: JSON.stringify(json)
    });
}

// grow region
function growRegion(radius) {
    var methodUrl = editorSvcUrl + "/grow?r=" + radius;

    $.ajax({
        url: methodUrl,
        type: "POST",
        mimeType: 'text/html',
        contentType: "text/plain"
    });
}

// get the shape of region
function getShape() {
    var methodUrl = editorSvcUrl + "/shape";
    $.ajax({
        url: methodUrl,
        type: "GET",
        headers: { Accept: "text/plain" }
    });
}

// Save region
function saveRegion() {
    var params = {
        owner: $("#SaveUserInput").val(),
        name: $("#SaveUserFootprintName").val(),
        region: $("#SaveUserRegionName").val()
    };

    var methodUrl = createUrl(editorSvcUrl, ["save"], params);
    $.ajax({
        url: methodUrl,
        type: "POST",
        mimeType: 'text/html',
        contentType: "text/plain"
    });
}


// Load region 
function loadRegion() {
    var params = {
        owner: $("#SaveUserInput").val(),
        name: $("#FootprintSelect option:selected").text(),
        region: $("#RegionSelect option:selected").text()
    };
    var methodUrl = createUrl(editorSvcUrl, ["load"], params);

    $.ajax({
        url: methodUrl,
        type: "GET",
        mimeType: 'text/html',
        contentType: "text/plain",
        headers: { Accept: "text/plain" }
    });
}

function setOwner() {
    var methodUrl = createUrl(authSvcUrl, ["me"]);
    var owner = "";
    var request = $.ajax({
        url: methodUrl,
        type: "GET",
        mimeType: 'text/html',
        contentType: "text/plain",
        success: function (data) {
            owner = $(data).find("name").text();
            $("#SaveUserInput").val(owner);
        }
    });

}


// set Footprint list of User
function setFootprintList() {
    var serviceUrl = createUrl(footprintSvcUrl, ["footprints"]);
    console.log(serviceUrl);
    $.get(serviceUrl, function (data, status) {
        $(data).find("footprint").each(function () {
            $("#FootprintSelect").append($('<option>').append($(this).find("owner").text()+"/"+$(this).find("name").text()));
        })
    });
}


// GET selected folder and the footprints within
function setRegionList() {
    //var owner = $("#SaveUserInput").val();
    var owner = $("#FootprintSelect option:selected").text().split("/")[0];
    var footprint = $("#FootprintSelect option:selected").text().split("/")[1];
    var serviceUrl = createUrl(footprintSvcUrl, ["users", owner, "footprints", footprint, "regions"])
    $.get(serviceUrl, function (data, status) {
        var nameList = $(data).find("name").each(function () {
            $("#RegionSelect").append($("<option>").append($(this).text()));
        })
    });
}
// <----- SERVICE CALLS