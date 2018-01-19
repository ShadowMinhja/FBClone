'use strict';

angular.module('fbCloneApp')
    .factory("dashboardResource", ["$q", "$resource", "appSettings", "currentUser", dashboardResource])

function dashboardResource($q, $resource, appSettings, currentUser) {
    var resource = {};

    var DashboardStats = $resource(appSettings.serverPath + "/api/DashboardStats/:id", null,
    {
        'statusTileCount': { //GET
            method: 'GET',
            isArray: false,
            url: appSettings.serverPath + '/api/DashboardStats/GetStatusTileCount',
            headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
        },
        'statusTilesAvg': { //GET
            method: 'GET',
            isArray: true,
            url: appSettings.serverPath + '/api/DashboardStats/GetStatusTileAvg',
            headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
        },
        'positivitySpread': { //GET
            method: 'GET',
            isArray: true,
            url: appSettings.serverPath + '/api/DashboardStats/GetPositivitySpread',
            headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
        },
        'questionTextAvg': { //GET
            method: 'GET',
            isArray: true,
            url: appSettings.serverPath + '/api/DashboardStats/GetQuestionTextAvgScore',
            headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
        },
        'staffMemberAvg': { //GET
            method: 'GET',
            isArray: true,
            url: appSettings.serverPath + '/api/DashboardStats/GetStaffMemberAvgScore',
            headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
        },
    });

    resource.getStatusTileCount = function (params) {
        var deferred = $q.defer();
        DashboardStats.statusTileCount(params, function (data) {
            deferred.resolve(data);
        }, function (error) {
            deferred.reject('There was an error retrieving the question response sets.');
        });
        return deferred.promise;
    }

    resource.getStatusTileAvg = function (params) {
        var deferred = $q.defer();
        DashboardStats.statusTilesAvg(params, function (data) {
            deferred.resolve(data);
        }, function (error) {
            deferred.reject('There was an error retrieving the question response sets.');
        });
        return deferred.promise;
    }

    resource.getPositivitySpread = function (params) {
        var deferred = $q.defer();
        DashboardStats.positivitySpread(params, function (data) {
            deferred.resolve(data);
        }, function (error) {
            deferred.reject('There was an error retrieving the question response sets.');
        });
        return deferred.promise;
    }

    resource.getQuestionTextAvg = function (params) {
        var deferred = $q.defer();
        DashboardStats.questionTextAvg(params, function (data) {
            deferred.resolve(data);
        }, function (error) {
            deferred.reject('There was an error retrieving the question response sets.');
        });
        return deferred.promise;
    }
        
    resource.getStaffMemberAvg = function (params) {
        var deferred = $q.defer();
        DashboardStats.staffMemberAvg(params, function (data) {
            deferred.resolve(data);
        }, function (error) {
            deferred.reject('There was an error retrieving the question response sets.');
        });
        return deferred.promise;
    }
        
    return resource;
}