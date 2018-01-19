'use strict';

angular.module('fbCloneApp')
    .factory("staffResource",
        ["$resource", 
            "appSettings",
            "currentUser",
                staffResource])
    
function staffResource($resource, appSettings, currentUser) {
    return $resource(appSettings.serverPath + "/api/StaffMembers/:id", null, 
        {
            'query': { //GET
                method: 'GET',
                isArray: true,
                headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
            },
            'get': { //GET
                headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
            },
            'save': { //POST
                method: 'POST', 
                headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
            },
            'update': { //PUT
                method: 'PUT',
                headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
            }
        });
}