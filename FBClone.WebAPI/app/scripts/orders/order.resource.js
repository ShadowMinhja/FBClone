'use strict';

angular
    .module('fbCloneApp')
    .factory("orderResource",
        ["$resource", "$q", "appSettings", "currentUser", orderResource])
    
function orderResource($resource, $q, appSettings, currentUser) {
    var orderResource = this;

    orderResource.createOrder = createOrder;
    orderResource.updateOrder = updateOrder;
    orderResource.getOrders = getOrders;

    var params = {
    };

    var Orders = $resource(appSettings.serverPath + "/api/Order/:id", params,
    {
            'query': { //GET
                method: 'GET',
                isArray: true,
                headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
            },
            'get': { //GET
                method: 'GET',
                isArray: true,
                headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
            },
            'save': { //POST
                method: 'POST',
                isArray: false,
                headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
            },
            'update': { //PUT
                method: 'PUT',
                headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().access_token }
            }
    });

    function getOrders() {
        var deferred = $q.defer();
        Orders.query().$promise.then(function (result) {
            deferred.resolve(result);
        }, function (error) {
            deferred.reject(error);
        });
        return deferred.promise;
    }

    function createOrder(newOrder) {
        var deferred = $q.defer();
        Orders.save(newOrder).$promise.then(function (result) {
            deferred.resolve(result);
        }, function (error) {
            deferred.reject(error);
        });
        return deferred.promise;
    }

    function updateOrder(order) {
        var deferred = $q.defer();
        Orders.update(order).$promise.then(function (result) {
            deferred.resolve(result);
        }, function (error) {
            deferred.reject(error);
        });
        return deferred.promise;
    }

    return orderResource;
}