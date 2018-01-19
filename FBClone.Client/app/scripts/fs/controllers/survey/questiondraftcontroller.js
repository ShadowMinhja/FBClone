'use strict';

angular.module('fbCloneApp')
  .controller('QuestionDraftCtrl', ["$scope", "$state", "$modal", "$filter", "helpers",  "categoryResource", "questionDraftResource", "answerDraftResource", "surveyStateService", "currentUser", "NgTableParams",
      function ($scope, $state, $modal, $filter, helpers, categoryResource, questionDraftResource, answerDraftResource, surveyStateService, currentUser, NgTableParams) {
        var self = this;
        self.categories = [];
        var originalData = [];
        init();

        function init() {
            categoryResource.query(function (data) {
                self.categories = _.map(data, function (obj, key) {
                    return {
                        id: obj.id,
                        name: obj.name
                    }
                });
            });
        }

        var newQuestionDraftTemplate = {
            id: null, surveyId: null, categoryId: null, questionType: "", isOptional: false, questionText: "", questionFactor: 1, adminOnly: false, sequence: null,
                //answers: [{ answerid: 1, answertext: "A" }, { answerid: 2, answertext: "B" }, { answerid: 3, answertext: "C" }, { answerid: 4, answertext: "D" }],
                selectedAnswers: [],
                availableAnswerChoices: [], //Store unwritten answers generated in modal
                userId: currentUser.getProfile().id
        };
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
                var surveyId = "";
                if(surveyStateService.selectedSurvey != null)
                    surveyId = surveyStateService.selectedSurvey.id;
                questionDraftResource.query({ surveyid: surveyId },
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

        surveyStateService.questionGrid = self.tableParams;

        function isSurveySelected() {
            if (surveyStateService.selectedSurvey != null)
                //self.tableParams.reload();
            return surveyStateService.selectedSurvey != null
        }
    
        //////////
        function add() {
            var item = angular.copy(newQuestionDraftTemplate);
            item.surveyid = surveyStateService.selectedSurvey.id;
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
                console.log("SUCCESS: Question Draft " + row.name + " deleted successfully.");
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
                row.categoryId = row.category.id;
                var originalRow = resetRow(row, rowForm);
                angular.extend(originalRow, row);
                row.$update({ id: row.id },
                    function (data) {
                        console.log("SUCCESS: Question Draft  " + row.name + " updated successfully.");
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
            else { //Create New Item
                resetRow(row, rowForm);
                //Give row temp Guid to resolve ModelState Error
                row.id = helpers.createGuid();
                row.categoryId = row.category.id;
                var newQuestionDraft = new questionDraftResource(row);
                newQuestionDraft.$save(function (data) {
                    console.log("SUCCESS: Question Draft  " + row.name + " created successfully.");
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
        
        // function to submit the form after all validation has occurred
        $scope.updateAnswerChoices = function (isValid) {
            console.log('validate form');
            debugger;

            // check to make sure the form is completely valid
            if (isValid) {
                console.log('our form is amazing');
            } else {
                console.log('form is invalid');
            }
        };

        $scope.openAnswerChoiceEditor = function (event, row) {

            var options = angular.element(event.target.parentElement).data('options');

            var modalInstance = $modal.open({
                templateUrl: 'answerChoiceEditor.html',
                controller: 'AnswerDraftCtrl',
                size: 'lg',
                backdropClass: 'splash' + ' ' + options,
                windowClass: 'splash' + ' ' + options,
                resolve: {
                    selectedAnswers: function () {
                        var selectedAnswers = angular.copy(row.selectedAnswers);
                        return selectedAnswers;
                    },
                    availableAnswerChoices: function () {
                        var availableAnswerChoices = angular.copy(row.availableAnswerChoices);
                        return availableAnswerChoices;
                    },
                    questionId: function () {
                        return row.id;
                    }
                }
            });

            modalInstance.result.then(function (answers) {
                var selectedItems = _.filter(answers, function (obj, key) { return obj.active == true });
                if (selectedItems.length > 0) {
                    var questionId = selectedItems[0].questionId;
                    var questionRow = _.filter(self.tableParams.data, function (obj, key) { return obj.id == questionId });
                    questionRow[0].selectedAnswers = selectedItems;
                    questionRow[0].availableAnswerChoices = answers;
                }
            }, function () {
                //console.log(val);
            });
        };

        $scope.isSurveySelected = isSurveySelected;
}]);