'use strict';

/**
 * @ngdoc function
 * @name fbCloneApp.controller:PagesTimelineCtrl
 * @description
 * # PagesTimelineCtrl
 * Controller of the fbCloneApp
 */
angular.module('fbCloneApp')
  .controller('TimelineCtrl', function ($scope) {
    $scope.page = {
      title: 'Timeline',
      subtitle: 'Place subtitle here...'
    };
  });
