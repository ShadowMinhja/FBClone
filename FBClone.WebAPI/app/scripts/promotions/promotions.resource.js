'use strict';

angular
    .module('common.services')
    .factory("promotionsResource",
        ["$http",
            "appSettings",
            "currentUser",
                promotionsResource])

function promotionsResource($http, appSettings, currentUser) {
    var req = {
        method: 'GET',
        url: appSettings.serverPath + "/api/Promotions",
        headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
    };
    return {
        getPromoUrl: function () {
            return $http(req).then(function (result) {
                return result.data;
            });
        }
    }
}