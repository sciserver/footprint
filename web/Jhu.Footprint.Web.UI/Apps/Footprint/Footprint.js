// Declaration of the services' urls
var editorServiceUrl = "../../api/v1/editor.svc";
var footprintServiceUrl = "../../api/v1/footprint.svc";

function callService(url, type, optParams, success, error) {
    var mimeType = "text/html";
    var contentType = "application/json";
    var dataType = "application/json";

    if (typeof optParams != "undefined") {
        $.each(optParams, function (key, value) {
            console.log(key + ": " + value);
            switch (key) {
                case "mimeType":
                    mimeType = value;
                    break;
                case "contentType":
                    contentType = value;
                    break;
                case "headers":
                    $.ajaxSetup({ headers: value });
                    break;
                case "data":
                    $.ajaxSetup({ data: value });
                    break;
            };
        });
    }

    $.ajax({
        url: url,
        type: type,
        mimeType: mimeType,
        contentType: contentType,
        dataType: "json",
        headers: {
            Accept: dataType,
        },
        success: success,
        error: error
    });
}


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