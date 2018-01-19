'use strict';

angular.module('fbCloneApp')

  .controller('StaffCtrl', ["$scope", "$state", "$filter", "$modal", "staffResource", "currentUser", "NgTableParams", "applicationSettingService", "toastr", "toastrConfig",
      function ($scope, $state, $filter, $modal, staffResource, currentUser, NgTableParams, applicationSettingService, toastr, toastrConfig) {
        $scope.page = {
            title: 'Staff Management',
            subtitle: 'Manage Your Staff and Passcodes for fbClone',
        };
        var self = this;
        var isFirstTime = false;
        var originalData = [];
        init();

        function init() {
            if (applicationSettingService.settings == null) {
                //Get Application Setting
                applicationSettingService.retrieveSettings().then(function (data) {
                    if (data != null) {
                        applicationSettingService.settings = data;
                        if (applicationSettingService.settings.staffSetup == false) {
                            openFirstTimeMessage();
                            isFirstTime = true;
                        }
                    }
                });
            }
            else {
                if (applicationSettingService.settings.staffSetup == false) {
                    openFirstTimeMessage();
                    isFirstTime = true;
                }
            }
        }

        function openFirstTimeMessage() {
            var modalInstance = $modal.open({
                templateUrl: 'firstTimeStaffEntry.html',
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

        var newStaffMemberTemplate = { id: null, name: "", gender: "", passcode: "", userid: currentUser.getProfile().id };
        self.tableParams = new NgTableParams({
            page: 1,            // show first page
            count: 10,          // count per page
            filter: {
                //name: 'M'       // initial filter
            },
            sorting: {
                name: 'asc'     // initial sorting
            },            
        }, { 
            getData: function ($defer, params) {
                //OData Query
                //staffResource.query({
                //    $filter: "contains(ProductCode, 'GDN') and Price ge 5 and Price le 20",
                //    $orderby: "Price desc"
                //}, function (data) {
                        
                //});

                //Normal Query
                staffResource.query(
                    function (data) {
                        originalData = angular.copy(data);
                        // use built-in angular filter
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
        self.cancel = cancel;
        self.del = del;
        self.save = save;
        $scope.showFilter = function () {
            if (self.tableParams.data.length > 0)
                return true;
            else
                return false;
        }
        $scope.showHelp = function () {
            if (self.tableParams.data !== null && self.tableParams.data.length > 0)
                return _.last(self.tableParams.data).isEditing;
            else
                return false;
        }
    
      //////////
    function add() {
        var item = angular.copy(newStaffMemberTemplate);
        //self.tableParams.data.push(item);
        self.tableParams.data.splice(0, 0, item);
        item.isEditing = true;
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
            console.log("SUCCESS: Staff member " + row.name + " deleted successfully.");
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
            row.$update({ id: row.id},
                function (data) {
                    console.log("SUCCESS: Staff member " + row.name + " saved successfully.");
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
        else {
            resetRow(row, rowForm);
            var newStaffMember = new staffResource(row);
            newStaffMember.$save(
                function (data) {
                    console.log("SUCCESS: Staff member " + row.name + " created successfully.");
                    if (isFirstTime == true) {
                        toastrConfig.timeOut = 7500;
                        toastrConfig.positionClass = "toast-top-full-width";
                        toastrConfig.onHidden = function () {
                            $state.go("app.dashboard");
                        }
                        toastr["success"]("Staff member created successfully!  Return to this page later if you wish to create more.", "Step 1 Success", {
                            iconClass: 'bg-greensea',
                            iconType: 'fa-check'
                        });
                    }
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
   
}]);