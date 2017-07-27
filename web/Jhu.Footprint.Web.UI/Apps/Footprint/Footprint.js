// Declaration of the services' urls
var editorServiceUrl = "../../api/v1/editor.svc";
var footprintServiceUrl = "../../api/v1/footprint.svc";


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


// get query parameters
function getQueryParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

function setOwner() {
    var methodUrl = createUrl(authSvcUrl, ["me"]);
    var owner;
    var request = $.ajax({
        url: methodUrl,
        type: "GET",
        mimeType: 'text/html',
        contentType: "text/plain",
        success: function (data) {
            owner = $(data).find("name").text();
            localStorage.setItem('owner', owner);
        }
    });
}