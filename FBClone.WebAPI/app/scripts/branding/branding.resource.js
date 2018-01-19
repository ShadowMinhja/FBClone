'use strict';

angular.module('fbCloneApp')
    .factory("brandingResource",
        ["$http", 
            "appSettings",
            "currentUser",
                brandingResource])
    
function brandingResource($http, appSettings, currentUser) {
    var req = {
        method: 'GET',
        url: appSettings.serverPath + "/api/Branding",
        headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
    };
    return {
        getLogoUrl: function () {
            return $http(req).then(function (result) {
                return result.data;
            });
        }
    }
}