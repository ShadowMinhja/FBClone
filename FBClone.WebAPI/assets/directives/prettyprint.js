'use strict';

/**
 * @ngdoc directive
 * @name fbCloneApp.directive:prettyprint
 * @description
 * # prettyprint
 */
/* jshint ignore:start */
angular.module('fbCloneApp')
  .directive('prettyprint', function () {
    return {
      restrict: 'C',
      link: function postLink(scope, element) {
        element.html(prettyPrintOne(element.html(),'',true));
      }
    };
  });
/* jshint ignore:end */
