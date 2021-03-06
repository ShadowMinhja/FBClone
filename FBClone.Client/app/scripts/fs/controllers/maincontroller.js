'use strict';

/**
 * @ngdoc function
 * @name fbCloneApp.controller:MainCtrl
 * @description
 * # MainCtrl
 * Controller of the fbCloneApp
 */
angular.module('fbCloneApp')
  .controller('MainCtrl', ["$scope", "$http", "$translate", "localStorageService", "currentUser", function ($scope, $http, $translate, localStorageService, currentUser) {

      $scope.main = {
          title: 'fbClone',
          settings: {
              navbarHeaderColor: 'scheme-default',
              sidebarColor: 'scheme-default',
              brandingColor: 'scheme-default',
              activeColor: 'default-scheme-color',
              headerFixed: true,
              asideFixed: true,
              rightbarShow: false
          },
          userName: null,
          isLoggedIn : function () {
              if ($scope.main.userName == "" || $scope.main.userName == null)
                $scope.main.userName = currentUser.getProfile().username;
            return currentUser.getProfile().isLoggedIn;
          },
          logOut: function () {
            localStorageService.remove("fsLogin");
            currentUser.setProfile("", "", "");
            window.location.reload();
          }
      };

      $scope.ajaxFaker = function () {
          $scope.data = [];
          var url = 'http://www.filltext.com/?rows=10&fname={firstName}&lname={lastName}&delay=5&callback=JSON_CALLBACK';

          $http.jsonp(url).success(function (data) {
              $scope.data = data;
              angular.element('.tile.refreshing').removeClass('refreshing');
          });
      };

      $scope.changeLanguage = function (langKey) {
          $translate.use(langKey);
          $scope.currentLanguage = langKey;
      };
      $scope.currentLanguage = $translate.proposedLanguage() || $translate.use();
      //
      init();

      function init() {
          var storedProfile = localStorageService.get("fsLogin");
          if (storedProfile != null) {
              //Check last logged in
              if (lastLoggedIn(storedProfile) > 7200) { //Greater than two hours, force reauthentication
                  localStorageService.remove("fsLogin");
              }
              else {
                  currentUser.setProfile(storedProfile.username, storedProfile.token, storedProfile.id, storedProfile.loginDate);
                  $scope.main.userName = storedProfile.username;
              }
          }
      }

      function lastLoggedIn(profile) {          
          var currentDateTime = moment().format('YYYY-MM-DD hh:mm:ss.SSS');
          var loginDiff = moment.duration(moment(currentDateTime).diff(moment(profile.loginDate))) / 1000; //In Seconds
          return loginDiff;
      }

  }]);
