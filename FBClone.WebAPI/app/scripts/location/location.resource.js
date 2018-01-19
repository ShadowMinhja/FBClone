'use strict';

angular
    .module('fbCloneApp')
    .factory("locationResource",
        ["$resource", 
            "appSettings",
            "currentUser",
                locationResource])
    
function locationResource($resource, appSettings, currentUser) {
    return $resource(appSettings.serverPath + "/api/Locations/:id", null, 
        {
            'query': { //GET
                method: 'GET',
                isArray: true,
                headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
            },
            'get': { //GET
                headers : { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
            },
            'save': { //POST
                method: 'POST',
                headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
            },
            'update': { //PUT
                method: 'PUT',
                headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
            },
            'checkClaimed': {
                method: 'GET',
                isArray: true,
                headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
            }
        });
}

angular
    .module('fbCloneApp')
    .factory("locationResourceCheck",
        ["$resource", 
            "appSettings",
            "currentUser",
                locationResourceCheck])
    
function locationResourceCheck($resource, appSettings, currentUser) {
    return $resource(appSettings.serverPath + "/api/Locations/CheckClaimed?placeId=:placeId", null, 
        {
            'query': { //GET
                method: 'GET',
                headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
            }
        });
}