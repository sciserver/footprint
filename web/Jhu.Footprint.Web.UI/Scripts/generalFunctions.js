﻿var authSvcUrl = "http://localhost/auth/api/v1/auth.svc";

// The ajax function 
function serviceCall(url, type, optParams) {
    // set default values
    var mimeType = "text/html";
    var contentType = "text/plain";

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
        contentType: contentType
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


// set user
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