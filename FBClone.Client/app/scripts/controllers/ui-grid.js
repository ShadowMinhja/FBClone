'use strict';

/**
 * @ngdoc function
 * @name fbCloneApp.controller:UiGridCtrl
 * @description
 * # UiGridCtrl
 * Controller of the fbCloneApp
 */
angular.module('fbCloneApp')
  .controller('GridCtrl', function ($scope) {
    $scope.page = {
      title: 'Grid',
      subtitle: 'Place subtitle here...'
    };
  });
