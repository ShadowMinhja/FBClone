'use strict';

/**
 * @ngdoc function
 * @name fbCloneApp.controller:PagesProfileCtrl
 * @description
 * # PagesProfileCtrl
 * Controller of the fbCloneApp
 */
angular.module('fbCloneApp')
  .controller('ProfileCtrl', function ($scope) {
    $scope.page = {
      title: 'Profile Page',
      subtitle: 'Place subtitle here...'
    };
  });
