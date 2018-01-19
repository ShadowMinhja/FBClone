'use strict';

/**
 * @ngdoc overview
 * @name fbCloneApp
 * @description
 * # fbCloneApp
 *
 * Main module of the application.
 */
angular
  .module('fbCloneApp', [
    'ngAnimate',
    'ngCookies',
    'ngResource',
    'ngSanitize',
    'ngTouch',
    'ngMessages',
    'picardy.fontawesome',
    'ui.bootstrap',
    'ui.router',
    'ui.utils',
    'angular-loading-bar',
    'angular-momentjs',
    'LocalStorageModule',
    //'FBAngular',
    'lazyModel',
    'toastr',
    //'angularBootstrapNavTree',
    'oc.lazyLoad',
    'ui.select',
    //'ui.tree',
    'textAngular',
    //'colorpicker.module',
    'angularFileUpload',
    'ngImgCrop',
    //'datatables',
    //'datatables.bootstrap',
    //'datatables.colreorder',
    //'datatables.colvis',
    //'datatables.tabletools',
    //'datatables.scroller',
    //'datatables.columnfilter',
    //'ui.grid',
    //'ui.grid.resizeColumns',
    //'ui.grid.edit',
    //'ui.grid.moveColumns',
    'ngTable',
    //'smart-table',
    'angular-flot',
    'angular-rickshaw',
    'easypiechart',
    //'uiGmapgoogle-maps',
    //'ui.calendar',
    'ngTagsInput',
    'pascalprecht.translate',
    'ngMaterial',
    'common.services',
    'helper.services',
    'as.sortable',
    'angularFileUpload',
    'schemaForm',
    'schemaForm-tinymce',
    'google.places'
  ])
  .run(['$rootScope', '$state', '$stateParams', 'currentUser', function ($rootScope, $state, $stateParams, currentUser) {
    $rootScope.$state = $state;
    $rootScope.$stateParams = $stateParams;
    $rootScope.$on('$stateChangeSuccess', function(event, toState) {

      event.targetScope.$watch('$viewContentLoaded', function () {

        angular.element('html, body, #content').animate({ scrollTop: 0 }, 200);

        setTimeout(function () {
          angular.element('#wrap').css('visibility','visible');

          if (!angular.element('.dropdown').hasClass('open')) {
            angular.element('.dropdown').find('>ul').slideUp();
          }
        }, 750);
      });
      //toState.name.indexOf("app.admin") != -1
      $rootScope.containerClass = toState.containerClass;
    });
    
      //The list of states that do not need authentication (like auth)
    var authorizedStates = ['core.login', 'core.signup'];

      // This method reports if a state is one of the authorizes states
    var isChildOfAuthorizedStated = function (stateName) {
        var isChild = false;
        authorizedStates.forEach(function (state) {
            if (stateName.indexOf(state) === 0) {
                isChild = true;
            }
        });
        return isChild;
    };
    $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
        // Check if the future state is not an authorized state 
        // and if the user is not authenticated. 
        if (!isChildOfAuthorizedStated(toState.name) && !currentUser.getProfile().isLoggedIn) {
            event.preventDefault();
            //$state.go('core.login');
            window.location('/login');
        }
    });
  }])

  .config(['uiSelectConfig', function (uiSelectConfig) {
    uiSelectConfig.theme = 'bootstrap';
  }])

  .config(function (localStorageServiceProvider) {
      localStorageServiceProvider
          .setPrefix('')
          //.setPrefix('fbCloneApp')
          //.setStorageType('sessionStorage')
          .setStorageCookie(30, '/')
          .setStorageCookieDomain('') //Set empty for local host testing
      ;
  })
  //angular-language
  .config(['$translateProvider', function($translateProvider) {
    $translateProvider.useStaticFilesLoader({
      prefix: 'languages/',
      suffix: '.json'
    });
    $translateProvider.useLocalStorage();
    $translateProvider.preferredLanguage('en');
  }])

  .config(function($momentProvider){
    $momentProvider
      .asyncLoading(false)
      .scriptUrl('/assets/bower_components/angular-momentjs/angular-momentjs.js');
  })

  .value('$', $)

  .config(['$stateProvider', '$urlRouterProvider', function($stateProvider, $urlRouterProvider) {

    $urlRouterProvider.otherwise('/dashboard');

    $stateProvider

    .state('app', {
      abstract: true,
      templateUrl: 'views/tmpl/app.html'
    })
    //dashboard
    .state('app.dashboard', {
      url: '/dashboard',
      controller: 'DashboardCtrl',
      templateUrl: 'scripts/dashboard/dashboard.html'      
    })
    //admin
    .state('app.admin', {
        abstract: true,
        url: '/admin',
        template: '<div ui-view></div>'
    })
    //admin/survey
    .state('app.admin.survey', {
        url: '/survey',
        //controller: 'SurveyCtrl',
        templateUrl: 'scripts/survey/survey.html'
    })
    //admin/staff
    .state('app.admin.staff', {
        url: '/staff',
        //controller: 'StaffCtrl',
        templateUrl: 'scripts/staff/staff.html'
    })
    //admin/branding
    .state('app.admin.branding', {
        url: '/branding',
        controller: 'BrandingCtrl',
        templateUrl: 'scripts/branding/branding.html', 
        resolve: {
            plugins: ['$ocLazyLoad', function($ocLazyLoad) {
              return $ocLazyLoad.load([
                '/assets/vendor/filestyle/bootstrap-filestyle.min.js'
              ]);
            }]
        }
    })
    //admin/promotions
    .state('app.admin.promotions', {
        url: '/promotions',
        controller: 'PromotionsCtrl',
        templateUrl: 'scripts/promotions/promotions.html',
        resolve: {
            plugins: ['$ocLazyLoad', function ($ocLazyLoad) {
                return $ocLazyLoad.load([
                  '/assets/vendor/filestyle/bootstrap-filestyle.min.js'
                ]);
            }]
        }
    })
    //admin/locations
    .state('app.admin.locations', {
        url: '/locations',
        //controller: 'LocationsCtrl',
        templateUrl: 'scripts/location/locations.html'
    })
     //menu
    .state('app.menu', {
        url: '/menu',
        controller: 'MenuCtrl',
        templateUrl: 'scripts/menu/menu.html'
    })
    .state('app.orders', {
        url: '/orders',
        controller: 'OrdersCtrl',
        templateUrl: 'scripts/orders/orders.html',
        resolve: {
            plugins: ['$ocLazyLoad', function ($ocLazyLoad) {
                return $ocLazyLoad.load([
                    '/assets/bower_components/qrcode-generator/js/qrcode.js',
                    '/assets/bower_components/angular-qrcode/angular-qrcode.js'
                ])
            }]

        }
    })
    //guestcards
    .state('app.guestcards', {
        abstract: true,
        url: '/guestcards',        
        templateUrl: 'scripts/guestcards/guestcards.html'
    })
    //guestcards/sessiondata
    .state('app.guestcards.sessiondata', {
        url: '/sessiondata',
        controller: 'SessionDataCtrl',
        templateUrl: 'scripts/guestcards/sessiondata.html'
    })
        
    //app core pages (errors, login,signup)
    .state('core', {
      abstract: true,
      template: '<div ui-view></div>'
    })
    //login
    .state('core.login', {
      url: '/login',
      controller: 'LoginCtrl',
      templateUrl: 'scripts/login/login.html'
    })
    //signup
    .state('core.signup', {
      url: '/signup',
      templateUrl: 'scripts/login/signup.html'
    })
    //forgot password
    .state('core.forgotpass', {
      url: '/forgotpass',
      controller: 'ForgotPasswordCtrl',
      templateUrl: 'scripts/login/forgotpass.html'
    })
    //page 404
    .state('core.page404', {
      url: '/page404',
      templateUrl: 'views/tmpl/pages/page404.html'
    })
    //page 500
    .state('core.page500', {
      url: '/page500',
      templateUrl: 'views/tmpl/pages/page500.html'
    })
    //page offline
    .state('core.page-offline', {
      url: '/page-offline',
      templateUrl: 'views/tmpl/pages/page-offline.html'
    })
    //locked screen
    .state('core.locked', {
      url: '/locked',
      templateUrl: 'views/tmpl/pages/locked.html'
    })
    
    //documentation
    .state('app.help', {
      url: '/help',
      controller: 'HelpCtrl',
      templateUrl: 'views/tmpl/help.html'
    });
  }]);

