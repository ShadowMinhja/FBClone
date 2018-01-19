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
                sequence: 'asc'     // initial sorting
            },            
        }, {
            getData: function ($defer, params) {
                var surveyId = "";
                if (surveyStateService.selectedSurvey != null) {
                    //surveyId = surveyStateService.selectedSurvey.id;
                    var data = surveyStateService.selectedSurvey.questionDrafts;
                    _.forEach(data, function(obj, key) {
                        obj.selectedAnswers = _.filter(obj.answerDrafts, { "active" : true });
                        obj.availableAnswers = obj.answerDrafts;
                    });
                    originalData = data;
                    var filteredData = params.filter() ?
                        $filter('filter')(data, params.filter()) :
                        data;
                    var orderedData = params.sorting() ?
                        $filter('orderBy')(filteredData, params.orderBy()) :
                        data;
                    params.total(orderedData.length); // set total for recalc pagination
                    $defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                }
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
        surveyStateService.questionGrid = self.tableParams;

        function isSurveySelected() {
            if (surveyStateService.selectedSurvey != null)
            return surveyStateService.selectedSurvey != null
        }
    
        //////////
        function add() {
            var item = angular.copy(newQuestionDraftTemplate);
            item.surveyid = surveyStateService.selectedSurvey.id;
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
            questionDraftResource.delete({ id: row.id }, row).$promise.then(
                function (data) {
                    console.log("SUCCESS: Question Draft deleted successfully.");
                    //If successful, remove from view
                    _.remove(self.tableParams.data, function (item) {
                        return row === item;
                    });
                    _.remove(surveyStateService.selectedSurvey.questionDrafts, { id : row.id });

                }
            );
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
                questionDraftResource.update({ id: row.id }, row).$promise.then(
                    function (data) {
                        console.log("SUCCESS: Question Draft  " + row.name + " updated successfully.");                    
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
                    });
            }
            else { //Create New Item
                resetRow(row, rowForm);
                //Give row temp Guid to resolve ModelState Error
                row.id = helpers.createGuid();
                row.categoryId = row.category.id;
                row.answerDrafts = row.availableAnswerChoices;
                questionDraftResource.save(row).$promise.then(
                    function (data) {
                        surveyStateService.selectedSurvey.questionDrafts.push(data);
                        var questionRow = _.filter(self.tableParams.data, { id: row.id });
                        questionRow.id = data.id;
                        console.log("SUCCESS: Question Draft  created successfully.");                    
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
                templateUrl: '/app/scripts/survey/partials/answerChoiceEditor.html',
                controller: 'AnswerDraftCtrl',
                size: 'lg',
                backdropClass: 'splash splash-2 splash-ef-14',
                windowClass: 'splash splash-2 splash-ef-14',
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