function ServiceBase() {
    this.serviceUrl = null;
}

ServiceBase.prototype.error = function (xhr, status, message) {
    alert(message);
}

ServiceBase.prototype.createUrl = function (pathParts, optQueryParts) {
    var finalUrl = this.serviceUrl;
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

ServiceBase.prototype.callService = function (url, type, optParams, success) {
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
        error: this.error
    });
}