'use strict';

/**
 * @ngdoc function
 * @name fbCloneApp.controller:PagesChatCtrl
 * @description
 * # PagesChatCtrl
 * Controller of the fbCloneApp
 */
angular.module('fbCloneApp')
  .controller('ChatCtrl', function ($scope, $resource) {
    $scope.inbox = $resource('scripts/jsons/chats.json').query();

    $scope.archive = function(index) {
      $scope.inbox.splice(index, 1);
    };
  });
