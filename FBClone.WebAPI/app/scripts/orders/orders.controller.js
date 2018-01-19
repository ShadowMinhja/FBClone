'use strict';

angular.module('fbCloneApp')

  .controller('OrdersCtrl', ["$rootScope", "$scope", "$q", "$timeout", "orderResource", "menuResource",
      "menuQrCodeResource", "currentUser", "toastr", "toastrConfig", "locationResource",
      "helpers", "receiveOrderService", 
    function ($rootScope, $scope, $q, $timeout, orderResource, menuResource,
        menuQrCodeResource, currentUser, toastr, toastrConfig, locationResource,
        helpers, receiveOrderService) {
        $scope.page = {
            title: 'Orders',
            subtitle: 'View Orders for The Store',
        };
        $scope.locations = [];
        $scope.orders = [];
        $scope.currentLocation = {};
        $scope.setLocation = setLocation;
        $scope.completeOrder = completeOrder;
        $scope.text = "";
        $scope.isCooking = false;
        $scope.chefStep = '';
        $scope.barValue = 0;
        $scope.barColor = "greensea";

        receiveOrderService.initialize();

        $scope.greetAll = function () {
            receiveOrderService.sendRequest();
        }

        $rootScope.$on("receiveOrder", function (e, order) {
            $scope.$apply(function () {
                var index = 0;
                var countItems = order.orderDetails.length;
                //Call Recursive Function
                retrieveMenuItems(order, index, countItems);
                showChef();
            });
        });

        function showChef() {
            if ($scope.isCooking == false) {
                $scope.chefStep = "/app/images/chef1.gif";
                $scope.isCooking = true;
                cookStart();
            }
        }

        function cookStart() {
            addProgress();
            $timeout(cookMid, 7000);
        }

        function cookMid() {
            $scope.chefStep = "/app/images/chef2.gif";
            $scope.barColor = "orange";
            $timeout(cookEnd, 10000);

        }

        function cookEnd() {
            $scope.chefStep = "/app/images/chef3.gif";
            $scope.barColor = "lightred";
            $timeout(cookComplete, 7000);
        }

        function cookComplete() {
            $scope.chefStep = "/app/images/chef4.jpg";
            $timeout(resetTime, 4000);
        }
        function addProgress() {
            if ($scope.barValue < 100) {
                $scope.barValue += 1;
                $timeout(addProgress, 240);
            }
        }

        function resetTime() {
            $scope.barValue = 0;
            $scope.barColor = "greensea";
            $scope.isCooking = false;
            //Get first order
            var inCompleteOrders = _.filter($scope.orders, { status : "Waiting" });
            var firstOrder = _.head(inCompleteOrders);
            completeOrder(firstOrder);
            if (inCompleteOrders.length - 1 > 0) {
                showChef(); //Launch Again
            }
        }
        //$scope.menus = [];
        //$scope.currentMenu = {};
        //$scope.setMenu = setMenu;

        $scope.menuQrCodes = [];
        $scope.generateQrCode = generateQrCode;
        $scope.qrCodeText = null;

        init();
                
        function init() {
            resetState();

            locationResource.query(function (data) {
                if (data.length > 0) {
                    $scope.locations = data;
                    $scope.currentLocation.selected = data[0];
                    menuQrCodeResource.getMenuQrCodes($scope.currentLocation.selected.id).$promise.then(function (data) {
                        if (data.length > 0) {
                            $scope.menuQrCodes = data;
                            $scope.qrCodeText = data[0].id;
                        }
                    });
                }
            });
            orderResource.getOrders().then(function (data) {
                _.forEach(data, function (obj, key) {
                    var index = 0;
                    var countItems = obj.orderDetails.length;
                    //Call Recursive Function
                    retrieveMenuItems(obj, index, countItems);
                    var inCompleteOrders = _.filter($scope.orders, { status: "Waiting" });
                    if (inCompleteOrders.length - 1 > 0) {
                        showChef(); //Launch Again
                    }
                });
            });
            //menuResource.getMenuHeaders().then(function (data) {
            //    $scope.menus = data;
            //});
        }

        function resetState() {
            $scope.currentMenu = {};
            $scope.menuQrCodes = [];
            $scope.qrCodeText = null;
        }

        function setLocation(item, model) {
            resetState();
            menuQrCodeResource.getMenuQrCodes(item.id).$promise.then(function (data) {
                if (data.length > 0) {
                    $scope.menuQrCodes = data;
                    $scope.qrCodeText = data[0].id;
                }
            });
        }

        //function setMenu(item, model) {
        //    var menuQrCode = _.find($scope.menuQrCodes, { menuId: item.id });
        //    if (menuQrCode !== undefined)
        //        $scope.qrCodeText = menuQrCode.id;
        //}

        function generateQrCode() {
            menuQrCodeResource.createMenuQrCode($scope.currentLocation.selected.id).$promise.then(function (data) {
                var menuQrCodeId = data.id;
                $scope.qrCodeText = menuQrCodeId;
                $timeout(function () {
                    var qrLink = angular.element('.qrcode-link');
                    var qrCodeBase64 = qrLink.attr('href');
                    if (qrCodeBase64 !== undefined && qrCodeBase64 !== null) {
                        var imageBlob = helpers.dataURItoBlob(qrCodeBase64);
                        menuQrCodeResource.uploadMenuQrCode(imageBlob, $scope.qrCodeText).then(function (result) {
                            _.remove($scope.menuQrCodes, { locationId: $scope.currentLocation.selected.id });
                            $scope.menuQrCodes.push(result.data);
                            toastrConfig.positionClass = "toast-top-full-width";
                            toastr["success"](result.message, "QR Code Generated Successfully!", {
                                iconClass: 'bg-greensea',
                                iconType: 'fa-check'
                            });
                        });
                    }
                });
            });

        }

        function retrieveMenuItems(order, index, total) {
            if (index < total) {
                menuResource.getItem(order.orderDetails[index].menuItemId).then(function (result) {
                    order.orderDetails[index].menuItem = result;
                    retrieveMenuItems(order, index + 1, total);
                });
            }
            else {
                $scope.orders.push(order);
                $scope.orders = _.sortBy($scope.orders, function (obj) { return obj.orderNumber });
            }
        }

        function completeOrder(order) {
            order.status = "Complete";
            orderResource.updateOrder(order).then(function (result) {
                //Signal R
                receiveOrderService.completeOrder(result);
            });
            order.Completed = true;
            $timeout(removeCompleted, 10000);
        }

        function removeCompleted() {
            //Get first order
            var completeOrders = _.filter($scope.orders, { status: "Complete" });
            var firstOrder = _.head(completeOrders);
            _.remove($scope.orders, firstOrder);
        }
    }
  ]);