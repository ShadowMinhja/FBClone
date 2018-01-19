'use strict';

angular.module('fbCloneApp')

  .controller('mtSwipeCtrl', function($scope, $timeout, $mdBottomSheet) {

    $scope.page = {
      title: 'Swipe',
      subtitle: 'Place subtitle here...'
    };

    $scope.onSwipeLeft = function(ev) {
      alert('You swiped left!!');
    };
    $scope.onSwipeRight = function(ev) {
      alert('You swiped right!!');
    };

  });




