'use strict';

angular
    .module('common.services')
    .factory("answerResource",
        ["$resource", 
            "appSettings",
            "currentUser",
                answerResource])
    
function answerResource($resource, appSettings, currentUser) {
    return $resource(appSettings.serverPath + "/api/Answers/:id", null, 
        {
            'query': { //GET
                method: 'GET',
                isArray: true,
                headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().token }
            },
            'get': { //GET
                headers : { 'Authorization': 'Bearer ' + currentUser.getProfile().token }
            },
            'save': { //POST
                method: 'POST',
                headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().token }
            },
            'update': { //PUT
                method: 'PUT',
                headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().token }
            }
        });
}