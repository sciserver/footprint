﻿// convert HHMMSS to degree
function hms_to_deg(numString) {
    regex = /[: hms]/;
    var hms = numString.split(regex).filter(function (v) { return v !== '' });
    if (hms.length = 3) {
        var num = (parseInt(hms[0]) + parseInt(hms[1]) / 60. + parseInt(hms[2]) / 3600.) * 15;
    }
    else {
        num = parseInt(hms);
    }
    return num;
}

// convert DDMMSS to degree
function dms_to_deg(numString) {
    regex = /[: dm°'"]/;
    var dms = numString.split(regex).filter(function (v) { return v !== '' });
    if (dms.length = 3) {
        var num = (parseInt(dms[0]) + parseInt(dms[1]) / 60. + parseInt(dms[2]) / 3600.);
    }
    else {
        num = parseInt(dms);
    }
    return num;
}
