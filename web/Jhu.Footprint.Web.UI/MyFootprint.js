var footprintSvcUrl = "http://localhost/footprint/api/v1/footprint.svc";

$(document).one('ready', function () {
    // Set owner
    // TODO: include group
    setOwner();
});

$(document).ready(function () {

    var user = localStorage.getItem("owner");
    $(document).on("click", "#DeleteFootprints", function () {
        var $footprints = $("input[name='deleteCheckbox']:checked");
        $footprints.each(function () {
            var footprint = $(this).val()
            deleteFootprint(user, footprint);
        });
    })
});

// delete region
function deleteFootprint(user, footprint) {
    var methodUrl = createUrl(footprintSvcUrl, ["users", user, "footprints", footprint]);
    serviceCall(methodUrl, "DELETE");
}
// edit region