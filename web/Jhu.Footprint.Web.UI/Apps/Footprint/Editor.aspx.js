var editorService;
var footprintService;

var editorCanvas;
var regionList;
var confirmModal;
var circleModal;
var rectangleModal;
var customRegionModal;
var multipointRegionModal;
var combinedRegionModal;
var saveFootprintModal;

$.getScript()

$(document).ready(function () {

    editorService = new EditorService("../../Api/V1/Editor.Svc");
    footprintService = new FootprintService("../../Api/V1/Footprint.Svc");

    editorCanvas = new EditorCanvas($("#editorCanvas")[0], editorService);
    regionList = new EditorRegionList($("#regionList")[0], editorService);
    confirmModal = new ConfirmModal($("#confirmModal")[0]);
    circleModal = new CircleModal($("#circleModal")[0]);
    rectangleModal = new RectangleModal($("#rectangleModal")[0]);
    customRegionModal = new CustomRegionModal($("#customRegionModal")[0]);
    multipointRegionModal = new MultipointRegionModal($("#multipointRegionModal")[0]);
    combinedRegionModal = new CombinedRegionModal($("#combinedRegionModal")[0]);
    saveFootprintModal = new SaveFootprintModal($("#saveFootprintModal")[0]);

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
        event.preventDefault();
        confirmModal.message("Do you really want to delete the selected regions?");
        confirmModal.on("ok",function () {
            var regions = regionList.getSelection();
            editorService.deleteFootprintRegions(regions, function () {
                refreshAll();
                confirmModal.hide();
            });
        });
        confirmModal.show();
    });

    $("#clear").on("click", function (event) {
        event.preventDefault();
        confirmModal.message("Do you really want to delete all regions?");
        confirmModal.on("ok", function () {
            editorService.deleteFootprintRegions([ "*" ], function () {
                refreshAll();
                confirmModal.hide();
            });
        });
        confirmModal.show();
    });

    regionList.on("click", function (item) {
        editorCanvas.delayedRefresh(getPlotParameters());
    });


    $("#circle").on("click", function (event) {
        event.preventDefault();
        var name = regionList.generateUniqueName("new_circle");
        circleModal.show({ regionName: name });
    });

    circleModal.on("ok", function () {
        createRegion(circleModal.region(), function() {
            circleModal.hide();
        });
    });

    $("#rectangle").on("click", function (event) {
        event.preventDefault();
        var name = regionList.generateUniqueName("new_rect");
        rectangleModal.show({ regionName: name });
    });

    rectangleModal.on("ok", function () {
        createRegion(rectangleModal.region(), function () {
            rectangleModal.hide();
        });
    });

    $("#polygon").on("click", function (event) {
        event.preventDefault();
        var name = regionList.generateUniqueName("new_poly");
        multipointRegionModal.show({ regionName: name, mode: "poly" });
    });

    $("#cHull").on("click", function (event) {
        event.preventDefault();
        var name = regionList.generateUniqueName("new_chull");
        multipointRegionModal.show({ regionName: name, mode: "chull" });
    });

    multipointRegionModal.on("ok", function () {
        createRegion(multipointRegionModal.region(), function () {
            multipointRegionModal.hide();
        });
    });

    $("#customRegion").on("click", function (event) {
        event.preventDefault();
        var name = regionList.generateUniqueName("new_region");
        customRegionModal.show({ regionName: name });
    });

    customRegionModal.on("ok", function () {
        createRegion(customRegionModal.region(), function () {
            customRegionModal.hide();
        });
    });

    $("#union").on("click", function (event) {
        event.preventDefault();
        if (regionList.getSelection().length < 2) {
            alert("At least two regions must be selected.");
            return;
        }
        var name = regionList.generateUniqueName("new_union");
        combinedRegionModal.clearFirstRegionList();
        combinedRegionModal.show(
            {
                regionName: name,
                combineMethod: "union",
                firstRegionVisible: false
            });
    });

    $("#intersect").on("click", function (event) {
        event.preventDefault();
        if (regionList.getSelection().length < 2) {
            alert("At least two regions must be selected.");
            return;
        }
        var name = regionList.generateUniqueName("new_intersect");
        combinedRegionModal.clearFirstRegionList();
        combinedRegionModal.show(
            {
                regionName: name,
                combineMethod: "intersect",
                firstRegionVisible: false
            });
    });

    $("#subtract").on("click", function (event) {
        event.preventDefault();
        if (regionList.getSelection().length < 2) {
            alert("At least two regions must be selected.");
            return;
        }
        var name = regionList.generateUniqueName("new_difference");
        combinedRegionModal.refreshFirstRegionList(regionList.getSelection());
        combinedRegionModal.show(
            {
                regionName: name,
                combineMethod: "subtract",
                firstRegionVisible: true
            });
    });

    combinedRegionModal.on("ok", function () {
        combineRegions(function () {
            combinedRegionModal.hide();
        })
    });

    $("#save").on("click", function (event) {
        event.preventDefault();
        if (!$("#owner").val()) {
            alert("To save footprints, you have to register and sign in.");
            return;
        }
        if (regionList.getSelection().length == 0) {
            alert("At least one region has to be added to the footprint.")
            return;
        }
        saveFootprintModal.show();
    });

    saveFootprintModal.on("ok", function () {
        saveFootprint(function () {
            alert("Footprint saved successfully.");
            saveFootprintModal.hide();
        })
    });

    refreshAll();
})

function createRegion(region, success) {
    editorService.createFootprintRegion(
        region.name, { region: region },
        function (result) {
            addRegion(result);
            if (success) success();
        });
}

function combineRegions(success) {
    var region = combinedRegionModal.region();
    var keepOriginal = combinedRegionModal.keepOriginal();
    var firstRegion = combinedRegionModal.firstRegion();
    var selection = regionList.getSelection();
    
    // Remove first region from sources and add it to the very beginning
    var sources = [];
    if (firstRegion) sources.push(firstRegion);
    for (i = 0; i < selection.length; i++) {
        if (selection[i] != firstRegion) sources.push(selection[i]);
    }

    var request = {
        region: combinedRegionModal.region(),
        sources: sources
    };

    editorService.combineFootprintRegions(
        region.name, combinedRegionModal.combineMethod, keepOriginal, request,
        function (result) {
            if (combinedRegionModal.keepOriginal()) {
                addRegion(result);
            } else {
                refreshAll();
            }
            if (success) success(result);
        });
}

function saveFootprint(success) {
    var owner = $("#owner").val();
    request = {
        footprint: {},
        source: "editor"
    }
    footprintService.copyUserFootprint(owner, saveFootprintModal.footprintName(), request,
        function (result) {
            if (success) success(result);
        });
}

function addRegion(result) {
    regionList.appendItem(result.region);
    regionList.applySelection([result.region.name]);
    editorCanvas.refresh(getPlotParameters());
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