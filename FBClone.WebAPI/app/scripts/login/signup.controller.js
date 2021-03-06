'use strict';

angular.module('fbCloneApp')

  .controller('SignupCtrl', ['$scope', '$state', 'userAccount', 'currentUser', function ($scope, $state, userAccount, currentUser) {
      var vm = this;
      
      vm.userData = {
          //firstName: '',
          //lastName: '',
          organizationName: '',
          //email: '',
          //password: '',
          //confirmPassword: '',
          subscriptionPlan: 'free',
          accept: ''
      };

      vm.registerBusinessUser = function () {
          //vm.userData.confirmPassword = vm.userData.password;

          userAccount.registration.registerBusinessUser(vm.userData,
              function (data) {
                  //vm.confirmPassword = "";
                  vm.message = "Registration successful! Logging in...";
                  //vm.login();
                  $state.go("app.dashboard");
              },
              function (response) {
                  vm.isLoggedIn = false;
                  vm.message = response.statusText + "\r\n";
                  if (response.data.exceptionMessage)
                      vm.message += response.data.exceptionMessage;

                  // Validation errors
                  if (response.data.modelState) {
                      for (var key in response.data.modelState) {
                          vm.message += response.data.modelState[key] + "\r\n";
                      }
                  }
              });
      }

      //vm.login = function () {
      //    vm.userData.grant_type = "password";
      //    vm.userData.userName = vm.userData.email;

      //    userAccount.login.loginUser(vm.userData,
      //        function (data) {
      //            vm.message = "Login successful!";
      //            vm.password = "";
      //            currentUser.setProfile(vm.userData.userName, data.access_token, data.id, null);
      //            $state.go("app.dashboard");
      //        },
      //        function (response) {
      //            vm.password = "";
      //            vm.message = response.statusText + "\r\n";
      //            if (response.data.exceptionMessage)
      //                vm.message += response.data.exceptionMessage;

      //            if (response.data.error) {
      //                vm.message += response.data.error;
      //            }
      //        });
      //}

      vm.isFree = function () {
          return vm.userData.subscriptionPlan == 'free' ? true : false;
      }

      vm.isPremium = function () {
          return vm.userData.subscriptionPlan == 'premium_monthly' ? true : false;
      }

      vm.isProfessional = function () {
          return vm.userData.subscriptionPlan == 'professional_monthly' ? true : false;
      }
      
  }]);