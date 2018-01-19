'use strict';

/**
 * @ngdoc function
 * @name fbCloneApp.controller:DashboardCtrl
 * @description
 * # DashboardCtrl
 * Controller of the fbCloneApp
 */
angular.module('fbCloneApp')
  .controller('DashboardCtrl', ['$scope', '$state', '$http', '$moment', 'helpers', 'applicationSettingService', 'locationResource', 'dashboardResource',
      function ($scope, $state, $http, $moment, helpers, applicationSettingService, locationResource, dashboardResource) {
        $scope.page = {
            title: 'Dashboard',
            //subtitle: 'Place subtitle here...',
            showFirstTime: false,
            settings: null,
            customerResponses: 'N/A',
            averageScore: 'N/A',
            averageDuration: 'N/A',
            positivityCounts: {
                countGood: 'N/A',
                countBad: 'N/A',
                percentGood: 'N/A',
                percentBad: 'N/A'
            },
        };

        $scope.steps = {
            step1: false,
            step2: false,
            step3: false,
            step4: false,
            step5: false
        }

        //Charts + Data
        $scope.staffScores = [];
        $scope.easypiechart = {
            percent: 0,
            options: {
                animate: {
                    duration: 3000,
                    enabled: true
                },
                barColor: '#16a085',
                scaleColor: false,
                lineCap: 'round',
                size: 140,
                lineWidth: 4
            }
        };
        $scope.easypiechart2 = {
            percent: 0,
            options: {
                animate: {
                    duration: 3000,
                    enabled: true
                },
                barColor: '#e05d6f',
                scaleColor: false,
                lineCap: 'round',
                size: 140,
                lineWidth: 4
            }
        };
        $scope.responses = [];
        //Sentiment Graph
        $scope.sentimentDataset = [{
            data: [], 
            label: 'Avg. Daily Score',
            bars: {
                show: true,
                barWidth: 0.6,
                lineWidth: 0,
                fillColor: { colors: [{ opacity: 0.8 }, { opacity: 0.8}] }
            }
        },{
            data: [],
            label: 'Bad Reviews',
            points: {
            show: true,
            radius: 4
            },
            splines: {
            show: true,
            tension: 0.45,
            lineWidth: 4,
            fill: 0
            }
        },{
            data: [],
            label: 'Good Reviews',
            points: {
                show: true,
                radius: 4
            },
            splines: {
                show: true,
                tension: 0.45,
                lineWidth: 4,
                fill: 0
            }
        }];
        $scope.sentimentOptions = {
            colors: ['#61c8b8', '#e05d6f', '#a2d200'],
            series: {
                shadowSize: 0
            },
            legend: {
                backgroundOpacity: 0,
                margin: -7,
                position: 'ne',
                noColumns: 2
            },
            xaxis: {
            tickLength: 0,
            font: {
                color: '#fff'
            },
            position: 'bottom',
            ticks: []
            },
            yaxis: {
                tickLength: 0,
                font: {
                    color: '#fff'
                }
            },
            grid: {
                borderWidth: {
                    top: 0,
                    right: 0,
                    bottom: 1,
                    left: 1
                },
                borderColor: 'rgba(255,255,255,.3)',
                margin:0,
                minBorderMargin:0,
                labelMargin:20,
                hoverable: true,
                clickable: true,
                mouseActiveRadius:6
            },
            tooltip: true,
            tooltipOpts: {
                content: '%s: %y',
                defaultTheme: false,
                shifts: {
                    x: 0,
                    y: 20
                }
            }
        };
        //Donut Data
        $scope.donutData = [{ label: 'No data', value: 0, color: '#00a3d8' }];
        $scope.oneAtATime = true;
        $scope.status = {
            isFirstOpen: true,
            tab1: {
                open: true
            },
            tab2: {
                open: false
            },
            tab3: {
                open: false
            }
        };

        //Color Dictionary
        var colorDictionary = ['#00a3d8', '#2fbbe8', '#72cae7', '#d9544f', '#ffc100', '#1693A5'];

        var params = { $skip: 0 };
        //Date filter
        $scope.dateFilter = {
            startDate: $moment().subtract(31, 'days').format('MMMM D, YYYY'),
            endDate: $moment().add(0, 'days').format('MMMM D, YYYY'),
            rangeOptions: {
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
        //Watchers
        $scope.$watch(
            function () { return $scope.dateFilter.startDate; },
            // This is the change listener, called when the value returned from the above function changes
            function (newValue, oldValue) {
                if (newValue !== oldValue) {
                    dateFilterChanged();
                }
            }
        );
        init();

        function init() {
            //Get Locations
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
                //Get Application Setting
                applicationSettingService.retrieveSettings().then(function (data) {
                    if (data != null && data.staffSetup !== null) {
                        applicationSettingService.settings = data;
                        checkApplicationSettings();
                        dateFilterChanged();
                    } else {
                        $state.go('core.signup');
                    }
                });
            });
            
        }

        function checkApplicationSettings() {
            var settings = applicationSettingService.settings;
            $scope.page.settings = settings;
            $scope.page.isLocationSetup = true;
            if (applicationSettingService.locations.length == 0) {
                $scope.page.isLocationSetup = false;
            }
            
            if (settings.staffSetup == false || settings.surveySetup == false || settings.brandingSetup == false || settings.promotionSetup == false || $scope.page.isLocationSetup == false) {
                $scope.page.showFirstTime = true;
                if (settings.promotionSetup == true) {
                    $scope.steps.step5 = true;
                }
                if (settings.brandingSetup == true) {
                    $scope.steps.step4 = true;
                }
                else if (settings.surveySetup == true) {
                    $scope.steps.step3 = true;
                }
                else if (settings.staffSetup == true) {
                    $scope.steps.step2 = true;
                }
                else if (settings.staffSetup == false) {
                    $scope.steps.step1 = true;
                }
                else {
                    //None
                }
            }
        }

        function getDashboardStats() {
            dashboardResource.getStatusTileCount(params).then(function (data) {
                $scope.page.customerResponses = data.customerResponses;
            });
            dashboardResource.getStatusTileAvg(params).then(function (data) {
                if (data.length > 0) {
                    data = _.sortBy(data, function (obj) { return obj.createdAt });
                    var nonNullData = _.filter(data, function (obj, key) { return obj.averageScore != null && obj.averageDuration != null && obj.totalSubscriptions != null });
                    if (nonNullData.length > 0) {
                        $scope.page.averageScore = helpers.formatPercent(_.sum(nonNullData, function (obj, key) { return obj.averageScore }) / nonNullData.length) + "%";
                        $scope.page.averageDuration = helpers.formatNumber(_.sum(nonNullData, function (obj, key) { return obj.averageDuration}) / nonNullData.length) + " seconds";
                    }
                    var xAxisDateIndex = 1;
                    //Reset data
                    $scope.sentimentDataset[0].data = [];
                    $scope.sentimentOptions.xaxis.ticks = [];
                    //Insert new data
                    _.forEach(data, function(obj, key) {
                        $scope.sentimentDataset[0].data.push([xAxisDateIndex, Math.round(helpers.formatPercent(obj.averageScore == null ? 0 : obj.averageScore))]);
                        $scope.sentimentOptions.xaxis.ticks.push([xAxisDateIndex, moment(obj.createdAt).format("M/DD")]);
                        xAxisDateIndex += 1;
                    });
                }
                else {
                    $scope.page.averageScore = "N/A";
                    $scope.page.averageDuration = "N/A";
                }
            });
            dashboardResource.getPositivitySpread(params).then(function (data) {
                if (data.length > 0) {
                    data = _.sortBy(data, function (obj) { return obj.createdAt });
                    var nonNullData = _.filter(data, function (obj, key) { return obj.positivity != null && obj.averageScore!= null });
                    if (nonNullData.length > 0) {
                        var goodCounts = _.filter(nonNullData, { positivity: "Good" });
                        var badCounts = _.filter(nonNullData, { positivity: "Bad" });
                        $scope.page.positivityCounts.countGood = _.sum(goodCounts, function (obj, key) { return obj.countOccurrences });
                        $scope.page.positivityCounts.countBad = _.sum(badCounts, function (obj, key) { return obj.countOccurrences });
                        var goodPercent = Math.round(helpers.formatPercent($scope.page.positivityCounts.countGood / ($scope.page.positivityCounts.countGood + $scope.page.positivityCounts.countBad)));
                        var badPercent = Math.round(helpers.formatPercent($scope.page.positivityCounts.countBad / ($scope.page.positivityCounts.countGood + $scope.page.positivityCounts.countBad)));
                        $scope.page.positivityCounts.percentGood = goodPercent + "%";
                        $scope.page.positivityCounts.percentBad = badPercent + "%";
                        $scope.easypiechart.percent = goodPercent;
                        $scope.easypiechart2.percent = badPercent;
                    
                        //Populate line graphs
                        var xAxisDateIndex = 1;
                        //Reset data
                        $scope.sentimentDataset[1].data = [];
                        $scope.sentimentDataset[2].data = [];
                        //Insert new data
                        _.forEach(data, function (obj, key) {
                            if(obj.positivity == "Bad")
                                $scope.sentimentDataset[1].data.push([xAxisDateIndex, Math.round(helpers.formatPercent(obj.averageScore == null ? 0 : obj.averageScore))]);
                            if (obj.positivity == "Good")
                                $scope.sentimentDataset[2].data.push([xAxisDateIndex, Math.round(helpers.formatPercent(obj.averageScore == null ? 0 : obj.averageScore))]);
                            xAxisDateIndex += 1;
                        });
                    }
                }
                else {
                    $scope.page.positivityCounts.countGood = "N/A";
                    $scope.page.positivityCounts.countBad = "N/A";
                    $scope.page.positivityCounts.percentGood = "N/A";
                    $scope.page.positivityCounts.percentBad = "N/A";
                    $scope.easypiechart.percent = 0;
                    $scope.easypiechart2.percent = 0;
                }
            });
            dashboardResource.getQuestionTextAvg(params).then(function (data) {
                if (data.length > 0) {
                    var uniqQuestionTexts = _.uniq(_.map(data, function (obj, key) { return obj.sequence })).sort();
                    _.forEach(uniqQuestionTexts, function (obj, key) {
                        var questionGroup = _.filter(data, { sequence: obj });
                        var averageScore = _.sum(questionGroup, function (o, k) { return o.averageScore }) / questionGroup.length;
                        $scope.responses.push({ sequence: obj, questionText: questionGroup[0].questionText, averageScore: helpers.formatPercent(averageScore) });
                    });
                    var totalQuestionSum = _.sum($scope.responses, function (obj, key) { return obj.averageScore });
                    $scope.donutData = [];
                    _.forEach($scope.responses, function (obj, key) {
                        var weightedPercent = obj.averageScore / totalQuestionSum;
                        $scope.donutData.push({ label: obj.questionText, value: Math.round(helpers.formatPercent(weightedPercent)), color: colorDictionary[$scope.donutData.length % colorDictionary.length] });
                    });
                }
                else {
                    $scope.responses = [];
                    $scope.donutData = [{ label: 'No data', value: 0, color: '#00a3d8' }];
                }
            });
            dashboardResource.getStaffMemberAvg(params).then(function (data) {
                if (data.length > 0) {
                    $scope.staffScores = []; //Reset Staff Scores
                    var uniqStaff = _.uniq(_.map(data, function (obj, key) { return obj.name })).sort();
                    _.forEach(uniqStaff, function (obj, key) {
                        var staffGroup = _.filter(data, { name: obj });
                        var countOccurrenceTotal = 0;
                        var averageScore = _.sum(staffGroup, function (o, k) { 
                            countOccurrenceTotal += o.countOccurrences; 
                            return o.averageScore * o.countOccurrences 
                        }) / countOccurrenceTotal;
                        $scope.staffScores.push({ staffMember: staffGroup[0].name, averageScore: Math.round(helpers.formatPercent(averageScore)) });
                    });
                }
                else {
                    $scope.staffScores = [];
                }
            });
        }

        function dateFilterChanged() {
            params.$filter = undefined;
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
            getDashboardStats();
        }
  }])

  .controller('StatisticsChartCtrl', function ($scope) {

    $scope.dataset = [{
      data: [[1,15],[2,40],[3,35],[4,39],[5,42],[6,50],[7,46],[8,49],[9,59],[10,60],[11,58],[12,74]],
      label: 'Unique Visits',
      points: {
        show: true,
        radius: 4
      },
      splines: {
        show: true,
        tension: 0.45,
        lineWidth: 4,
        fill: 0
      }
    }, {
      data: [[1,50],[2,80],[3,90],[4,85],[5,99],[6,125],[7,114],[8,96],[9,130],[10,145],[11,139],[12,160]],
      label: 'Page Views',
      bars: {
        show: true,
        barWidth: 0.6,
        lineWidth: 0,
        fillColor: { colors: [{ opacity: 0.3 }, { opacity: 0.8}] }
      }
    }];

    $scope.options = {
      colors: ['#e05d6f','#61c8b8'],
      series: {
        shadowSize: 0
      },
      legend: {
        backgroundOpacity: 0,
        margin: -7,
        position: 'ne',
        noColumns: 2
      },
      xaxis: {
        tickLength: 0,
        font: {
          color: '#fff'
        },
        position: 'bottom',
        ticks: [
          [ 1, 'JAN' ], [ 2, 'FEB' ], [ 3, 'MAR' ], [ 4, 'APR' ], [ 5, 'MAY' ], [ 6, 'JUN' ], [ 7, 'JUL' ], [ 8, 'AUG' ], [ 9, 'SEP' ], [ 10, 'OCT' ], [ 11, 'NOV' ], [ 12, 'DEC' ]
        ]
      },
      yaxis: {
        tickLength: 0,
        font: {
          color: '#fff'
        }
      },
      grid: {
        borderWidth: {
          top: 0,
          right: 0,
          bottom: 1,
          left: 1
        },
        borderColor: 'rgba(255,255,255,.3)',
        margin:0,
        minBorderMargin:0,
        labelMargin:20,
        hoverable: true,
        clickable: true,
        mouseActiveRadius:6
      },
      tooltip: true,
      tooltipOpts: {
        content: '%s: %y',
        defaultTheme: false,
        shifts: {
          x: 0,
          y: 20
        }
      }
    };
  })

  .controller('ActualStatisticsCtrl',function($scope){
    $scope.easypiechart = {
      percent: 100,
      options: {
        animate: {
          duration: 3000,
          enabled: true
        },
        barColor: '#418bca',
        scaleColor: false,
        lineCap: 'round',
        size: 140,
        lineWidth: 4
      }
    };
    $scope.easypiechart2 = {
      percent: 75,
      options: {
        animate: {
          duration: 3000,
          enabled: true
        },
        barColor: '#e05d6f',
        scaleColor: false,
        lineCap: 'round',
        size: 140,
        lineWidth: 4
      }
    };
    $scope.easypiechart3 = {
      percent: 46,
      options: {
        animate: {
          duration: 3000,
          enabled: true
        },
        barColor: '#16a085',
        scaleColor: false,
        lineCap: 'round',
        size: 140,
        lineWidth: 4
      }
    };
  })

  .controller('BrowseUsageCtrl', function ($scope) {

    $scope.donutData = [
      {label: 'Great Food', value: 25, color: '#00a3d8'},
      {label: 'Great Service', value: 20, color: '#2fbbe8'},
      {label: 'Poor Environment', value: 15, color: '#72cae7'},
      {label: 'Long Wait', value: 5, color: '#d9544f'},
      {label: 'Restroom was dirty', value: 10, color: '#ffc100'},
      {label: 'Needs Improvement', value: 25, color: '#1693A5'}
    ];

    $scope.oneAtATime = true;

    $scope.status = {
      isFirstOpen: true,
      tab1: {
        open: true
      },
      tab2: {
        open: false
      },
      tab3: {
        open: false
      }
    };

  })

  .controller('RealtimeLoadCtrl', function($scope, $interval){

    $scope.options1 = {
      renderer: 'area',
      height: 133
    };

    var seriesData = [ [], []];
    var random = new Rickshaw.Fixtures.RandomData(50);

    for (var i = 0; i < 50; i++) {
      random.addData(seriesData);
    }

    var updateInterval = 800;

    $interval(function() {
      random.removeData(seriesData);
      random.addData(seriesData);
    }, updateInterval);

    $scope.series1 = [{
      name: 'Series 1',
      color: 'steelblue',
      data: seriesData[0]
    }, {
      name: 'Series 2',
      color: 'lightblue',
      data: seriesData[1]
    }];

    $scope.features1 = {
      hover: {
        xFormatter: function(x) {
          return new Date(x * 1000).toUTCString();
        },
        yFormatter: function(y) {
          return Math.floor(y) + '%';
        }
      }
    };
  })

  .controller('ProjectProgressCtrl', function($scope, DTOptionsBuilder, DTColumnDefBuilder){
    $scope.projects = [{
      title: 'Graphic layout for client',
      priority: {
        value: 1,
        title: 'High Priority'
      },
      status: 42,
      chart: {
        data: [1,3,2,3,5,6,8,5,9,8],
        color: '#cd97eb'
      }
    },{
      title: 'Make website responsive',
      priority: {
        value: 3,
        title: 'Low Priority'
      },
      status: 89,
      chart: {
        data: [2,5,3,4,6,5,1,8,9,10],
        color: '#a2d200'
      }
    },{
      title: 'Clean html/css/js code',
      priority: {
        value: 1,
        title: 'High Priority'
      },
      status: 23,
      chart: {
        data: [5,6,8,2,1,6,8,4,3,5],
        color: '#ffc100'
      }
    },{
      title: 'Database optimization',
      priority: {
        value: 2,
        title: 'Normal Priority'
      },
      status: 56,
      chart: {
        data: [2,9,8,7,5,9,2,3,4,2],
        color: '#16a085'
      }
    },{
      title: 'Database migration',
      priority: {
        value: 3,
        title: 'Low Priority'
      },
      status: 48,
      chart: {
        data: [3,5,6,2,8,9,5,4,3,2],
        color: '#1693A5'
      }
    },{
      title: 'Email server backup',
      priority: {
        value: 2,
        title: 'Normal Priority'
      },
      status: 10,
      chart: {
        data: [7,8,6,4,3,5,8,9,10,7],
        color: '#3f4e62'
      }
    }];

    $scope.dtOptions = DTOptionsBuilder.newOptions().withBootstrap();
    $scope.dtColumnDefs = [
      DTColumnDefBuilder.newColumnDef(0),
      DTColumnDefBuilder.newColumnDef(1),
      DTColumnDefBuilder.newColumnDef(2),
      DTColumnDefBuilder.newColumnDef(3),
      DTColumnDefBuilder.newColumnDef(4).notSortable()
    ];
  });

