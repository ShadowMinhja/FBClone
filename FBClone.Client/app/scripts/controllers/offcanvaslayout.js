'use strict';

/**
 * @ngdoc function
 * @name fbCloneApp.controller:OffcanvaslayoutCtrl
 * @description
 * # OffcanvaslayoutCtrl
 * Controller of the fbCloneApp
 */
angular.module('fbCloneApp')
  .controller('OffcanvaslayoutCtrl', function ($scope) {
    $scope.page = {
      title: 'Off-canvas sidebar',
      subtitle: 'On small devices'
    };
  });
