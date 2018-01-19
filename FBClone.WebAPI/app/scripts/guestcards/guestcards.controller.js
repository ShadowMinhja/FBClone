'use strict';

angular.module('fbCloneApp')

  .controller('SessionDataCtrl', ["$scope", "$state", "$filter", "$moment", "helpers", "surveyResource", "questionresponsesetResource", "currentUser", "NgTableParams", "applicationSettingService", "staffResource", "toastr", "toastrConfig",
        function ($scope, $state, $filter, $moment, helpers, surveyResource, questionresponsesetResource, currentUser, NgTableParams, applicationSettingService, staffResource, toastr, toastrConfig) {
        var self = this;
        var params = { $skip: 0 };

        $scope.page = {
            title: 'Session Data',
            subtitle: 'Responses by Guests to Your Surveys',
            questionResponseSets: [],
            view: "Master",
            selectedStaff: null,
            itemsPerPage: 15
        };

        $scope.totalItems = null;
        $scope.currentPage = 1;
        $scope.staffMembers = [];
        //Date filter
        $scope.dateFilter = { 
            startDate: $moment().subtract(31, 'days').format('MMMM D, YYYY'),
            endDate: $moment().add(0, 'days').format('MMMM D, YYYY'),
            rangeOptions : {
                ranges: {
                    Today: [$moment(), $moment()],
                    Yesterday: [$moment().subtract(1, 'days'), $moment().subtract(1, 'days')],
                    'Last 7 Days': [$moment().subtract(6, 'days'), $moment()],
                    'Last 30 Days': [$moment().subtract(29, 'days'), $moment()],
                    'This Month': [$moment().startOf('month'), $moment().endOf('month')],
                    'Last Month': [$moment().subtract(1, 'month').startOf('month'), $moment().subtract(1, 'month').endOf('month')]
                },
                opens: 'left',
                startDate: $moment().subtract(29, 'days'),
                endDate: $moment(),
                parentEl: '#content'
            }
        }
        $scope.selectedSession = null;
        //Carousel
        $scope.myInterval = 5000;
        $scope.noWrapSlides = false;

        //Watchers
        $scope.$watch(
            function () { return $scope.dateFilter.startDate; },
            // This is the change listener, called when the value returned from the above function changes
            function (newValue, oldValue) {
                if (newValue !== oldValue) {
                    filterChanged();
                }
            }
        );
        init();

        function init() {            
            //Get Question Response Sets
            filterChanged();
            staffResource.query(function (data) {
                $scope.staffMembers = data;
            });
        }
        
        $scope.pageChanged = function (page) {
            params.$skip = (page - 1) * $scope.page.itemsPerPage;
            getQuestionResponseSets(false);
        };

        $scope.sessionSummary = function () {
            $scope.page.view = "Master";
            $scope.selectedSession = null;
        }

        $scope.sessionDetails = function (response) {
            $scope.page.view = "Detail";
            response.questionResponses = formatData(response.questionResponses);
            $scope.selectedSession = response;
        }

        $scope.showMaster = function() {
            return $scope.page.view == "Master";
        }

        $scope.showDetail = function () {
            return $scope.page.view == "Detail";
        }
        
        //Staff Filter
        $scope.changeStaffMember = function () {
            filterChanged();
        }
        
        function filterChanged() {
            params.$filter = undefined
            var selectedStaff = $scope.page.selectedStaff;
            if (selectedStaff != null) {
                params.$filter = "StaffMemberId eq '" + selectedStaff + "'";
                $scope.currentPage = 1;
                params.$skip = ($scope.currentPage - 1) * $scope.page.itemsPerPage;
            }
            var dateFilter = $scope.dateFilter;
            if (dateFilter.startDate != null) {
                if (params.$filter != undefined)
                    params.$filter += ' and '
                else
                    params.$filter = "";
                params.$filter += helpers.getDateFilter("CreatedAt", moment(new Date(dateFilter.startDate)), "ge");
            }
            if (dateFilter.endDate != null) {
                if (params.$filter != undefined)
                    params.$filter += ' and '
                else
                    params.$filter = "";
                params.$filter += helpers.getDateFilter("CreatedAt", moment(new Date(dateFilter.endDate)), "le");
            }
            getQuestionResponseSets(true);
        }

        function getQuestionResponseSets(refreshTotal) {
            //Run query
            questionresponsesetResource.getQuestionResponseSets(params).then(function (data) {
                if (data != null) {
                    if (refreshTotal == true) {
                        $scope.totalItems = data.count;
                    }
                    $scope.page.questionResponseSets = formatData(data.items);
                }
            });
        }

        function formatData(data) {
            data = _.each(data, function (obj, key) {
                if(obj.totalScore != null)
                    obj.totalScore = helpers.formatPercent(obj.totalScore);
                if(obj.createdAt != null)
                    obj.createdAt = moment(obj.createdAt).format("MMMM Do YYYY, h:mm:ss a");
                if(obj.questionScore != null)
                    obj.questionScore = helpers.formatPercent(obj.questionScore);
            });
            return data;
        }

}]);