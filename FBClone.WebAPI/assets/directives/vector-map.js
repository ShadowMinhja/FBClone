'use strict';

/**
 * @ngdoc directive
 * @name fbCloneApp.directive:vectorMap
 * @description
 * # vectorMap
 */
angular.module('fbCloneApp')
  .directive('vectorMap', function () {
    return {
      restrict: 'AE',
      scope: {
        options: '='
      },
      link: function postLink(scope, element) {
        var options = scope.options;
        element.vectorMap(options);
      }
    };
  });
