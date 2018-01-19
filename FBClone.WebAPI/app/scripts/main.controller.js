'use strict';

/**
 * @ngdoc function
 * @name fbCloneApp.controller:MainCtrl
 * @description
 * # MainCtrl
 * Controller of the fbCloneApp
 */
angular.module('fbCloneApp')
  .controller('MainCtrl', ["$scope", "$state", "$http", "$translate", "localStorageService", "currentUser", "userAccount",
    function ($scope, $state, $http, $translate, localStorageService, currentUser, userAccount) {

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
                $scope.main.userName = currentUser.getProfile().userName;
            return currentUser.getProfile().isLoggedIn;
          },
          logOut: function () {
            localStorageService.remove("fsLogin");
            currentUser.setProfile("", "", "");
            userAccount.accountActions.logOutUser().$promise.then(
                function () {
                    localStorageService.remove("fsLogin");
                    window.location = "/restauranteurs";
                }, function (error) {
                    localStorageService.remove("fsLogin");
                    window.location = "/restauranteurs";
                });
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
              //Set Local Storage Info
              currentUser.setProfile(storedProfile.userName, storedProfile.access_token, storedProfile.id, storedProfile.loginDate);

              //Check last logged in
              if (lastLoggedIn(storedProfile) > 86400) { //Greater than 24 hours, force reauthentication           
                userAccount.accountActions.logOutUser().$promise.then(
                    function(){
                        localStorageService.remove("fsLogin");
                        window.location = "/restauranteurs";
                    }, function (error) {
                        localStorageService.remove("fsLogin");
                        window.location = "/restauranteurs";
                    });
              }
              else {
                  $scope.main.userName = storedProfile.userName;
              }              
          }
      }

      function lastLoggedIn(profile) {          
          var currentDateTime = moment().format('YYYY-MM-DD HH:mm:ss.SSS');
          var loginDiff = moment.duration(moment(currentDateTime).diff(moment(profile.loginDate))) / 1000; //In Seconds
          return loginDiff;
      }

  }]);
