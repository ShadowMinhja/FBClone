'use strict';

angular
    .module('common.services')
    .factory("stateProvinceResource",
        ["$resource", 
            "appSettings",
            "currentUser",
                stateProvinceResource])
    
function stateProvinceResource($resource, appSettings, currentUser) {
    return $resource(appSettings.serverPath + "/api/StateProvinces/", null, 
        {
            'query': { //GET
                method: 'GET',
                isArray: true,
                headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
            }
        });
}