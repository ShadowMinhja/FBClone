'use strict';

/**
 * @ngdoc function
 * @name fbCloneApp.controller:ShopSingleProductCtrl
 * @description
 * # ShopSingleProductCtrl
 * Controller of the fbCloneApp
 */
angular.module('fbCloneApp')
  .controller('SingleProductCtrl', function ($scope) {
    $scope.page = {
      title: 'Single Product',
      subtitle: 'Place subtitle here...'
    };
  });
