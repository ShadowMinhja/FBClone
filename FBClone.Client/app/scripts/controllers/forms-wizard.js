'use strict';

/**
 * @ngdoc function
 * @name fbCloneApp.controller:FormsWizardCtrl
 * @description
 * # FormsWizardCtrl
 * Controller of the fbCloneApp
 */
angular.module('fbCloneApp')
  .controller('FormWizardCtrl', function ($scope) {
    $scope.page = {
      title: 'Form Wizard',
      subtitle: 'Place subtitle here...'
    };
  });
