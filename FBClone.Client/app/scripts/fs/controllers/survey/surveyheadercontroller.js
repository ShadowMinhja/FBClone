'use strict';

angular.module('fbCloneApp')

  .controller('SurveyHeaderCtrl', ["$scope", function ($scope) {
        $scope.page = {
            title: 'Manage Surveys',
            subtitle: 'Build Questionnaries to Learn More about Your Business',
        };
    }]);