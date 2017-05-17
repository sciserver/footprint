var footprintSvcUrl = "http://localhost/footprint/api/v1/footprint.svc";

$(document).one('ready', function () {
    // Set owner
    // TODO: include group
    setOwner();
});

$(document).ready(function () {

    var user = localStorage.getItem("owner");
    // delete footprints
    $(document).on("click", "#SelectFootprints", function () {
        $(this).addClass("hidden");
        $(".DeleteFootprints").removeClass("hidden");
    });


    $(document).on("click", "#DeleteFootprints", function () {
        var $footprints = $("input[name='deleteCheckbox']:checked");
        $footprints.each(function () {
            var footprint = $(this).val();
            deleteFootprint(user, footprint);
        });

        $(".DeleteFootprints").addClass("hidden");
        $("#SelectFootprints").removeClass("hidden");
        __doPostBack($(this));
    });
});

// delete footprints
function deleteFootprint(user, footprint) {
    var methodUrl = createUrl(footprintSvcUrl, ["users", user, "footprints", footprint]);
    serviceCall(methodUrl, "DELETE");
}