'use strict';

angular
    .module('helper.services', [])
    .factory("helpers",
        helpers)

function helpers() {
    var createGuid = function () {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }

    var formatNumber = function (number) {
        return Math.round(number * 100) / 100;
    }

    var formatPercent = function (number) {
        return Math.round(number * 100 * 100) / 100;
    }

    var getDateFilter = function (date, filterDate, comparison) {
        var yearFilter = filterDate.format("YYYY");
        var monthFilter = filterDate.format("MM");
        var dayFilter = filterDate.format("DD");
        return " " + date + " " + comparison + " datetimeoffset'" + filterDate.format("YYYY-MM-DD") + "'";
    }

    var dataURItoBlob = function (dataURI) {
        var binary = atob(dataURI.split(',')[1]);
        var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0];
        var array = [];
        for (var i = 0; i < binary.length; i++) {
            array.push(binary.charCodeAt(i));
        }
        return new Blob([new Uint8Array(array)], { type: mimeString });
    }

    return {
        createGuid: createGuid,
        formatNumber: formatNumber,
        formatPercent: formatPercent,
        getDateFilter: getDateFilter,
        dataURItoBlob: dataURItoBlob,
    }
}