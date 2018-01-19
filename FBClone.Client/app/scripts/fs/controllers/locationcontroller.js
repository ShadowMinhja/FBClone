'use strict';

angular.module('fbCloneApp')

  .controller('LocationsCtrl', ["$scope", "$state", "$filter", "locationResource", "currentUser", "NgTableParams", function ($scope, $state, $filter, locationResource, currentUser, NgTableParams) {
        $scope.page = {
            title: 'Manage Store Locations',
            subtitle: 'Set Up Multiple Stores',
        };
        var self = this;
        var originalData = [];
        init();
        
        function init() {
           
        }

        var newLocationTemplate = { id: null, name: "", street: "", city: "", state: "", zipcode: "", userid: currentUser.getProfile().id  };
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
                locationResource.query(
                    function (data) {
                        originalData = data;
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
                    });
            }
        });
        self.add = add;
        self.cancel = cancel;
        self.del = del;
        self.save = save;
    
      //////////
    function add () {
        self.tableParams.data.push(newLocationTemplate);
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
            var newStaffMember = new locationResource(row);
            newStaffMember.$save(function (data) {
                console.log("SUCCESS: Location " + row.name + " created successfully.");
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