'use strict';
angular.module('fbCloneApp')
    .factory('receiveOrderService', ['$', '$rootScope', receiveOrderService])
    
    function receiveOrderService($, $rootScope) {
        var service = {
            hub: null,
            initialize: initialize,
            completeOrder: completeOrder,
            //sendRequest: sendRequest  //Unused
        };
        return service;

        // ***************************************************************
        function initialize() {
            //Getting the connection object
            var connection = $.hubConnection();

            //Creating proxy
            service.hub = connection.createHubProxy('ordersHub');

            //Publishing an event when server pushes a greeting message
            service.hub.on('receiveOrder', function (message) {
                $rootScope.$emit("receiveOrder", message);
            });

            //Starting connection
            connection.start()
                .done(function () { console.log('Now connected, connection ID=' + connection.id); })
                .fail(function () { console.log('Could not Connect!'); });

        }

        function completeOrder(order) {
            //Invoking greetAll method defined in hub
            service.hub.invoke('completeOrder', order);
        }

        function sendRequest() {
            //Invoking greetAll method defined in hub
            service.hub.invoke('sendOrder');
        }

    }