var footprintSvcUrl = "http://localhost/footprint/api/v1/footprint.svc";
var editorSvcUrl = "http://localhost/footprint/api/v1/editor.svc";
var authSvcUrl = "http://localhost/auth/api/v1/auth.svc";

$(document).ready(function () {

    // Centering modals
    $("body").on('show.bs.modal', ".modal", function () {
        centerModals($(this));
    });
    $(window).on('resize', centerModals);

});