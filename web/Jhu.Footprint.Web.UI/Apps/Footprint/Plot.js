$(document).ready(function () {

    // Centering modals
    $("body").on('show.bs.modal', ".modal", function () {
        centerModals($(this));
    });
    $(window).on('resize', centerModals);

});