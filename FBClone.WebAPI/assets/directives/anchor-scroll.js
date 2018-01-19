'use strict';

/**
 * @ngdoc directive
 * @name fbCloneApp.directive:anchorScroll
 * @description
 * # anchorScroll
 */
angular.module('fbCloneApp')
  .directive('anchorScroll', ['$location', '$anchorScroll', function($location, $anchorScroll) {
    return {
      restrict: 'AC',
      link: function(scope, el, attr) {
        el.on('click', function(e) {
          $location.hash(attr.anchorScroll);
          $anchorScroll();
        });
      }
    };
  }]);
