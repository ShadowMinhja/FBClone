'use strict';

/**
 * @ngdoc function
 * @name fbCloneApp.controller:UiPortletsCtrl
 * @description
 * # UiPortletsCtrl
 * Controller of the fbCloneApp
 */
angular.module('fbCloneApp')
  .controller('PortletsCtrl', function ($scope) {
    $scope.page = {
      title: 'Portlets',
      subtitle: 'Place subtitle here...'
    };
  });
