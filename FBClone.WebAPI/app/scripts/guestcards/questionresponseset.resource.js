'use strict';

angular.module('common.services')
    .factory("questionresponsesetResource", ["$q", "$resource", "appSettings", "currentUser", questionresponsesetResource])

function questionresponsesetResource($q, $resource, appSettings, currentUser) {
    var resource = {};

    var QuestionResponseSets = $resource(appSettings.serverPath + "/api/QuestionResponseSets/:id", null,
    {
        'query': { //GET
            method: 'GET',
            isArray: false,
            headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
        },
        'get': { //GET
            headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
        }
    });

    resource.getQuestionResponseSets = function (params) {
        var deferred = $q.defer();
        QuestionResponseSets.query(params,function (data) {
            deferred.resolve(data);
        }, function (error) {
            deferred.reject('There was an error retrieving the question response sets.');
        });
        return deferred.promise;
    }

    return resource;
}