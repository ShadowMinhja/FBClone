'use strict';

angular.module('fbCloneApp')
    .service("applicationSettingService", ["$http", "appSettings", "currentUser", applicationSettingService])
        
function applicationSettingService($http, appSettings, currentUser) {
    var applicationSettingService = this;
    applicationSettingService.settings = null;
    applicationSettingService.locations = null;

    applicationSettingService.retrieveSettings = function () {
        var req = {
            method: 'GET',
            url: appSettings.serverPath + "/api/ApplicationSettings",
            headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
        };
        return $http(req).then(function (result) {
            return result.data;
        });
    }

    return applicationSettingService;
}