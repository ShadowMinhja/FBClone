'use strict';

angular.module('fbCloneApp')

  .controller('SurveyCtrl', ["$scope", "$state", "$filter", "surveyResource", "surveyStateService", "currentUser", "NgTableParams",
    function ($scope, $state, $filter, surveyResource, surveyStateService, currentUser, NgTableParams) {
        var self = this;
        self.selectedRow = [];
        var originalData = [];
        init();

        function init() {

        }

        var newSurveyTemplate = { id: null, name: "", description: "", active: false, userid: currentUser.getProfile().id };
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
                surveyResource.query(
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


        self.row_Click = function(row) {
            if (row.isEditing == undefined || row.isEditing == false) {
                if (surveyStateService.selectedSurvey == null) {//Brand New 
                    surveyStateService.selectedSurvey = row;
                    surveyStateService.refreshQuestionGrid();
                }
                else { //Check if de-selecting
                    if (surveyStateService.selectedSurvey.id == row.id) {
                        //Already Selected, Clear Selection
                        surveyStateService.selectedSurvey = null;
                    }
                    else {
                        surveyStateService.selectedSurvey = row;
                        surveyStateService.refreshQuestionGrid();
                    }
                }
            }
        }

        function isRowSelected(surveyId) {
            if (surveyStateService.selectedSurvey == null)
                return false
            else
                return surveyStateService.selectedSurvey.id == surveyId
        }

        //////////

        function add() {
            var item = angular.copy(newSurveyTemplate);
            self.tableParams.data.push(item);
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
                console.log("SUCCESS: Survey " + row.name + " deleted successfully.");
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
                surveyStateService.selectedSurvey = null;
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
                        console.log("SUCCESS: Survey " + row.name + " saved successfully.");
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
                var newStaffMember = new surveyResource(row);
                newStaffMember.$save(function (data) {
                    console.log("SUCCESS: Survey " + row.name + " created successfully.");
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
    
        $scope.isRowSelected = isRowSelected;
}]);