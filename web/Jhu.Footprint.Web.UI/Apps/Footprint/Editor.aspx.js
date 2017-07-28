var editorService = new EditorService("../../Api/V1/Editor.Svc");

var editorCanvas;
var regionList;
var circleModal;
var rectangleModal;
var customRegionModal;
var multipointRegionModal;

$(document).ready(function () {

    editorCanvas = new EditorCanvas($("#editorCanvas")[0], editorService);
    regionList = new EditorRegionList($("#regionList")[0], editorService);
    circleModal = new CircleModal($("#circleModal")[0]);
    rectangleModal = new RectangleModal($("#rectangleModal")[0]);
    customRegionModal = new CustomRegionModal($("#customRegionModal")[0]);
    multipointRegionModal = new MultipointRegionModal($("#multipointRegionModal")[0]);

    // Event handlers

    $("#refresh").on("click", function (event) {
        event.preventDefault();
        refreshAll();
    });

    $("#projection").on("click", function (event) {
        editorCanvas.delayedRefresh(getPlotParameters());
    });

    $("#viewDropdown").on("click", "input", function (event) {
        editorCanvas.delayedRefresh(getPlotParameters());
    });

    $("#delete").on("click", function (event) {
        regions = regionList.getSelection();
        editorService.deleteFootprintRegions(regions, function () {
            refreshAll();
        });
    });

    regionList.on("click", function (item) {
        editorCanvas.delayedRefresh(getPlotParameters());
    });


    $("#newCircle").on("click", function (event) {
        event.preventDefault();
        var name = "new_circle_" + (regionList.count() + 1);
        circleModal.show({ circleName: name });
    });

    circleModal.on("ok", function (region) {
        addRegion(region);
        circleModal.hide();
    });

    $("#newRectangle").on("click", function (event) {
        event.preventDefault();
        var name = "new_rect_" + (regionList.count() + 1);
        rectangleModal.show({ rectangleName: name });
    });

    rectangleModal.on("ok", function (region) {
        addRegion(region, function () {
            rectangleModal.hide();
        });
    });

    $("#newPolygon").on("click", function (event) {
        event.preventDefault();
        var name = "new_poly_" + (regionList.count() + 1);
        multipointRegionModal.show({ multipointRegionName: name, multipointRegionPoly: true });
    });

    $("#newCHull").on("click", function (event) {
        event.preventDefault();
        var name = "new_chull_" + (regionList.count() + 1);
        multipointRegionModal.show({ multipointRegionName: name, multipointRegionCHull: true });
    });

    multipointRegionModal.on("ok", function (region) {
        addRegion(region, function () {
            multipointRegionModal.hide();
        });
    });

    $("#newCustomRegion").on("click", function (event) {
        event.preventDefault();
        var name = "new_region_" + (regionList.count() + 1);
        customRegionModal.show({ customRegionName: name });
    });

    customRegionModal.on("ok", function (region) {
        addRegion(region, function () {
            customRegionModal.hide();
        });
    });

    refreshAll();
})

function addRegion(region, success) {
    editorService.createFootprintRegion(
        region.name, { region: region },
        function (result) {
            regionList.appendItem(result.region);
            regionList.applySelection([result.region.name]);
            editorCanvas.refresh(getPlotParameters());
            if (success) success();
        });
}

function refreshAll() {
    editorCanvas.refresh(getPlotParameters());
    regionList.refreshList();
}

function getPlotParameters() {
    var plot = {
        "proj": $("#projection").val(),
        "sys": $("#equatorial")[0].checked ? "eq" : "gal",
        // ra
        // dec
        // b
        // l
        // width
        // height
        // theme
        "zoom": $("#autoZoom")[0].checked,
        "rotate": $("#autoRotate")[0].checked,
        "grid": $("#grid")[0].checked,
        "degStyle": $("#decimal")[0].checked ? "decimal" : "hms",
        "highlights": regionList.getSelection().join(","),
    };
    return plot;
}



// ---------------
// TODO: delete

/*
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


*/

/*
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