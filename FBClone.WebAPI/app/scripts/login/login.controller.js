'use strict';
angular.module('fbCloneApp')
  .controller('LoginCtrl', ['$state', 'localStorageService', 'userAccount', 'currentUser', function ($state, localStorageService, userAccount, currentUser) {
      var vm = this;

      vm.userData = {
          userName: '',
          email: '',
          password: ''
      };

      vm.login = function () {          
          vm.userData.grant_type = "password";
          vm.userData.userName = vm.userData.email;

          userAccount.login.loginUser(vm.userData,
              function (data) {
                  vm.message = "Login successful!";
                  vm.password = "";
                  currentUser.setProfile(vm.userData.userName, data.access_token, data.id, null);
                  //Store Local Storage to Persist Login
                  localStorageService.set("fsLogin", currentUser.getProfile())
                  $state.go("app.dashboard");
              },
              function (response) {
                  vm.password = "";
                  vm.message = response.statusText + "\r\n";
                  if (response.data.exceptionMessage)
                      vm.message += response.data.exceptionMessage;

                  if (response.data.error) {
                      if (response.data.error == "invalid_grant")
                          vm.message = "Error, invalid username or bad password. Please try again.";
                      else
                          vm.message += response.data.error;
                  }
              });
      }
  }]);
