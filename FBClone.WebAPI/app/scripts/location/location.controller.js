'use strict';

angular.module('fbCloneApp')

    .controller('LocationsCtrl', ["$scope", "$state", "$filter", "$modal", "locationResource", "locationResourceCheck", "menuResource", "stateProvinceResource", "currentUser", "toastr", "toastrConfig", "NgTableParams", "applicationSettingService",
        function ($scope, $state, $filter, $modal, locationResource, locationResourceCheck, menuResource, stateProvinceResource, currentUser, toastr, toastrConfig, NgTableParams, applicationSettingService) {
            $scope.page = {
                title: 'Manage Store Locations',
                subtitle: 'Set Up Multiple Stores',
                states: []
            };
            $scope.addressSearchVar = '';
            var self = this;
            var isFirstTime = false;
            var originalData = [];
            init();
        
            function init() {
                retrieveStateList();
                if (applicationSettingService.locations == null) {
                    //Get Application Setting
                    locationResource.query(
                        function (data) {
                            applicationSettingService.locations = data;
                        },
                        function (response) {
                            if (response.data && response.data.exceptionMessage) {
                                console.log("Error: " + response.data.exceptionMessage);
                            }
                        }
                    ).$promise.then(function () {
                        if (applicationSettingService.locations != null && applicationSettingService.locations.length == 0) {
                            openFirstTimeMessage();
                            isFirstTime = true;
                        } 
                    });
                }
                else {
                    if (applicationSettingService.locations != null && applicationSettingService.locations.length == 0) {
                        openFirstTimeMessage();
                        isFirstTime = true;
                    } 
                }
            }

            var newLocationTemplate = { id: null, name: "", address1: "", locality: "", region: "", postalCode: "", userid: currentUser.getProfile().id  };
                self.tableParams = new NgTableParams({
                    page: 1,            // show first page
                    count: 10,          // count per page
                    filter: {
                        //name: 'M'       // initial filter
                    },
                    sorting: {
                        name: 'asc'     // initial sorting
                    },            
                },
                {
                    getData: function ($defer, params) {
                        locationResource.query(
                            function (data) {
                                originalData = angular.copy(data);
                                var filteredData = params.filter() ?
                                    $filter('filter')(data, params.filter()) :
                                    data;
                                var orderedData = params.sorting() ?
                                    $filter('orderBy')(filteredData, params.orderBy()) :
                                    data;
                                params.total(orderedData.length); // set total for recalc pagination
                                $defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                            },
                            function (response) {
                                if (response.data && response.data.exceptionMessage) {
                                    console.log("Error: " + response.data.exceptionMessage);
                                }
                            }
                    );
                }
            });
            self.add = add;
            self.locationAdd = locationAdd;
            self.cancel = cancel;
            self.del = del;
            self.save = save;
            $scope.showHelp = function () {
                if (self.tableParams.data !== null && self.tableParams.data.length > 0)
                    return _.last(self.tableParams.data).isEditing;
                else
                    return false;
            }
    
        //////////
        function retrieveStateList() {
            stateProvinceResource.query(function (data) {
                $scope.page.states = data;
            },
            function (response) {
                if (response.data && response.data.exceptionMessage) {
                    console.log("Error: " + response.data.exceptionMessage);
                }
            });
        }

        function locationAdd() {
            var googlePlaceAddress = $scope.addressSearchVar;
            var addrParts = googlePlaceAddress.adr_address.split(/>,|> /);
            var newLocation = angular.copy(newLocationTemplate);
            //Check if exists and/or if already claimed
            locationResourceCheck.query({ placeId: googlePlaceAddress.place_id }).$promise.then(function (result) {
                if (result != undefined && result.claimed == "true") {
                    toastrConfig.timeOut = 7500;
                    toastrConfig.positionClass = "toast-top-full-width";
                    toastr["warning"]("This location has already been claimed. Please contact support to claim this business.", "Location Unavailable", {
                        iconClass: 'bg-orange',
                        iconType: 'fa-warning'
                    });
                }
                else {
                    newLocation.name = googlePlaceAddress.name;
                    _.forEach(addrParts, function (obj, key) {
                        var jqObj = $(obj + ">");
                        switch (jqObj.attr("class")) {
                            case "street-address":
                                newLocation.address1 = jqObj.html();
                                break;
                            case "locality":
                                newLocation.locality = jqObj.html();
                                break;
                            case "region":
                                var stateCode = jqObj.html();
                                if ($scope.page.states.length > 0) {
                                    newLocation.region = _.find($scope.page.states, { "abbreviation": stateCode }).name;
                                } else {
                                    //Error can't match state. User selects themselves
                                }
                                break;
                            case "postal-code":
                                newLocation.postalCode = jqObj.html();
                                break;
                            case "country-name":
                                newLocation.country = jqObj.html();
                                break;
                        }
                    });

                    //Set Hidden Fields
                    newLocation.address = googlePlaceAddress.formatted_address;
                    newLocation.placeId = googlePlaceAddress.place_id;
                    newLocation.geoLat = googlePlaceAddress.geometry.location.lat();
                    newLocation.geoLng = googlePlaceAddress.geometry.location.lng();
                    self.tableParams.data.splice(0, 0, newLocation);
                    newLocation.isEditing = true;
                    self.tableTracker.setCellDirty(newLocation, "name", true);
                }
            });
        }

        function openFirstTimeMessage() {
            var options = null;//angular.element(event.target.parentElement).data('options');

            var modalInstance = $modal.open({
                templateUrl: 'firstTimeLocationEntry.html',
                controller: 'ModalInstanceCtrl',
                size: 'lg',
                backdropClass: 'splash splash-2 splash-ef-14',
                windowClass: 'splash splash-2 splash-ef-14',
                resolve: {
                    items: function () {
                        return [];
                    }
                }
            });
        }

        function add () {
            //self.tableParams.data.push(newLocationTemplate);
            self.tableParams.data.splice(0, 0, newLocationTemplate);
            newLocationTemplate.isEditing = true;
        }

        function cancel(row, rowForm) {
            if (row.id) {
                var originalRow = resetRow(row, rowForm);
                angular.extend(row, originalRow);
            }
            else {
                resetRow(row, rowForm);
                _.remove(self.tableParams.data, row);
            }
        }

        function del(row) {
            //Try to Delete From Database
            row.$delete({ id: row.id }, function (data) {
                console.log("SUCCESS: Location " + row.name + " deleted successfully.");
                //If successful, remove from view
                _.remove(self.tableParams.data, function (item) {
                    return row === item;
                });
                self.tableParams.reload().then(function (data) {
                    if (data.length === 0 && self.tableParams.total() > 0) {
                        self.tableParams.page(self.tableParams.page() - 1);
                        self.tableParams.reload();
                    }
                });
                menuResource.menuHeaders = []; //Reset Menus
            });
        }

        function resetRow(row, rowForm) {
            row.isEditing = false;
            rowForm.$setPristine();
            self.tableTracker.untrack(row);
            return _.findWhere(originalData, { id: row.id });
        }

        function save(row, rowForm) {
            if (row.id) {
                var originalRow = resetRow(row, rowForm);
                angular.extend(originalRow, row);
                row.$update({ id: row.id },
                    function (data) {
                        console.log("SUCCESS: Location " + row.name + " saved successfully.");
                        locationSuccessMessage();
                    },
                    function (response) {
                        if (response.data.modelstate) {
                            var message = "";
                            for (var key in response.data.modelState) {
                                message += response.data.modelState[key] + "\r\n";
                            }
                            toastr["error"]("There was a problem claiming this location. Please try again or contact support for assistance.", "Add Location Failed", {
                                iconClass: 'bg-lightred',
                                iconType: 'fa-warning'
                            });
                        }
                        if (response.data && response.data.exceptionMessage) {
                            console.log("Error: " + response.data.exceptionMessage);
                        }
                    }
                )
            }
            else {
                resetRow(row, rowForm);
                var newLocation = new locationResource(row);
                newLocation.$save(function (data) {
                    console.log("SUCCESS: Location " + row.name + " created successfully.");
                    locationSuccessMessage();
                    //Reload Grid
                    self.tableParams.reload().then(function (data) {
                        if (data.length === 0 && self.tableParams.total() > 0) {
                            self.tableParams.page(self.tableParams.page() - 1);
                            self.tableParams.reload();
                        }
                    });
                },
                    function (response) {
                        if (response.data.modelstate) {
                            var message = "";
                            for (var key in response.data.modelState) {
                                message += response.data.modelState[key] + "\r\n";
                            }
                            console.log("Error: " + message);
                        }
                        if (response.data && response.data.exceptionMessage) {
                            console.log("Error: " + response.data.exceptionMessage);
                        }
                    }
                )
            }
        }

        function locationSuccessMessage() {
            if (isFirstTime == true) {
                toastrConfig.timeOut = 7500;
                toastrConfig.positionClass = "toast-top-full-width";
                toastrConfig.onHidden = function () {
                    $state.go("app.dashboard");
                }
                toastr["success"]("Location added successfully! Come back here if you want to add more.", "Step 5 Success", {
                    iconClass: 'bg-greensea',
                    iconType: 'fa-check'
                });
            }
            else {
                toastr["success"]("Yay! Your location was claimed successfully!", "Add Location", {
                    iconClass: 'bg-greensea',
                    iconType: 'fa-check'
                });
            }
        }
    
}]);