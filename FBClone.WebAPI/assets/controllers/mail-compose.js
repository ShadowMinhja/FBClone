'use strict';

/**
 * @ngdoc function
 * @name fbCloneApp.controller:MailComposeCtrl
 * @description
 * # MailComposeCtrl
 * Controller of the fbCloneApp
 */
angular.module('fbCloneApp')
  .controller('MailComposeCtrl', function ($scope) {
    $scope.availableRecipients = ['RLake@nec.gov','RBastian@lacus.io','VMonroe@orci.ly','YMckenzie@mattis.gov','VMcmyne@molestie.org','BKliban@aliquam.gov','HHellems@tincidunt.org','KAngell@sollicitudin.ly'];
  });
