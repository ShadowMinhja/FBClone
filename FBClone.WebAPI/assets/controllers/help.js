'use strict';

/**
 * @ngdoc function
 * @name fbCloneApp.controller:HelpCtrl
 * @description
 * # HelpCtrl
 * Controller of the fbCloneApp
 */
angular.module('fbCloneApp')
  .controller('HelpCtrl', function ($scope) {
     $scope.page = {
      title: 'Documentation',
      subtitle: 'Place subtitle here...'
    };
  });
