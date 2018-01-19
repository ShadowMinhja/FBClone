'use strict';

angular.module('fbCloneApp')

  .controller('StaffCtrl', ["$scope", "$state", "$filter", "staffResource", "currentUser", "NgTableParams", function ($scope, $state, $filter, staffResource, currentUser, NgTableParams) {
        $scope.page = {
            title: 'Staff Management',
            subtitle: 'Manage Your Staff and Passcodes for fbClone',

        };
        var self = this;
        var originalData = [];
        init();

        function init() {

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
                        originalData = data;
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
    
      //////////
    function add() {
        self.tableParams.data.push(newStaffMemberTemplate);
        newStaffMemberTemplate.isEditing = true;
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