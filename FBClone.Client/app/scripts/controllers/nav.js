'use strict';

/**
 * @ngdoc function
 * @name fbCloneApp.controller:NavCtrl
 * @description
 * # NavCtrl
 * Controller of the fbCloneApp
 */
angular.module('fbCloneApp')
  .controller('NavCtrl', function ($scope) {
    $scope.oneAtATime = false;

    $scope.status = {
      isFirstOpen: true,
      isSecondOpen: true,
      isThirdOpen: true
    };

  });
