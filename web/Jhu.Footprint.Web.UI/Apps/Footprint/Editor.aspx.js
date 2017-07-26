var regionCombineMode = null;

$(document).ready(function () {

    // Event handlers

    $("#newCircle").on("click", function (event) {
        event.preventDefault();
        $("#circleModal").modal("show");
    });

    $("#refresh").on("click", function (event) {
        event.preventDefault();
        refreshAll();
    });

    $("#circleModalOk").on("click", function (event) {
        event.preventDefault();
        addRegion($("#circleName").val(), getRegionCircle());
    });

    refreshAll();
})

// Local functions

function refreshAll() {
    refreshCanvas();
    refreshRegionList();
}

function getPlotParameters() {
    return {
        "proj": $("#projection").val(),
        "sys": $("#equatorial")[0].checked ? "eq" : "gal",
        // ra
        // dec
        // b
        // l
        "width": $("#canvas").width(),
        "height": $("#canvas").height(),
        // theme
        "zoom": $("#autoZoom")[0].checked,
        "rotate": $("#autoRotate")[0].checked,
        "grid": $("#grid")[0].checked,
        "degStyle": $("#decimal")[0].checked ? "decimal" : "hms",
        "ts": new Date().getTime()
    };
}

function refreshCanvas() {
    var plot = getPlotParameters();
    var url = createUrl(editorServiceUrl, ["footprint", "plot"], plot);
    $("#canvas").css('background-image', 'url(' + url + ')');
}

function refreshRegionList() {
    var url = createUrl(editorServiceUrl, ["footprint", "regions"]);
    callService(url, "GET", null,
        function (result, status, xhr) {
            clearRegionList();
            $.each(result.regions, function (key, value) {
                appendRegionListItem(value)
            });
        },
        function (xhr, status, error) {

        });
}

function clearRegionList()
{
    var list = $("#regionList");
    list.empty();
}

function appendRegionListItem(region)
{
    var html = '<div class="gw-list-item">';
    html += '<span style="width: 24px"><input type="checkbox" data-item="' + region.name + '" /></span>';
    html += '<span class="gw-list-span">' + region.name + '</span>';
    html += '</div>';

    var list = $("#regionList");
    list.append(html);
}

function getRegionCircle() {
    return "CIRCLE J2000 " + $("#circleCenterRa").val() + " " + $("#circleCenterDec").val() + " " + $("#circleRadius").val();
}

function addRegion(regionName, regionString) {
    var url = createUrl(editorServiceUrl, ["footprint", "regions", regionName]);
    var request = {
        contentType: "application/json",
        data: JSON.stringify({
            region: {
                fillFactor: 1.0,
                name: regionName,
                regionString: regionString
            }
        })
    };
    callService(url, "POST", request,
        function (result, status, xhr) {
            appendRegionListItem(result.region);
            refreshCanvas();
        },
        function (xhr, status, error) {
        });

    $(".modal").modal("hide");
}

// 

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