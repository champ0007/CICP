/// <reference path="../angular.min.js" />
var AUDashboardApp = angular.module("AUDashboardApp", []);

AUDashboardApp.controller('DashboardController', ['$scope', function ($scope) {

    $scope.ActiveProjects = 9;
    $scope.PendingInvoices = 3;
    $scope.ActiveResources = 60;
    $scope.OpenActionItems = 1;

    var FakeNotifications = [
        { message: 'EDC-AU meet', eventdate: '11-Sep', type: 'fa fa-calendar fa-fw' },
        { message: '4 users added', eventdate: '44 mins ago', type: 'fa fa-comment fa-fw' },
        { message: '5 users added', eventdate: '45 mins ago', type: 'fa fa-twitter fa-fw' },
        { message: '7 tasks added', eventdate: '47 mins ago', type: 'fa fa-tasks fa-fw' },
        { message: '7 tasks added', eventdate: '47 mins ago', type: 'fa fa-tasks fa-fw' },
        { message: '7 tasks added', eventdate: '47 mins ago', type: 'fa fa-tasks fa-fw' },
    ];

    $scope.notifications = FakeNotifications;
    
   

    //  $scope.ShowDetails = false;

var ProjectDetails = [
        { Client: 'AMP', ProjectName: 'AMP', Stage: 'Active', Probability: 'Medium', Technology: 'CQ', Startdate: '4-Jul-2014' },
        { Client: 'Telstra', ProjectName: 'Telstra', Stage: 'Active', Probability: 'Medium', Technology: 'CQ', Startdate: '4-Jul-2014' },
        { Client: 'Caltex', ProjectName: 'Caltex', Stage: 'Sold', Probability: 'Medium', Technology: 'Sitecore', Startdate: '4-Jul-2014' },
        { Client: 'VicRoads', ProjectName: 'VicRoads', Stage: 'Sold', Probability: 'Medium', Technology: 'Sitecore', Startdate: '4-Jul-2014' },
        { Client: 'CPA ITB', ProjectName: 'CPA ITB', Stage: 'Completed', Probability: 'Medium', Technology: 'Sitecore', Startdate: '4-Jul-2014' },
        { Client: 'Sydney Trains', ProjectName: 'SydneyTrains', Stage: 'Lost', Probability: 'Medium', Technology: 'CQ', Startdate: '4-Jul-2014' }
];

$scope.ActiveProjectDetails = ProjectDetails;

var ActionItems = [
        { ShortDesc: 'TafeNSW', Owner: 'Adam', DueDate: '20-Aug', Status: 'Closed'},
        { ShortDesc: 'Testing CoE', Owner: 'Shakeel', DueDate: '20-Aug', Status: 'Open' },
        { ShortDesc: 'Upcoming Holidays', Owner: 'Tushar', DueDate: '20-Aug', Status: 'Open' },
        { ShortDesc: 'India Trip', Owner: 'Adam', DueDate: '20-Aug', Status: 'Open' },
        { ShortDesc: 'India Trip', Owner: 'Adam', DueDate: '20-Aug', Status: 'Open' },
        { ShortDesc: 'India Trip', Owner: 'Adam', DueDate: '20-Aug', Status: 'Open' }
];

$scope.AllActionItems = ActionItems;


}]);