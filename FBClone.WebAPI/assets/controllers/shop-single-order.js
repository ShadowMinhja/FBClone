'use strict';

/**
 * @ngdoc function
 * @name fbCloneApp.controller:ShopSingleOrderCtrl
 * @description
 * # ShopSingleOrderCtrl
 * Controller of the fbCloneApp
 */
angular.module('fbCloneApp')
  .controller('SingleOrderCtrl', function ($scope) {
    $scope.page = {
      title: 'Single Order',
      subtitle: 'Place subtitle here...'
    };
  });
