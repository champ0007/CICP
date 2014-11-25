/// <reference path="../angular.min.js" />
/// <reference path="../jquery-1.10.2.intellisense.js" />
var todos;

var AUDashboardApp = angular.module("AUDashboardApp", ["ngRoute", "tc.chartjs", "angularFileUpload", "angularUtils.directives.dirPagination"]);

AUDashboardApp.config(['$routeProvider',
    function ($routeProvider) {
        $routeProvider.
        when('/Dashboard', {
            templateUrl: 'partials/partialDashboard.html',
            controller: 'DashboardController'
        }).
        when('/ActionItems', {
            templateUrl: 'partials/ActionItems.html',
            controller: 'ActionItemsController'
        }).
        when('/ActiveProjects', {
            templateUrl: 'partials/ActiveProjects.html',
            controller: 'ActiveProjectsController'
        }).
        when('/ActiveResources', {
            templateUrl: 'partials/ActiveResources.html',
            controller: 'ActiveResourcesController'
        }).
        when('/NewActionItems', {
            templateUrl: 'partials/NewActionItems.html',
            controller: 'NewActionItemsController'
        }).
        when('/Operations', {
            templateUrl: 'partials/Operations.html',
            controller: 'OperationsController'
        }).
        when('/Invoices', {
            templateUrl: 'partials/Invoices.html',
            controller: 'InvoicesController'
        }).
        otherwise({
            redirectTo: '/Dashboard'
        });
    }
]);

AUDashboardApp.controller('DashboardController', ['$scope', '$http', function ($scope, $http) {

    $http({
        method: 'GET',
        url: 'api/Dashboard/getactiveprojects'
    }).
    success(function (data, status, headers, config) {
        $scope.ActiveProjects = data;
    }).
    error(function (data, status, headers, config) {
        // called asynchronously if an error occurs
        // or server returns response with an error status.
        $scope.ActiveProjects = -1;
    });


    $scope.PendingInvoices = 9;
    $scope.ActiveResources = 32;
    $scope.OpenActionItems = 5;
    var ProjectEntity;

    var FakeNotifications = [{
        message: '7 tasks added',
        eventdate: '47 mins ago',
        type: 'fa fa-tasks fa-fw'
    }, {
        message: 'AMP India visit',
        eventdate: '18-Aug',
        type: 'fa fa-tasks fa-fw'
    }, {
        message: 'Submit Project Report',
        eventdate: '20-Aug',
        type: 'fa fa-twitter fa-fw'
    }, {
        message: 'Telstra India Visit',
        eventdate: '1-Sep',
        type: 'fa fa-comment fa-fw'
    }, {
        message: 'EDC-AU meet',
        eventdate: '12-Sep',
        type: 'fa fa-calendar fa-fw'
    }, {
        message: 'EDC Upcoming Holiday',
        eventdate: '2-Oct',
        type: 'fa fa-tasks fa-fw'
    }];

    $scope.notifications = FakeNotifications;


    var InvoicesData = [{
        Project: 'Time Saver App',
        Partner: 'Hallam/Enderby',
        Resource: 'Tiwari Harsha, Harkala Ram',
        Period: 'P10_FY12',
        Date: '02/20/12 - 03/02/12',
        AmountUSD: 4860,
        ATBApproval: 'Received',
        ATBSentOn: '11/26/2012',
        InvoiceRaised: 'Yes',
        InvoiceNumber: '3000079888',
        InvoiceRaisedOn: '11/27/2012',
        Comments: '---',
        PaymentReceived: 'Received'
    }, {
        Project: 'Aus Super Spt',
        Partner: 'Milesi / Lipman',
        Resource: 'Tiwari Harsha, Harkala Ram',
        Period: 'P10_FY12',
        Date: '02/20/12 - 03/02/12',
        AmountUSD: 4860,
        ATBApproval: 'Received',
        ATBSentOn: '11/26/2012',
        InvoiceRaised: 'Yes',
        InvoiceNumber: '3000079888',
        InvoiceRaisedOn: '11/27/2012',
        Comments: '---',
        PaymentReceived: 'Received'
    }, {
        Project: 'ANZ',
        Partner: 'Carlisle / Adappa',
        Resource: 'Tiwari Harsha, Harkala Ram',
        Period: 'P10_FY12',
        Date: '02/20/12 - 03/02/12',
        AmountUSD: 4860,
        ATBApproval: 'Received',
        ATBSentOn: '11/26/2012',
        InvoiceRaised: 'Yes',
        InvoiceNumber: '3000079888',
        InvoiceRaisedOn: '11/27/2012',
        Comments: '---',
        PaymentReceived: 'Received'
    }, {
        Project: 'Time Saver App',
        Partner: 'Hallam/Enderby',
        Resource: 'Tiwari Harsha, Harkala Ram',
        Period: 'P10_FY12',
        Date: '02/20/12 - 03/02/12',
        AmountUSD: 4860,
        ATBApproval: 'Received',
        ATBSentOn: '11/26/2012',
        InvoiceRaised: 'Yes',
        InvoiceNumber: '3000079888',
        InvoiceRaisedOn: '11/27/2012',
        Comments: '---',
        PaymentReceived: 'Received'
    }, {
        Project: 'Time Saver App',
        Partner: 'Hallam/Enderby',
        Resource: 'Tiwari Harsha, Harkala Ram',
        Period: 'P10_FY12',
        Date: '02/20/12 - 03/02/12',
        AmountUSD: 4860,
        ATBApproval: 'Received',
        ATBSentOn: '11/26/2012',
        InvoiceRaised: 'Yes',
        InvoiceNumber: '3000079888',
        InvoiceRaisedOn: '11/27/2012',
        Comments: '---',
        PaymentReceived: 'Received'
    }, {
        Project: 'Time Saver App',
        Partner: 'Hallam/Enderby',
        Resource: 'Tiwari Harsha, Harkala Ram',
        Period: 'P10_FY12',
        Date: '02/20/12 - 03/02/12',
        AmountUSD: 4860,
        ATBApproval: 'Received',
        ATBSentOn: '11/26/2012',
        InvoiceRaised: 'Yes',
        InvoiceNumber: '3000079888',
        InvoiceRaisedOn: '11/27/2012',
        Comments: '---',
        PaymentReceived: 'Received'
    }, {
        Project: 'Time Saver App',
        Partner: 'Hallam/Enderby',
        Resource: 'Tiwari Harsha, Harkala Ram',
        Period: 'P10_FY12',
        Date: '02/20/12 - 03/02/12',
        AmountUSD: 4860,
        ATBApproval: 'Received',
        ATBSentOn: '11/26/2013',
        InvoiceRaised: 'Yes',
        InvoiceNumber: '3000079888',
        InvoiceRaisedOn: '11/27/2012',
        Comments: '---',
        PaymentReceived: 'Received'
    }, {
        Project: 'Time Saver App',
        Partner: 'Hallam/Enderby',
        Resource: 'Tiwari Harsha, Harkala Ram',
        Period: 'P10_FY12',
        Date: '02/20/12 - 03/02/12',
        AmountUSD: 4860,
        ATBApproval: 'Received',
        ATBSentOn: '11/26/2013',
        InvoiceRaised: 'Yes',
        InvoiceNumber: '3000079888',
        InvoiceRaisedOn: '11/27/2012',
        Comments: '---',
        PaymentReceived: 'Received'
    }, {
        Project: 'Time Saver App',
        Partner: 'Hallam/Enderby',
        Resource: 'Tiwari Harsha, Harkala Ram',
        Period: 'P10_FY12',
        Date: '02/20/12 - 03/02/12',
        AmountUSD: 4860,
        ATBApproval: 'Received',
        ATBSentOn: '11/26/2013',
        InvoiceRaised: 'Yes',
        InvoiceNumber: '3000079888',
        InvoiceRaisedOn: '11/27/2013',
        Comments: '---',
        PaymentReceived: 'Received'
    }, {
        Project: 'Time Saver App',
        Partner: 'Hallam/Enderby',
        Resource: 'Tiwari Harsha, Harkala Ram',
        Period: 'P10_FY12',
        Date: '02/20/12 - 03/02/12',
        AmountUSD: 4860,
        ATBApproval: 'Received',
        ATBSentOn: '11/26/2012',
        InvoiceRaised: 'Yes',
        InvoiceNumber: '3000079888',
        InvoiceRaisedOn: '11/27/2012',
        Comments: '---',
        PaymentReceived: 'Received'
    }, {
        Project: 'Time Saver App',
        Partner: 'Hallam/Enderby',
        Resource: 'Tiwari Harsha, Harkala Ram',
        Period: 'P10_FY12',
        Date: '02/20/12 - 03/02/12',
        AmountUSD: 4860,
        ATBApproval: 'Received',
        ATBSentOn: '11/26/2012',
        InvoiceRaised: 'Yes',
        InvoiceNumber: '3000079888',
        InvoiceRaisedOn: '11/27/2012',
        Comments: '---',
        PaymentReceived: 'Pending'
    }, {
        Project: 'Time Saver App',
        Partner: 'Hallam/Enderby',
        Resource: 'Tiwari Harsha, Harkala Ram',
        Period: 'P10_FY12',
        Date: '02/20/12 - 03/02/12',
        AmountUSD: 4860,
        ATBApproval: 'Received',
        ATBSentOn: '11/26/2014',
        InvoiceRaised: 'Yes',
        InvoiceNumber: '3000079888',
        InvoiceRaisedOn: '11/27/2012',
        Comments: '---',
        PaymentReceived: 'Received'
    }, {
        Project: 'Time Saver App',
        Partner: 'Hallam/Enderby',
        Resource: 'Tiwari Harsha, Harkala Ram',
        Period: 'P10_FY12',
        Date: '02/20/12 - 03/02/12',
        AmountUSD: 4860,
        ATBApproval: 'Received',
        ATBSentOn: '11/26/2014',
        InvoiceRaised: 'Yes',
        InvoiceNumber: '3000079888',
        InvoiceRaisedOn: '11/27/2012',
        Comments: '---',
        PaymentReceived: 'Received'
    }, {
        Project: 'Time Saver App',
        Partner: 'Hallam/Enderby',
        Resource: 'Tiwari Harsha, Harkala Ram',
        Period: 'P10_FY12',
        Date: '02/20/12 - 03/02/12',
        AmountUSD: 4860,
        ATBApproval: 'Received',
        ATBSentOn: '11/26/2014',
        InvoiceRaised: 'Yes',
        InvoiceNumber: '3000079888',
        InvoiceRaisedOn: '11/27/2012',
        Comments: '---',
        PaymentReceived: 'Received'
    }, {
        Project: 'Time Saver App',
        Partner: 'Hallam/Enderby',
        Resource: 'Tiwari Harsha, Harkala Ram',
        Period: 'P10_FY12',
        Date: '02/20/12 - 03/02/12',
        AmountUSD: 4860,
        ATBApproval: 'Received',
        ATBSentOn: '11/26/2012',
        InvoiceRaised: 'Yes',
        InvoiceNumber: '3000079888',
        InvoiceRaisedOn: '11/27/2012',
        Comments: '---',
        PaymentReceived: 'Received'
    }, {
        Project: 'Time Saver App',
        Partner: 'Hallam/Enderby',
        Resource: 'Sharma, Tushar',
        Period: 'P10_FY12',
        Date: '02/20/12 - 03/02/12',
        AmountUSD: 4860,
        ATBApproval: 'Received',
        ATBSentOn: '11/26/2012',
        InvoiceRaised: 'Yes',
        InvoiceNumber: '3000079888',
        InvoiceRaisedOn: '11/27/2012',
        Comments: '---',
        PaymentReceived: 'Pending'
    }, {
        Project: 'Time Saver App',
        Partner: 'Hallam/Enderby',
        Resource: 'Tiwari Harsha, Harkala Ram',
        Period: 'P10_FY12',
        Date: '02/20/12 - 03/02/12',
        AmountUSD: 4860,
        ATBApproval: 'Received',
        ATBSentOn: '11/26/2012',
        InvoiceRaised: 'Yes',
        InvoiceNumber: '3000079888',
        InvoiceRaisedOn: '11/27/2012',
        Comments: '---',
        PaymentReceived: 'Received'
    }, {
        Project: 'Time Saver App',
        Partner: 'Hallam/Enderby',
        Resource: 'Tiwari Harsha, Harkala Ram',
        Period: 'P10_FY12',
        Date: '02/20/12 - 03/02/12',
        AmountUSD: 4860,
        ATBApproval: 'Received',
        ATBSentOn: '11/26/2012',
        InvoiceRaised: 'Yes',
        InvoiceNumber: '3000079888',
        InvoiceRaisedOn: '11/27/2012',
        Comments: '---',
        PaymentReceived: 'Received'
    }, {
        Project: 'Time Saver App',
        Partner: 'Hallam/Enderby',
        Resource: 'Tiwari Harsha, Harkala Ram',
        Period: 'P10_FY12',
        Date: '02/20/12 - 03/02/12',
        AmountUSD: 4860,
        ATBApproval: 'Received',
        ATBSentOn: '11/26/2012',
        InvoiceRaised: 'Yes',
        InvoiceNumber: '3000079888',
        InvoiceRaisedOn: '11/27/2012',
        Comments: '---',
        PaymentReceived: 'Received'
    }];


    var FakeInvoicesData = [{
        Project: 'Project 1',
        Partner: 'Partner 1',
        Resource: 'Resource 1, Resource 2',
        Period: 'P10_FY13',
        Date: '02/20/17 - 03/02/17',
        AmountUSD: 1234,
        ATBApproval: 'Received',
        ATBSentOn: '11/26/2017',
        InvoiceRaised: 'Yes',
        InvoiceNumber: '123456789',
        InvoiceRaisedOn: '11/27/2017',
        Comments: '---',
        PaymentReceived: 'Received'
    }, {
        Project: 'Project 2',
        Partner: 'Partner 2',
        Resource: 'Resource 2, Resource 3',
        Period: 'P10_FY13',
        Date: '02/20/18 - 03/02/18',
        AmountUSD: 5678,
        ATBApproval: 'Received',
        ATBSentOn: '11/26/2018',
        InvoiceRaised: 'Yes',
        InvoiceNumber: '987654321',
        InvoiceRaisedOn: '11/27/2017',
        Comments: '---',
        PaymentReceived: 'Received'
    }, {
        Project: 'Project 3',
        Partner: 'Partner 3',
        Resource: 'Resource 3, Resource 4',
        Period: 'P10_FY13',
        Date: '02/20/19 - 03/02/19',
        AmountUSD: 9012,
        ATBApproval: 'Received',
        ATBSentOn: '11/26/2019',
        InvoiceRaised: 'Yes',
        InvoiceNumber: '111111111',
        InvoiceRaisedOn: '11/27/2017',
        Comments: '---',
        PaymentReceived: 'Received'
    }, {
        Project: 'Project 4',
        Partner: 'Partner 4',
        Resource: 'Resource 3, Resource 2',
        Period: 'P10_FY13',
        Date: '02/20/18 - 03/02/18',
        AmountUSD: 1234,
        ATBApproval: 'Received',
        ATBSentOn: '11/26/2017',
        InvoiceRaised: 'Yes',
        InvoiceNumber: '987654321',
        InvoiceRaisedOn: '11/27/2017',
        Comments: '---',
        PaymentReceived: 'Received'
    }, {
        Project: 'Project 5',
        Partner: 'Partner 1',
        Resource: 'Resource 1, Resource 4',
        Period: 'P10_FY13',
        Date: '02/20/19 - 03/02/19',
        AmountUSD: 5678,
        ATBApproval: 'Received',
        ATBSentOn: '11/26/2018',
        InvoiceRaised: 'Yes',
        InvoiceNumber: '222222222',
        InvoiceRaisedOn: '11/27/2017',
        Comments: '---',
        PaymentReceived: 'Received'
    }, {
        Project: 'Project 6',
        Partner: 'Partner 2',
        Resource: 'Resource 1, Resource 2',
        Period: 'P10_FY13',
        Date: '02/20/18 - 03/02/18',
        AmountUSD: 9876,
        ATBApproval: 'Received',
        ATBSentOn: '11/26/2019',
        InvoiceRaised: 'Yes',
        InvoiceNumber: '987654321',
        InvoiceRaisedOn: '11/27/2017',
        Comments: '---',
        PaymentReceived: 'Received'
    }, {
        Project: 'Project 7',
        Partner: 'Partner 3',
        Resource: 'Resource 1, Resource 2',
        Period: 'P10_FY13',
        Date: '02/20/19 - 03/02/19',
        AmountUSD: 5432,
        ATBApproval: 'Received',
        ATBSentOn: '11/26/2017',
        InvoiceRaised: 'Yes',
        InvoiceNumber: '112233445',
        InvoiceRaisedOn: '11/27/2017',
        Comments: '---',
        PaymentReceived: 'Received'
    }, {
        Project: 'Project 8',
        Partner: 'Partner 4',
        Resource: 'Resource 1, Resource 2',
        Period: 'P10_FY13',
        Date: '02/20/18 - 03/02/18',
        AmountUSD: 4321,
        ATBApproval: 'Received',
        ATBSentOn: '11/26/2018',
        InvoiceRaised: 'Yes',
        InvoiceNumber: '987654321',
        InvoiceRaisedOn: '11/27/2017',
        Comments: '---',
        PaymentReceived: 'Received'
    },

    ];


    $scope.AllInvoices = FakeInvoicesData;

    //Start Key Updates
    var keyUpdates = $scope.keyUpdates = [];

    $scope.getKeyUpdates = function () {

        $http({
            method: 'GET',
            url: 'api/Dashboard/GetReferenceData?storageId=KeyUpdates'
        }).
        success(function (data, status, headers, config) {

            if (data != 'null') {
                keyUpdates = $scope.keyUpdates = JSON.parse(JSON.parse(data));
            }
        }).
        error(function (data, status, headers, config) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.

        });

    };

    $scope.EditKeyUpdates = function (keyUpdate, index) {
        keyUpdate.index = index;
        $scope.keyUpdate = jQuery.extend(true, {}, keyUpdate); // deep copy
        $scope.OriginalKeyUpdate = jQuery.extend(true, {}, keyUpdate); // deep copy

    }

    $scope.getKeyUpdates();

    $scope.setKeyUpdates = function (keyUpdates) {
        var referenceData = new Object();
        referenceData.storageId = 'KeyUpdates';
        referenceData.storageData = JSON.stringify(keyUpdates);
        $http({
            url: 'api/Dashboard/SetReferenceData',
            method: "POST",
            data: JSON.stringify(JSON.stringify(referenceData))
        })
            .then(function (response) {
                $scope.getKeyUpdates();
            },
                function (response) { // optional
                }
            );
    };

    $scope.$watch('keyUpdates', function (newValue, oldValue) {
        if (newValue !== oldValue) { // This prevents unneeded calls to the local storage
            $scope.setKeyUpdates(keyUpdates);
        }
    }, true);

    $scope.AddKeyUpdate = function (keyUpdate) {
        if (keyUpdate.index >= 0) {
            keyUpdates[keyUpdate.index] = keyUpdate;
        } else {
            keyUpdates.push(keyUpdate);
        }

        $scope.keyUpdates = keyUpdates;
        $scope.keyUpdate = '';
    };
    //End Key updates

    //Start 
    // Chart.js Data
    $scope.skillChartData = {
        labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
        datasets: [
          {
              label: 'FY15 Business',
              fillColor: '#FFFFFF',
              strokeColor: 'rgba(69,201,102,0.9)',
              highlightFill: 'rgba(69,201,102,0.9)',
              highlightStroke: 'rgba(220,220,220,1)',
              data: [65, 59, 80, 81, 56, 55, 40]
          },
          {
              label: 'FY14 Business',
              fillColor: '#FFFFFF',
              strokeColor: 'rgba(1,187,205,0.8)',
              highlightFill: 'rgba(38, 208, 255, 0.75)',
              highlightStroke: 'rgba(151,187,205,1)',
              data: [28, 48, 40, 19, 86, 27, 30]
          }
        ]
    };

    // Chart.js Options
    $scope.skillChartOptions = {

        // Sets the chart to be responsive
        responsive: true,

        //Boolean - Whether the scale should start at zero, or an order of magnitude down from the lowest value
        scaleBeginAtZero: true,

        //Boolean - Whether grid lines are shown across the chart
        scaleShowGridLines: true,

        //String - Colour of the grid lines
        scaleGridLineColor: "rgba(0,0,0,.05)",

        //Number - Width of the grid lines
        scaleGridLineWidth: 1,

        //Boolean - If there is a stroke on each bar
        barShowStroke: true,

        //Number - Pixel width of the bar stroke
        barStrokeWidth: 2,

        //Number - Spacing between each of the X value sets
        barValueSpacing: 5,

        //Number - Spacing between data sets within X values
        barDatasetSpacing: 1,

        //String - A legend template
        legendTemplate: '<ul class="tc-chart-js-legend"><% for (var i=0; i<datasets.length; i++){%><li><span style="background-color:<%=datasets[i].fillColor%>"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>'
    };

    // Chart.js Data
    $scope.ProjectChartData = [
      {
          value: 300,
          color: '#F7464A',
          highlight: '#FF5A5E',
          label: 'Sitecore'
      },
      {
          value: 50,
          color: '#46BFBD',
          highlight: '#5AD3D1',
          label: 'Hybris'
      },
      {
          value: 100,
          color: '#FDB45C',
          highlight: '#FFC870',
          label: 'Adobe CQ'
      }
    ];

    // Chart.js Options
    $scope.ProjectChartOptions = {

        // Sets the chart to be responsive
        responsive: true,

        //Boolean - Whether we should show a stroke on each segment
        segmentShowStroke: true,

        //String - The colour of each segment stroke
        segmentStrokeColor: '#fff',

        //Number - The width of each segment stroke
        segmentStrokeWidth: 2,

        //Number - The percentage of the chart that we cut out of the middle
        percentageInnerCutout: 50, // This is 0 for Pie charts

        //Number - Amount of animation steps
        animationSteps: 100,

        //String - Animation easing effect
        animationEasing: 'easeOutQuint',

        //Boolean - Whether we animate the rotation of the Doughnut
        animateRotate: false,

        //Boolean - Whether we animate scaling the Doughnut from the centre
        animateScale: false,

        //String - A legend template     
        legendTemplate: '<ul class="tc-chart-js-legend"><% for (var i=0; i<datasets.length; i++){%><li><span style="background-color:<%=datasets[i].fillColor%>"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>'

    };


    //End


}]);

AUDashboardApp.controller('ActionItemsController', ['$scope', '$filter', '$http', function ($scope, $filter, $http) {

    var todos = $scope.todos = [];

    $scope.newTodo = '';
    $scope.newAssignedTo = '';
    $scope.editedTodo = null;
    var STORAGE_ID = 'ToDoItems';

    $scope.$watch('todos', function (newValue, oldValue) {
        //$scope.remainingCount = $filter('filter')(todos, { completed: false }).length;
        //$scope.completedCount = todos.length - $scope.remainingCount;
        //$scope.allChecked = !$scope.remainingCount;
        if (newValue !== oldValue) { // This prevents unneeded calls to the local storage
            $scope.setTodos(todos);
        }
    }, true);

    //// Monitor the current route for changes and adjust the filter accordingly.
    //$scope.$on('$routeChangeSuccess', function () {
    //    var status = $scope.status = $routeParams.status || '';

    //    $scope.statusFilter = (status === 'active') ?
    //			{ completed: false } : (status === 'completed') ?
    //			{ completed: true } : null;
    //});

    $scope.getTodos = function () {

        $http({
            method: 'GET',
            url: 'api/Dashboard/GetReferenceData?storageId=' + STORAGE_ID
        }).
        success(function (data, status, headers, config) {

            if (data != 'null') {
                todos = $scope.todos = JSON.parse(JSON.parse(data));
                $scope.$parent.OpenActionItems = todos.length;
            }
        }).
        error(function (data, status, headers, config) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.

        });

    };

    $scope.getTodos();

    $scope.setTodos = function (todos) {
        //localStorage.setItem(STORAGE_ID, JSON.stringify(todos));
        //debugger;
        var referenceData = new Object();
        referenceData.storageId = STORAGE_ID;
        referenceData.storageData = JSON.stringify(todos);
        $http({
            url: 'api/Dashboard/SetReferenceData',
            method: "POST",
            data: JSON.stringify(JSON.stringify(referenceData))
        })
            .then(function (response) { },
                function (response) { // optional
                }
            );
    };

    $scope.addTodo = function () {
        //debugger;
        var newTodo = $scope.newTodo.trim();
        var newAssignedTo = $scope.newAssignedTo.trim();
        if (!newTodo.length && !newTodo.length) {
            return;
        }
        todos.push({
            title: newTodo,
            AssignedTo: newAssignedTo,
            completed: false
        });

        $scope.newTodo = '';
        $scope.newAssignedTo = '';
    };

    $scope.editTodo = function (todo) {
        $scope.editedTodo = todo;
        // Clone the original todo to restore it on demand.
        $scope.originalTodo = angular.extend({}, todo);
    };

    $scope.doneEditing = function (todo) {
        $scope.editedTodo = null;
        todo.title = todo.title.trim();
        todo.AssignedTo = todo.AssignedTo.trim();

        if (!todo.title) {
            $scope.removeTodo(todo);
        }
    };

    $scope.revertEditing = function (todo) {
        todos[todos.indexOf(todo)] = $scope.originalTodo;
        $scope.doneEditing($scope.originalTodo);
    };

    $scope.removeTodo = function (todo) {
        todos.splice(todos.indexOf(todo), 1);
    };

    $scope.clearCompletedTodos = function () {
        $scope.todos = todos = todos.filter(function (val) {
            return !val.completed;
        });
    };

    $scope.markAll = function (completed) {
        todos.forEach(function (todo) {
            todo.completed = !completed;
        });
    };
}]);

AUDashboardApp.controller('ActiveProjectsController', ['$scope', '$filter', '$http', function ($scope, $filter, $http) {
    var STORAGE_ID = 'Projects';
    $scope.EditMode = "false";

    var ProjectDetails = $scope.ActiveProjectDetails = [];

    $scope.getProjects = function () {
        //debugger;
        $http({
            method: 'GET',
            url: 'api/Dashboard/GetReferenceData?storageId=' + STORAGE_ID
        }).
        success(function (data, status, headers, config) {

            if (data != 'null') {
                ProjectDetails = $scope.ActiveProjectDetails = JSON.parse(JSON.parse(data));
                $scope.$parent.ActiveProjects = ProjectDetails.length;
                $scope.UpdateChart();
            }
        }).
        error(function (data, status, headers, config) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.

        });

    };

    $scope.EditProject = function (project, index) {
        project.index = index;
        $scope.ProjectEntity = jQuery.extend(true, {}, project); // deep copy
        $scope.OriginalProject = jQuery.extend(true, {}, project); // deep copy
    }

    $scope.setProjects = function (ProjectDetails) {
        var referenceData = new Object();
        referenceData.storageId = STORAGE_ID;
        referenceData.storageData = JSON.stringify(ProjectDetails);
        $http({
            url: 'api/Dashboard/SetReferenceData',
            method: "POST",
            data: JSON.stringify(JSON.stringify(referenceData))
        })
            .then(function (response) {
                $scope.getProjects();
            },
                function (response) { // optional
                }
            );
    };

    $scope.$watch('ActiveProjectDetails', function (newValue, oldValue) {
        if (newValue !== oldValue) { // This prevents unneeded calls to the local storage
            $scope.setProjects(ProjectDetails);
        }
    }, true);

    $scope.AddProject = function (ProjectEntity) {
        //debugger;
        if (ProjectEntity.index >= 0) {
            ProjectDetails[ProjectEntity.index] = ProjectEntity;
        } else {
            ProjectDetails.push(ProjectEntity);
        }

        $scope.ActiveProjectDetails = ProjectDetails;
        $scope.ProjectEntity = '';
    };

    $scope.UpdateChart = function () {
        $http({
            method: 'GET',
            url: 'api/Dashboard/GetProjectChartData'
        }).
      success(function (data, status, headers, config) {
          if (data != null) {
              debugger;
              $scope.chartLabels = JSON.parse(data[0]);
              $scope.chartData = JSON.parse(data[1]);
              $scope.ActiveProjectChartData.labels = $scope.chartLabels;
              $scope.ActiveProjectChartData.datasets[0].data = $scope.chartData;
          }
      }).
      error(function (data, status, headers, config) {
          // called asynchronously if an error occurs
          // or server returns response with an error status.
          $scope.AllResources = -1;
      });
    };


    // Chart.js Data
    $scope.ActiveProjectChartData = {
        labels: $scope.chartLabels,
        datasets: [
          {
              label: 'Project Status',
              fillColor: 'rgba(220,220,220,0.5)',
              strokeColor: 'rgba(220,220,220,0.8)',
              highlightFill: 'rgba(220,220,220,0.75)',
              highlightStroke: 'rgba(220,220,220,1)',
              data: $scope.chartData
          }
        ]
    };

    // Chart.js Options
    $scope.ActiveProjectChartOptions = {

        // Sets the chart to be responsive
        responsive: true,

        //Boolean - Whether the scale should start at zero, or an order of magnitude down from the lowest value
        scaleBeginAtZero: true,

        //Boolean - Whether grid lines are shown across the chart
        scaleShowGridLines: true,

        //String - Colour of the grid lines
        scaleGridLineColor: "rgba(0,0,0,.05)",

        //Number - Width of the grid lines
        scaleGridLineWidth: 1,

        //Boolean - If there is a stroke on each bar
        barShowStroke: true,

        //Number - Pixel width of the bar stroke
        barStrokeWidth: 2,

        //Number - Spacing between each of the X value sets
        barValueSpacing: 15,

        //Number - Spacing between data sets within X values
        barDatasetSpacing: 50,

        //String - A legend template
        legendTemplate: '<ul class="tc-chart-js-legend"><% for (var i=0; i<datasets.length; i++){%><li><span style="background-color:<%=datasets[i].fillColor%>"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>'
    };

    $scope.getProjects();

}]);

AUDashboardApp.controller('ActiveResourcesController', ['$scope', '$http', 'FileUploader', function ($scope, $http, FileUploader) {
    var STORAGE_ID = 'Resources';
    $scope.EditMode = "false";

    var resources = $scope.AllResources = [];

    $scope.getResources = function () {
        $http({
            method: 'GET',
            url: 'api/Dashboard/GetReferenceData?storageId=' + STORAGE_ID
        }).
        success(function (data, status, headers, config) {
            if (data != null) {
                resources = $scope.AllResources = JSON.parse(JSON.parse(data));
                $scope.$parent.ActiveResources = $scope.AllResources.length;
                $scope.UpdateChart();
            }
        }).
        error(function (data, status, headers, config) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.
            $scope.AllResources = -1;
        });
    };



    $scope.setResources = function (resourcesToBeSaved) {
        var referenceData = new Object();
        referenceData.storageId = STORAGE_ID;
        referenceData.storageData = JSON.stringify(resourcesToBeSaved);
        $http({
            url: 'api/Dashboard/SetReferenceData',
            method: "POST",
            data: JSON.stringify(JSON.stringify(referenceData))
        })
            .then(function (response) {
                $scope.getResources();
            },
                function (response) { // optional
                }
            );
    };

    $scope.$watch('AllResources', function (newValue, oldValue) {
        if (newValue !== oldValue) { // This prevents unneeded calls to the local storage
            $scope.setResources(resources);
        }
    }, true);

    $scope.EditResource = function (resource, index) {
        debugger;
        resource.index = index;
        $scope.EditMode = "true";
        //Shallow Copy - $scope.ResourceEntity = resource;
        $scope.ResourceEntity = jQuery.extend(true, {}, resource); // deep copy
        $scope.OriginalResourceEntity = jQuery.extend(true, {}, resource); // deep copy
    }

    $scope.addResource = function (resource) {
        //debugger;
        //debugger;
        if (resource.index >= 0) {
            resources[resource.index] = resource;
        } else {
            resources.push(resource);
        }
        $scope.AllResources = resources;
        $scope.ResourceEntity = '';

    };

    var chartLabels, chartData;

    $scope.UpdateChart = function () {
        $http({
            method: 'GET',
            url: 'api/Dashboard/GetResourceChartData'
        }).
      success(function (data, status, headers, config) {
          if (data != null) {
              debugger;
              chartLabels = JSON.parse(data[0]);
              chartData = JSON.parse(data[1]);
          }
      }).
      error(function (data, status, headers, config) {
          // called asynchronously if an error occurs
          // or server returns response with an error status.
          $scope.AllResources = -1;
      });


        // Chart.js Data
        $scope.ResourceChartData = {
            labels: chartLabels, // ['AMP', 'Telstra', 'QUU', 'AusSuper', 'ANZ', 'Caltex'],
            datasets: [
              {
                  label: 'My First dataset',
                  fillColor: '#FFFFFF',
                  strokeColor: '#000000',
                  highlightFill: '#000000',
                  highlightStroke: '#FFFFFF',
                  data: chartData //[65, 59, 80, 81, 56, 55]
              }

            ]
        };

        // Chart.js Options
        $scope.ResourceChartOptions = {

            // Sets the chart to be responsive
            responsive: true,

            //Boolean - Whether the scale should start at zero, or an order of magnitude down from the lowest value
            scaleBeginAtZero: true,

            //Boolean - Whether grid lines are shown across the chart
            scaleShowGridLines: true,

            //String - Colour of the grid lines
            scaleGridLineColor: "rgba(0,0,0,.05)",

            //Number - Width of the grid lines
            scaleGridLineWidth: 1,

            //Boolean - If there is a stroke on each bar
            barShowStroke: true,

            //Number - Pixel width of the bar stroke
            barStrokeWidth: 2,

            //Number - Spacing between each of the X value sets
            barValueSpacing: 15,

            //Number - Spacing between data sets within X values
            barDatasetSpacing: 1,

            //String - A legend template
            legendTemplate: '<ul class="tc-chart-js-legend"><% for (var i=0; i<datasets.length; i++){%><li><span style="background-color:<%=datasets[i].fillColor%>"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>'
        };

    };

    //File upload functionality
    var uploader = $scope.uploader = new FileUploader({
        url: 'api/Dashboard/UploadResources',
        autoUpload: true
        //removeAfterUpload: true
    });

    uploader.onCompleteItem = function (fileItem, response, status, headers) {
        $scope.getResources();
    };

    $scope.getResources();

}]);

AUDashboardApp.controller('NewActionItemsController', ['$scope', '$filter', '$http', function ($scope, $filter, $http) {
    var STORAGE_ID = 'NewToDoItems'; // To be passed
    //$scope.EditMode = "false";

    var NewToDos = $scope.NewToDos = [];

    $scope.getNewToDos = function () {
        $http({
            method: 'GET',
            url: 'api/Dashboard/GetReferenceData?storageId=' + STORAGE_ID
        }).
        success(function (data, status, headers, config) {

            if (data != 'null') {
                NewToDos = $scope.NewToDos = JSON.parse(JSON.parse(data));
                $scope.$parent.OpenActionItems = NewToDos.length;
                $scope.UpdateChart($filter('filter')(NewToDos, { Status: "Open" }).length, $filter('filter')(NewToDos, { Status: "Closed" }).length, $filter('filter')(NewToDos, { Status: "Pending" }).length);
            }
        }).
        error(function (data, status, headers, config) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.

        });

    };

    $scope.EditToDoItem = function (ToDoItem, index) {
        ToDoItem.index = index;
        $scope.ToDoItem = jQuery.extend(true, {}, ToDoItem); // deep copy
        $scope.OriginalToDoItem = jQuery.extend(true, {}, ToDoItem); // deep copy
    }

    $scope.setToDoItems = function (NewToDos) {
        var referenceData = new Object();
        referenceData.storageId = STORAGE_ID;
        referenceData.storageData = JSON.stringify(NewToDos);
        $http({
            url: 'api/Dashboard/SetReferenceData',
            method: "POST",
            data: JSON.stringify(JSON.stringify(referenceData))
        })
            .then(function (response) {
                $scope.getNewToDos();
            },
                function (response) { // optional
                }
            );
    };

    $scope.$watch('NewToDos', function (newValue, oldValue) {
        if (newValue !== oldValue) { // This prevents unneeded calls to the local storage
            $scope.setToDoItems(NewToDos);
        }
    }, true);

    $scope.AddToDoItem = function (ToDoItem) {
        ToDoItem.Status = 'Open';
        if (ToDoItem.index >= 0) {
            NewToDos[ToDoItem.index] = ToDoItem;
        } else {
            NewToDos.push(ToDoItem);
        }

        $scope.NewToDos = NewToDos;
        $scope.NewToDoItem = '';
    };

    $scope.UpdateChart = function (open, closed, pending) {
        // Chart.js Data
        $scope.ActionItemChartData = [
          {
              value: open,
              color: '#F7464A',
              highlight: '#FF5A5E',
              label: 'Open'
          },
          {
              value: closed,
              color: '#46BFBD',
              highlight: '#5AD3D1',
              label: 'Closed'
          },
          {
              value: pending,
              color: '#FDB45C',
              highlight: '#FFC870',
              label: 'Pending'
          }
        ];

        // Chart.js Options
        $scope.ActionItemChartOptions = {

            // Sets the chart to be responsive
            responsive: true,

            //Boolean - Whether we should show a stroke on each segment
            segmentShowStroke: true,

            //String - The colour of each segment stroke
            segmentStrokeColor: '#fff',

            //Number - The width of each segment stroke
            segmentStrokeWidth: 2,

            //Number - The percentage of the chart that we cut out of the middle
            percentageInnerCutout: 0, // This is 0 for Pie charts

            //Number - Amount of animation steps
            animationSteps: 100,

            //String - Animation easing effect
            animationEasing: 'easeOutBounce',

            //Boolean - Whether we animate the rotation of the Doughnut
            animateRotate: true,

            //Boolean - Whether we animate scaling the Doughnut from the centre
            animateScale: false,

            //String - A legend template
            legendTemplate: '<ul class="tc-chart-js-legend"><% for (var i=0; i<segments.length; i++){%><li><span style="background-color:<%=segments[i].fillColor%>"></span><%if(segments[i].label){%><%=segments[i].label%><%}%></li><%}%></ul>'

        };
    };

    $scope.getNewToDos();

}]);

AUDashboardApp.controller('OperationsController', ['$scope', '$http', function ($scope, $http) {

    //Start Key Updates
    var keyUpdates = $scope.keyUpdates = [];

    $scope.getKeyUpdates = function () {
        //debugger;
        $http({
            method: 'GET',
            url: 'api/Dashboard/GetReferenceData?storageId=KeyUpdates'
        }).
        success(function (data, status, headers, config) {

            if (data != 'null') {
                keyUpdates = $scope.keyUpdates = JSON.parse(JSON.parse(data));
            }
        }).
        error(function (data, status, headers, config) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.

        });

    };

    $scope.EditKeyUpdates = function (keyUpdate, index) {
        keyUpdate.index = index;
        $scope.keyUpdate = jQuery.extend(true, {}, keyUpdate); // deep copy
        $scope.OriginalKeyUpdate = jQuery.extend(true, {}, keyUpdate); // deep copy

    }

    $scope.getKeyUpdates();

    $scope.setKeyUpdates = function (keyUpdates) {
        var referenceData = new Object();
        referenceData.storageId = 'KeyUpdates';
        referenceData.storageData = JSON.stringify(keyUpdates);
        $http({
            url: 'api/Dashboard/SetReferenceData',
            method: "POST",
            data: JSON.stringify(JSON.stringify(referenceData))
        })
            .then(function (response) {
                $scope.getKeyUpdates();
            },
                function (response) { // optional
                }
            );
    };

    $scope.$watch('keyUpdates', function (newValue, oldValue) {
        if (newValue !== oldValue) { // This prevents unneeded calls to the local storage
            $scope.setKeyUpdates(keyUpdates);
        }
    }, true);

    $scope.AddKeyUpdate = function (keyUpdate) {
        if (keyUpdate.index >= 0) {
            keyUpdates[keyUpdate.index] = keyUpdate;
        } else {
            keyUpdates.push(keyUpdate);
        }

        $scope.keyUpdates = keyUpdates;
        $scope.keyUpdate = '';
    };
    //End Key updates

    //Start 

    $scope.ResourceDeploymentData = {
        labels: ['April', 'May', 'June', 'July', 'August', 'September', 'October'],
        datasets: [
          {
              label: 'FY15 Business',
              fillColor: '#0cc09f',//rgba(69,201,102,0.75)',
              strokeColor: 'rgba(220,220,220,0.8)',
              highlightFill: '#0aac8e',
              highlightStroke: 'rgba(220,220,220,1)',
              data: [40, 66, 65, 81, 75, 75, 80]
          }
        ]
    };

    // Chart.js Data
    $scope.skillChartData = {
        labels: ['April', 'May', 'June', 'July', 'August', 'September', 'October'],
        datasets: [
          {
              label: 'FY15 Business',
              fillColor: 'rgba(620,201,102,0.75)',
              strokeColor: 'rgba(220,220,220,0.8)',
              highlightFill: 'rgba(69,201,102,0.9)',
              highlightStroke: 'rgba(220,220,220,1)',
              data: [65, 59, 80, 81, 56, 55, 40]
          },
          {
              label: 'FY14 Business',
              fillColor: 'rgba(321, 176, 0, 1)',
              strokeColor: 'rgba(321, 176, 0, 0.8)',
              highlightFill: 'rgba(38, 208, 255, 0.75)',
              highlightStroke: 'rgba(151,187,205,1)',
              data: [28, 48, 40, 19, 86, 27, 30]
          }
        ]
    };

    // Chart.js Options
    $scope.skillChartOptions = {

        // Sets the chart to be responsive
        responsive: true,

        //Boolean - Whether the scale should start at zero, or an order of magnitude down from the lowest value
        scaleBeginAtZero: true,

        //Boolean - Whether grid lines are shown across the chart
        scaleShowGridLines: true,

        //String - Colour of the grid lines
        scaleGridLineColor: "rgba(0,0,0,.05)",

        //Number - Width of the grid lines
        scaleGridLineWidth: 1,

        //Boolean - If there is a stroke on each bar
        barShowStroke: true,

        //Number - Pixel width of the bar stroke
        barStrokeWidth: 2,

        //Number - Spacing between each of the X value sets
        barValueSpacing: 5,

        //Number - Spacing between data sets within X values
        barDatasetSpacing: 1,

        //String - A legend template
        legendTemplate: '<div class="tc-chart-js-legend"><% for (var i=0; i<datasets.length; i+=3){%><span style="background-color:<%=datasets[i].fillColor%>"></span><%if(i<datasets.length){%><%=datasets[i].label%><%}%><%if(i+1<datasets.length){%><span style="background-color:<%=datasets[i+1].fillColor%>"></span> &nbsp; <%=datasets[i+1].label%><%}%><%if(i+2<datasets.length){%><span style="background-color:<%=datasets[i+2].fillColor%>"></span><%=datasets[i+2].label%><%}%><%}%></div>'
        //legendTemplate: '<ul class="tc-chart-js-legend"><% for (var i=0; i<datasets.length; i++){%><li><span style="background-color:<%=datasets[i].fillColor%>"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>'
    };

    // Chart.js Data
    $scope.ProjectChartData = [
      {
          value: 300,
          color: '#d9534f',
          highlight: '#F7464A',
          label: 'Sitecore'
      },    
       {
           value: 50,
           color: '#f8c705',
           highlight: '#FFC870',
           label: 'Core .NET'
       },
      {
          value: 50,
          color: '#00b0f0',
          highlight: '#46BFBD',
          label: 'Hybris'
      },
      {
          value: 100,
          color: '#FDB45C',
          highlight: '#FFC870',
          label: 'Adobe CQ'
      },
      {
          value: 300,
          color: '#fe6f54',
          highlight: '#E32400',
          label: 'Core Java'
      },
      {
          value: 200,
          color: '#7ec351',
          highlight: 'lightgreen',
          label: 'QA'
      }
    ];

    // Chart.js Options
    $scope.ProjectChartOptions = {

        // Sets the chart to be responsive
        responsive: true,

        //Boolean - Whether we should show a stroke on each segment
        segmentShowStroke: true,

        //String - The colour of each segment stroke
        segmentStrokeColor: '#fff',

        //Number - The width of each segment stroke
        segmentStrokeWidth: 2,

        //Number - The percentage of the chart that we cut out of the middle
        percentageInnerCutout: 50, // This is 0 for Pie charts

        //Number - Amount of animation steps
        animationSteps: 100,

        //String - Animation easing effect
        animationEasing: 'easeOutQuint',

        //Boolean - Whether we animate the rotation of the Doughnut
        animateRotate: true,

        //Boolean - Whether we animate scaling the Doughnut from the centre
        animateScale: false,

        //String - A legend template
        legendTemplate: '<ul class="tc-chart-js-legend"><% for (var i=0; i<segments.length; i++){%><li><span style="background-color:<%=segments[i].fillColor%>"></span><%if(segments[i].label){%><%=segments[i].label%><%}%></li><%}%></ul>'

    };


    //End


    $scope.UpdateSoldProposedChart = function () {
        $http({
            method: 'GET',
            url: 'api/Dashboard/GetSoldProposedChartData'
        }).
      success(function (data, status, headers, config) {
          if (data != null) {
              debugger;
              $scope.SoldProposedChartLabels = JSON.parse(data[0]);
              $scope.SoldProjectsChartData = JSON.parse(data[1]);
              $scope.ProposedProjectsChartData = JSON.parse(data[2]);

              $scope.ODYoYData.labels = $scope.SoldProposedChartLabels;
              $scope.ODYoYData.datasets[0].data = $scope.SoldProjectsChartData;
              $scope.ODYoYData.datasets[1].data = $scope.ProposedProjectsChartData;
          }
      }).
      error(function (data, status, headers, config) {
          // called asynchronously if an error occurs
          // or server returns response with an error status.
          $scope.AllResources = -1;
      });
    };

    $scope.UpdateSoldProposedChart();

    // Chart.js Data
    $scope.ODYoYData = {
        labels: $scope.SoldProposedChartLabels,
        datasets: [
          {
              label: 'Sold resource needs',
              fillColor: 'rgba(320,220,220,0.2)',
              strokeColor: 'rgba(220,220,220,1)',
              pointColor: 'rgba(220,220,220,1)',
              pointStrokeColor: '#fff',
              pointHighlightFill: '#fff',
              pointHighlightStroke: 'rgba(220,220,220,1)',
              data: $scope.SoldProjectsChartData
          },
          {
              label: 'Proposed resource needs',
              fillColor: 'rgba(151,187,205,0.2)',
              strokeColor: 'rgba(151,187,205,1)',
              pointColor: 'rgba(151,187,205,1)',
              pointStrokeColor: '#fff',
              pointHighlightFill: '#fff',
              pointHighlightStroke: 'rgba(151,187,205,1)',
              data: $scope.ProposedProjectsChartData
          }
        ]
    };

    // Chart.js Options
    $scope.ODYoYOptions = {

        // Sets the chart to be responsive
        responsive: true,

        ///Boolean - Whether grid lines are shown across the chart
        scaleShowGridLines: true,

        //String - Colour of the grid lines
        scaleGridLineColor: "rgba(0,0,0,.05)",

        //Number - Width of the grid lines
        scaleGridLineWidth: 1,

        //Boolean - Whether the line is curved between points
        bezierCurve: true,

        //Number - Tension of the bezier curve between points
        bezierCurveTension: 0.4,

        //Boolean - Whether to show a dot for each point
        pointDot: true,

        //Number - Radius of each point dot in pixels
        pointDotRadius: 4,

        //Number - Pixel width of point dot stroke
        pointDotStrokeWidth: 1,

        //Number - amount extra to add to the radius to cater for hit detection outside the drawn point
        pointHitDetectionRadius: 20,

        //Boolean - Whether to show a stroke for datasets
        datasetStroke: true,

        //Number - Pixel width of dataset stroke
        datasetStrokeWidth: 2,

        //Boolean - Whether to fill the dataset with a colour
        datasetFill: true,

        // Function - on animation progress
        onAnimationProgress: function () { },

        // Function - on animation complete
        onAnimationComplete: function () { },

        //String - A legend template
        legendTemplate: '<ul class="tc-chart-js-legend"><% for (var i=0; i<datasets.length; i++){%><li><span style="background-color:<%=datasets[i].strokeColor%>"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>'
    };



}]);

AUDashboardApp.controller('InvoicesController', ['$scope', '$filter', '$http', 'FileUploader', function ($scope, $filter, $http, FileUploader) {
    var STORAGE_ID = 'Invoices';
    $scope.EditMode = "false";
    $scope.currentPage = 1;
    $scope.pageSize = 5;

    var InvoiceDetails = $scope.InvoiceDetails = [];

    $scope.getInvoices = function () {
        //debugger;
        $http({
            method: 'GET',
            url: 'api/Dashboard/GetReferenceData?storageId=' + STORAGE_ID
        }).
        success(function (data, status, headers, config) {

            if (data != 'null') {
                InvoiceDetails = $scope.InvoiceDetails = JSON.parse(JSON.parse(data));
                //$scope.$parent.ActiveProjects = InvoiceDetails.length;
            }
        }).
        error(function (data, status, headers, config) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.

        });

    };

    $scope.EditInvoice = function (invoice, index) {
        invoice.index = index;
        $scope.InvoiceEntity = jQuery.extend(true, {}, invoice); // deep copy
        $scope.OriginalInvoice = jQuery.extend(true, {}, invoice); // deep copy
    }

    $scope.ShowReceived = function (showReceived) {
        if (showReceived == "Received" || showReceived == "Yes") {
            return true;
        }
        else
            return false;
    }

    $scope.ShowPending = function (showPending) {
        if (showPending == "Pending" || showPending == "No") {
            return true;
        }
        else
            return false;
    }

    $scope.getInvoices();

    $scope.setInvoices = function (InvoiceDetails) {
        var referenceData = new Object();
        referenceData.storageId = STORAGE_ID;
        referenceData.storageData = JSON.stringify(InvoiceDetails);
        $http({
            url: 'api/Dashboard/SetReferenceData',
            method: "POST",
            data: JSON.stringify(JSON.stringify(referenceData))
        })
            .then(function (response) {
                $scope.getInvoices();
            },
                function (response) { // optional
                }
            );
    };

    $scope.$watch('InvoiceDetails', function (newValue, oldValue) {
        if (newValue !== oldValue) { // This prevents unneeded calls to the local storage
            $scope.setInvoices(InvoiceDetails);
        }
    }, true);

    $scope.AddInvoice = function (InvoiceEntity) {
        //debugger;
        if (InvoiceEntity.index >= 0) {
            InvoiceDetails[InvoiceEntity.index] = InvoiceEntity;
        } else {
            InvoiceDetails.push(InvoiceEntity);
        }

        $scope.InvoiceDetails = InvoiceDetails;
        $scope.InvoiceEntity = '';
    };


    //File upload functionality
    var uploader = $scope.uploader = new FileUploader({
        url: 'api/Dashboard/UploadInvoices',
        autoUpload: true
        //removeAfterUpload: true
    });

    uploader.onCompleteItem = function (fileItem, response, status, headers) {
        $scope.getInvoices();
    };

    // FILTERS

    //uploader.filters.push({
    //    name: 'customFilter',
    //    fn: function(item /*{File|FileLikeObject}*/, options) {
    //        return this.queue.length < 10;
    //    }
    //});




    // Chart.js Data
    $scope.InvoiceChartData = {
        labels: ['Payment Pending', 'Closed', 'ATB Approval Pending', 'Processed'],
        datasets: [
          {
              label: 'Invoices by Status',
              fillColor: 'rgba(220,220,220,0.5)',
              strokeColor: 'rgba(220,220,220,0.8)',
              highlightFill: 'rgba(220,220,220,0.75)',
              highlightStroke: 'rgba(220,220,220,1)',
              data: [6, 5, 12, 23]
          }
        ]
    };

    // Chart.js Options
    $scope.InvoiceChartOptions = {

        // Sets the chart to be responsive
        responsive: true,

        //Boolean - Whether the scale should start at zero, or an order of magnitude down from the lowest value
        scaleBeginAtZero: true,

        //Boolean - Whether grid lines are shown across the chart
        scaleShowGridLines: true,

        //String - Colour of the grid lines
        scaleGridLineColor: "rgba(0,0,0,.05)",

        //Number - Width of the grid lines
        scaleGridLineWidth: 1,

        //Boolean - If there is a stroke on each bar
        barShowStroke: true,

        //Number - Pixel width of the bar stroke
        barStrokeWidth: 2,

        //Number - Spacing between each of the X value sets
        barValueSpacing: 15,

        //Number - Spacing between data sets within X values
        barDatasetSpacing: 50,

        //String - A legend template
        legendTemplate: '<ul class="tc-chart-js-legend"><% for (var i=0; i<datasets.length; i++){%><li><span style="background-color:<%=datasets[i].fillColor%>"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>'
    };

}]);