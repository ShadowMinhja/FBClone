'use strict';

/**
 * @ngdoc function
 * @name fbCloneApp.controller:PagesLoginCtrl
 * @description
 * # PagesLoginCtrl
 * Controller of the fbCloneApp
 */
angular.module('fbCloneApp')
  .controller('LoginCtrl', function ($scope, $state) {
    $scope.login = function() {
      $state.go('app.dashboard');
    };
  });
