﻿// Declaration of the services' urls
// TODO: maybe should be done in a more proper way?
var footprintSvcUrl = "http://localhost/footprint/api/v1/footprint.svc";
var editorSvcUrl = "http://localhost/footprint/api/v1/editor.svc";

$(document).one('ready', function () {
    // Set owner
    // TODO: include group
    setOwner();
    user = localStorage.getItem("owner");
    $("#SaveUserInput").val(user);

    // Set Save region parameters
    setSaveRegionParameters();
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
    $("body").on("click", "#LoadRegionButton", function () {
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


// Building region creating ajax call
function addRegion(type, json) {
    var methodUrl = createUrl(editorSvcUrl, [type]);

    serviceCall(methodUrl, "POST", { contentType: "application/json", data: JSON.stringify(json) });
}

// grow region
function growRegion(radius) {
    var methodUrl = editorSvcUrl + "/grow?r=" + radius;

    serviceCall(methodUrl, "POST");
}

// Save region
function saveRegion() {
    var params = {
        owner: $("#SaveUserInput").val(),
        name: $("#SaveUserFootprintName").val(),
        region: $("#SaveUserRegionName").val(),
        method: $("input:radio[name=FootprintCombinationMethod]:checked").val()
    };

    var methodUrl = createUrl(editorSvcUrl, ["save"], params);

    serviceCall(methodUrl, "POST");
}

function setSaveRegionParameters() {
    var footprintName = getQueryParameterByName("footprintName");
    var regionName = getQueryParameterByName("regionName");
    if (footprintName && regionName )
    {
        $("#SaveUserFootprintName").val(footprintName);
        $("#SaveUserRegionName").val(regionName);
    }

}

// <----- Ajax 



// Unnecessary? 
/*
// get the shape of region
function getShape() {
    var methodUrl = editorSvcUrl + "/shape";
    $.ajax({
        url: methodUrl,
        type: "GET",
        headers: { Accept: "text/plain" }
    });
}


// Setup Cookies
function setCookies() {
    $.ajax({
        url: createUrl(editorSvcUrl, ["reset"]),
        type: "GET",
        mimeType: 'text/html'
    });
}
*/