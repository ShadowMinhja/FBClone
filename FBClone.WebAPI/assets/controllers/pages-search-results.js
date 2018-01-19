'use strict';

/**
 * @ngdoc function
 * @name fbCloneApp.controller:PagesSearchResultsCtrl
 * @description
 * # PagesSearchResultsCtrl
 * Controller of the fbCloneApp
 */
angular.module('fbCloneApp')
  .controller('SearchResultsCtrl', function ($scope) {
    $scope.page = {
      title: 'Search Results',
      subtitle: 'Place subtitle here...'
    };
  });
