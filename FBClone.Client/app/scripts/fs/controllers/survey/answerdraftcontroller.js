'use strict';

angular.module('fbCloneApp')
    .controller('AnswerDraftCtrl', ["$scope", "$compile", "$q", "$modalInstance", "$filter", "answerDraftResource", "currentUser", "selectedAnswers", "availableAnswerChoices", "questionId", "NgTableParams", "ngTableEventsChannel",
        function ($scope, $compile, $q, $modalInstance, $filter, answerDraftResource, currentUser, selectedAnswers, availableAnswerChoices, questionId, NgTableParams, ngTableEventsChannel) {
            var self = this;
            $scope.endPosition = -1;
            var endPositionBuffer = 10;
            var newAnswerChoiceTemplate = { answerId: null, active: false, questionId: questionId, answerText: null, answerFactor: null, sequence: null, userid: currentUser.getProfile().id };
            $scope.selectedAnswers = selectedAnswers;
            //$scope.availableChoices = [
            //    { answerid: 1, questionid: questionId, answertext: "A", answerfactor: 1, sequence: 1, userid: currentUser.getProfile().id }, 
            //    { answerid: 2, questionid: questionId, answertext: "B", answerfactor: 1, sequence: 2, userid: currentUser.getProfile().id },
            //    { answerid: 3, questionid: questionId, answertext: "C", answerfactor: 1, sequence: 3, userid: currentUser.getProfile().id },
            //    { answerid: 4, questionid: questionId, answertext: "D", answerfactor: 1, sequence: 4, userid: currentUser.getProfile().id }
            //];
            $scope.availableChoices = [];
            $scope.newAnswerChoice = {};
            $scope.sortableOptions = {
                accept: function (sourceItemHandleScope, destSortableScope, destItemScope) {
                    if (destSortableScope.modelValue !== undefined && destSortableScope.modelValue.length > 0) {
                        if (destItemScope !== undefined) {
                            var columns = $(destItemScope.element).children();
                            $.each(columns, function (index, value) {
                                if ($(value).attr("no-drag") !== undefined) {
                                    return false;
                                }
                            });
                        }

                        return true;
                    }
                    else
                        return false;
                    //return sourceItemHandleScope.itemScope.sortableScope.$id !== destSortableScope.$id;
                },
                //Triggered when an item is moved across columns
                //itemMoved: function (event) {
                //    event.source.itemScope.modelValue.status = event.dest.sortableScope.$parent.column.name;
                //},
                orderChanged: function (event) {
                    var srcItem = event.source.itemScope.modelValue;
                    var srcIndex = event.source.index;
                    var targetIndex = event.dest.index;

                    if (checkNoDrag(event.dest.sortableScope.element, targetIndex) == false) {
                        if (srcIndex < targetIndex) { //Drag Item Down: Set the sequence, and decrease all elements before it
                            for (var j = targetIndex; j > srcIndex; j--) {
                                $scope.tableParams.data[j - 1].sequence -= 1;
                            }
                            $scope.tableParams.data[targetIndex].sequence = targetIndex + 1;
                        }
                        else if (srcIndex > targetIndex) { //Drag Item Up: Set the sequence, and increase all elements after it
                            for (var i = targetIndex; i < srcIndex; i++) {
                                $scope.tableParams.data[i + 1].sequence += 1;
                            }
                            $scope.tableParams.data[targetIndex].sequence = targetIndex + 1;
                        }
                        else {
                            //The index is unchanged
                        }
                        _.each($scope.tableParams.data, function (obj, key) {
                            update(obj);
                        });
                    }
                    resort();
                },
                containment: '#answer-choice-container',
                containerPositioning: 'relative'
            };

            $scope.tableParams = new NgTableParams({
                page: 1,            // show first page
                count: 10,          // count per page
                filter: {
                    //name: 'M'       // initial filter
                },
                sorting: {
                    sequence: 'asc'     // initial sorting
                },
            }, {
                //data: $scope.availableChoices
                getData: function ($defer, params) {
                    if (questionId != null) {
                        answerDraftResource.query({ questionId: questionId == null ? "null" : questionId },
                            function (data) {
                                $scope.availableChoices = transformUnselected(data);
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
                    else {
                        return transformUnselected($scope.availableChoices);
                    }
                }
            });

            //Callback after table is rendered
            $scope.$on("onRepeatLast", function (scope, element, attrs) {
                //Set no-drag for all unchecked items
                _.each($scope.availableChoices, function (obj, key) {
                    if(obj.sequence == getEndPosition()){
                        var rowIndex = questionId != null ? _.findIndex($scope.tableParams.data, { id: obj.id, answerText: obj.answerText })
                            : _.findIndex($scope.tableParams.data, { answerText: obj.answerText });
                        toggleSortable(rowIndex, false);
                    }
                });
            });

            init();

            function init() {
                updateAvailableAnswerChoices(); //Added back any newly created answers that haven't had a chance to save yet
                angular.extend($scope.newAnswerChoice, newAnswerChoiceTemplate); //Resets newAnswerChoice                
                highlightSelectedAnswers();
                resort();
            }

            //Utility Functions
            function getEndPosition() {
                $scope.endPosition = $scope.availableChoices.length + endPositionBuffer;
                return $scope.endPosition;
            }
            $scope.getEndPosition = getEndPosition;

            function transformUnselected(data) {
                _.each(data, function (obj, key) {
                    if (obj.sequence == -1)
                        obj.sequence = data.length + endPositionBuffer;
                });
                getEndPosition();
                return data;
            }

            function getSequence(sequence) {
                return sequence == $scope.endPosition ? null : sequence;
            }
            $scope.getSequence = getSequence;

            function checkNoDrag(container, targetIndex) {
                var result = false;
                var row = $(container).children()[targetIndex];
                var columns = $(row).children();
                $.each(columns, function (index, value) {
                    if ($(value).attr("no-drag") !== undefined)
                        result =  true;
                });
                return result;
            }

            function subscribeToTableEvents(tableParams) {
                if (!tableParams) return;
                //ngTableEventsChannel.onAfterCreated(function () { }, $scope, tableParams);
                //ngTableEventsChannel.onDatasetChanged(function () { }, $scope, tableParams);
                //ngTableEventsChannel.onAfterReloadData(function () { }, $scope, $scope.tableParams);
            }

            function updateAvailableAnswerChoices() {
                var answersToAdd = _.filter(availableAnswerChoices, function (obj, key) { return obj.answerid == null });
                _.forEach(answersToAdd, function (obj, key) {
                    $scope.availableChoices.push(obj);
                });
            }

            function highlightSelectedAnswers() {
                _.forEach($scope.selectedAnswers, function (obj, key) {
                    var item = _.find($scope.availableChoices, { answerText: obj.answerText }); //use instead of answerid, because some newly created answers may need to be here
                    if (item !== undefined) {
                        item.active = true;
                    }
                    else {
                        if (item !== undefined)
                            item.sequence = getEndPosition();
                    }
                });
            }

            function updateSorting() {
                var selectedItems = _.filter($scope.tableParams.data, function (obj, key) { return obj.active == true });
                for (var i = 0; i < selectedItems.length; i++) {
                    selectedItems[i].sequence = i + 1;
                    update(selectedItems[i]);
                }
            }

            function resort() {
                //Special condition when only two items
                if ($scope.tableParams.data.length == 2) {
                    var firstItem = $scope.tableParams.data[0];
                    if (firstItem.sequence == getEndPosition()) {
                        //Rearrange due to incorrect sort
                        $scope.tableParams.data.splice(0, 1);
                        $scope.tableParams.data.push(firstItem);
                    }
                }
                $scope.tableParams.sorting({}); //Clear sorting
                $scope.tableParams.reload();  //Have to reload in between otherwise it won't recognize the sort command.
                $scope.tableParams.sorting("sequence", "asc");
            }

            $scope.isRowSelected = function (isSelected) {
                return isSelected === undefined ? false : isSelected;
            }

            $scope.answerCheckBoxChanged = function(row, index) {
                if (row.active == false) {
                    toggleSortable(index, false);
                    row.sequence = getEndPosition();
                    _.remove($scope.selectedAnswers, row);
                    toggleSortable(index, false);

                    updateSorting();
                    if (row.id)
                        update(row);
                    else
                        resort();
                }
                else {
                    toggleSortable(index, true);
                    $scope.selectedAnswers.push(row);
                    row.sequence = $scope.selectedAnswers.length;
                    if(row.id)
                        update(row);
                    else
                        resort();
                }
            }

            function toggleSortable(index, flag) {
                var targetRow = $("#answer-choice-container").children()[index]
                var targetColumns = $(targetRow).children();
                var $targetDragHandle;

                //Find column with as-sortable-item-handle
                if (targetColumns.length > 0) {
                    for (var i = 0; i < targetColumns.length; i++) {
                        if ($(targetColumns[i]).attr("as-sortable-item-handle") != undefined) {
                            $targetDragHandle = $(targetColumns[i]);
                            break;
                        }
                    }
                }
                if ($targetDragHandle != undefined) {
                    if (flag == true) {
                        $targetDragHandle.removeAttr("no-drag");
                    }
                    else {//False  
                        $targetDragHandle.attr("no-drag", "");
                    }
                }
            }

            //Submit form to create Answer choices
            $scope.answerDraftGrid_Create = function (answerChoiceForm) {
                //Create the new Answer Choice
                var item = {};
                angular.extend(item, $scope.newAnswerChoice);
                item.sequence = getEndPosition();
                $scope.availableChoices.push(item);
                _.each($scope.availableChoices, function (obj, key) {
                    if (obj.active == false)
                        obj.sequence = getEndPosition();
                });
                if (questionId != null) //Will have to batch save later if no questionId
                    save(item);
                $scope.tableParams.reload();
                
                //Reset the form
                answerChoiceForm.$setPristine();
                angular.extend($scope.newAnswerChoice, newAnswerChoiceTemplate); //Resets newAnswerChoice to prepare for next entry
            }

            $scope.del = function (row) {
                if (questionId != null) {
                    //Try to Delete From Database
                    row.$delete({ id: row.id }, function (data) {
                        console.log("SUCCESS: Answer Draft text: " + row.answerText + " deleted successfully.");
                        //If successful, remove from view
                        _.remove($scope.tableParams.data, function (item) {
                            return row === item;
                        });
                        $scope.tableParams.reload().then(function (data) {
                            if (data.length === 0 && $scope.tableParams.total() > 0) {
                                $scope.tableParams.page($scope.tableParams.page() - 1);
                            }
                        });
                    });
                }
                else {
                    var rowToDeleteIdx = _.findIndex($scope.availableChoices, row);
                    if (rowToDeleteIdx !== undefined) {
                        $scope.availableChoices.splice(rowToDeleteIdx, 1);
                    }
                    if (row.active == true) {
                        rowToDeleteIdx = _.findIndex($scope.selectedAnswers, row);
                        $scope.selectedAnswers.splice(rowToDeleteIdx, 1);
                    }
                }
                updateSorting();
            }

            var save = function (answerChoiceForm) {
                var newAnswer = new answerDraftResource(answerChoiceForm);
                newAnswer.$save(function (data) {
                    console.log("SUCCESS: Answer choice created successfully.");
                    //Reload Grid
                    $scope.tableParams.reload().then(function (data) {
                        if (data.length === 0 && $scope.tableParams.total() > 0) {
                            $scope.tableParams.page($scope.tableParams.page() - 1);
                            $scope.tableParams.reload();
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
            var update = function (row) {
                if (row.id) {
                    var updatedRow = angular.copy(row);
                    if (updatedRow.sequence == getEndPosition())
                        updatedRow.sequence = -1;
                    updatedRow.$update({ id: updatedRow.id },
                        function (data) {
                            console.log("SUCCESS: Answer Draft text: " + data.answerText + " updated successfully.");
                            //Reload Grid
                            $scope.tableParams.reload().then(function (data) {
                                if (data.length === 0 && self.tableParams.total() > 0) {
                                    self.tableParams.page(self.tableParams.page() - 1);
                                    resort();
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
                else {
                    //No Update made. This answer will be saved later when question is created
                }
            }
           
            //Submit form to parent Question Draft Grid
            $scope.ok = function (item, valid) {
                if (valid) {
                    var answers = angular.copy($scope.tableParams.data);
                    _.each(answers, function (obj, key) {
                        if (obj.sequence == getEndPosition())
                            obj.sequence = -1;
                    });
                    $modalInstance.close(answers);
                }
                else
                    return false;
            };

            $scope.cancel = function () {
                $modalInstance.dismiss('cancel');
            };
    }]);