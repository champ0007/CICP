/// <reference path="../angular.min.js" />
/// <reference path="../jquery-1.10.2.intellisense.js" />
var todos;

var CICPApp = angular.module("CICPApp", ["ngRoute", "tc.chartjs", "angularFileUpload", "angularUtils.directives.dirPagination", "angular-chartist"]);

CICPApp.config(['$routeProvider',
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

CICPApp.controller('DashboardController', ['$scope', '$http','$location', 'FileUploader', function ($scope, $http, $location, FileUploader) {

    $scope.UserIdentity = null;
    $scope.UserPassword = null;
    $scope.UserValidated = false;
    $scope.LoginMessage = null;

    $scope.ValidateUserLogin = function () {
        $http({
            method: 'GET',
            url: 'api/Dashboard/GetAuthentication?authToken=' + $scope.UserIdentity + "-" + $scope.UserPassword
        }).
        success(function (data, status, headers, config) {
            var isValidUser = JSON.parse(data);
            $scope.UserValidated = JSON.parse(data)
            if (isValidUser == "false")
                $scope.LoginMessage = "* user name/password not correct";
            else {
                $scope.LoginMessage = null;
                $http({
                    method: 'GET',
                    url: 'api/Dashboard/GetDashboardCounts?authToken=' + $scope.UserIdentity + "-" + $scope.UserPassword
                }).
                success(function (data, status, headers, config) {
                    $scope.PendingInvoices = JSON.parse(JSON.parse(data))[0];
                    $scope.ActiveProjects = JSON.parse(JSON.parse(data))[1];
                    $scope.OpenActionItems = JSON.parse(JSON.parse(data))[2];
                    $scope.ActiveResources = JSON.parse(JSON.parse(data))[3];

                }).
                error(function (data, status, headers, config) {
                    // called asynchronously if an error occurs
                    // or server returns response with an error status.
                    $scope.ActiveProjects = -1;
                    $scope.PendingInvoices = -1;
                    $scope.ActiveResources = -1;
                    $scope.OpenActionItems = -1;
                });


                //$scope.PendingInvoices = 9;
                //$scope.ActiveResources = 32;
                //$scope.OpenActionItems = 5;

                var ProjectEntity;

                var FakeNotifications = [{
                    message: 'USI - Canada Monthly Meet',
                    eventdate: '8-May',
                    type: 'fa fa-calendar fa-fw'
                }, {
                    message: 'Big Client Project - Kick Off',
                    eventdate: '1-Jun',
                    type: 'fa fa-calendar fa-fw'
                }];

                $scope.notifications = FakeNotifications;
                debugger;
                $location.path('/Dashboard');
                

                $scope.ProjectChartData = [];

                $scope.UpdateProjChart = function () {
                    $http({
                        method: 'GET',
                        url: 'api/Dashboard/GetProjChartData?authToken=' + $scope.UserIdentity + "-" + $scope.UserPassword
                    }).
                  success(function (data, status, headers, config) {
                      if (data != null) {
                          debugger;
                          $scope.ProjectChartData = [];
                          var ProjectChartDataList = JSON.parse(data[0]);
                          for (i = 0; i <= ProjectChartDataList.length; i++) {
                              var dataItem = new Object();
                              dataItem.value = ProjectChartDataList[i].Count;
                              dataItem.label = ProjectChartDataList[i].ProjectStatus;
                              dataItem.color = colors[i];
                              dataItem.highlight = highlights[i];
                              $scope.ProjectChartData.push(dataItem);
                          }
                          //$scope.revenueChartPrevData = JSON.parse(data[1]);
                          //$scope.revenueChartCurrData = JSON.parse(data[0]);
                          //$scope.skillChartData.datasets[0].data = $scope.revenueChartCurrData;
                          //$scope.skillChartData.datasets[1].data = $scope.revenueChartPrevData;
                          //$scope.skillChartData.datasets[0].label = JSON.parse(data[2]);
                          //$scope.skillChartData.datasets[1].label = JSON.parse(data[3]);
                      }
                  }).
                  error(function (data, status, headers, config) {
                      // called asynchronously if an error occurs
                      // or server returns response with an error status.
                      $scope.AllResources = -1;
                  });
                };



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
            }
        }).
        error(function (data, status, headers, config) {
            $scope.UserValidated = false;
            $scope.LoginMessage = "* user name/password not correct";
        });
    }
    $scope.UserLogout = function () {
        $scope.UserIdentity = null;
        $scope.UserPassword = null;
        $scope.UserValidated = false;
        $scope.LoginMessage = null;
    }

    //File upload functionality
    var cicpmasterdatauploader = $scope.cicpmasterdatauploader = new FileUploader({
        url: 'api/Dashboard/UploadCICPMasterData',
        autoUpload: true,
        removeAfterUpload: true
    });

    ClearFileUpload = function () {
        debugger;
        $scope.UploadSuccessMessage = "";
        $('#cicpfileuploader').val() = "";
    }

    cicpmasterdatauploader.onCompleteItem = function (fileItem, response, status, headers) {
        $scope.UploadSuccessMessage = "File uploaded successfully !";
    };




}]);

CICPApp.controller('ActionItemsController', ['$scope', '$filter', '$http', function ($scope, $filter, $http) {

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
            url: 'api/Dashboard/GetReferenceData?storageId=' + STORAGE_ID + '&authToken=' + $scope.UserIdentity + "-" + $scope.UserPassword
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
        ////debugger;
        var referenceData = new Object();
        referenceData.storageId = STORAGE_ID;
        referenceData.storageData = JSON.stringify(todos);
        referenceData.authToken = $scope.UserIdentity + "-" + $scope.UserPassword;
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
        ////debugger;
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

CICPApp.controller('ActiveProjectsController', ['$scope', '$filter', '$http', function ($scope, $filter, $http) {
    var STORAGE_ID = 'Projects';
    $scope.EditMode = "false";
    $scope.ActiveFilterSet;
    $scope.currentProjectPage = 1;
    $scope.pageSize = 10;
    $scope.ProjectPerPage = 10;

    var ProjectDetails = $scope.ActiveProjectDetails = [];

    $scope.getProjects = function () {
        ////debugger;
        $http({
            method: 'GET',
            url: 'api/Dashboard/GetReferenceData?storageId=' + STORAGE_ID + '&authToken=' + $scope.UserIdentity + "-" + $scope.UserPassword,
        }).
        success(function (data, status, headers, config) {

            if (data != 'null') {
                ProjectDetails = $scope.ActiveProjectDetails = JSON.parse(JSON.parse(data));
                $scope.OriginalProjectDetails = JSON.parse(JSON.parse(data));
                $scope.$parent.ActiveProjects = $filter('filter')(ProjectDetails, { Stage: "Sold" }).length;
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
  
    $scope.AddProject = function (ProjectEntity, action) {
        var projectRequest = new Object();
        projectRequest.Projects = ProjectDetails;
        projectRequest.Project = ProjectEntity;
        projectRequest.action = action;
        projectRequest.authToken = $scope.UserIdentity + "-" + $scope.UserPassword;

        $http({
            method: 'POST',
            url: 'api/Dashboard/UpsertProject',
            data: projectRequest
        }).
       success(function (data, status, headers, config) {
           //success logic
           if (data != 'null') {
               ProjectDetails = $scope.ActiveProjectDetails = JSON.parse(JSON.parse(data));
               $scope.OriginalProjectDetails = JSON.parse(JSON.parse(data));
               $scope.$parent.ActiveProjects = $filter('filter')(ProjectDetails, { Stage: "Sold" }).length;
               $scope.UpdateChart();

               $scope.ActiveProjectDetails = ProjectDetails;
               $scope.ProjectEntity = '';
           }
       }).
       error(function (data, status, headers, config) {
           //error handling logic
       });

    };

    $scope.OpenAddProject = function () {
        $scope.ProjectEntity = '';
    };


    //Start Key Updates
    var keyUpdates = $scope.keyUpdates = [];

    $scope.getKeyUpdates = function () {

        $http({
            method: 'GET',
            url: 'api/Dashboard/GetReferenceData?storageId=KeyUpdates&authToken=' + $scope.UserIdentity + "-" + $scope.UserPassword
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
        referenceData.authToken = $scope.UserIdentity + "-" + $scope.UserPassword;
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

    $scope.DeleteKeyUpdate = function (keyUpdate) {
        keyUpdates.splice(keyUpdate.index, 1);
    };

    //End Key updates




    $scope.UpdateChart = function () {
        $http({
            method: 'GET',
            url: 'api/Dashboard/GetProjectChartData?authToken=' + $scope.UserIdentity + "-" + $scope.UserPassword
        }).
      success(function (data, status, headers, config) {
          if (data != null) {
              //debugger;
              $scope.ProjectChartData = [];
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

CICPApp.controller('ActiveResourcesController', ['$scope', '$http', 'FileUploader', function ($scope, $http, FileUploader) {
    var STORAGE_ID = 'Resources';
    $scope.EditMode = "false";
    $scope.currentPage = 1;
    $scope.currentGSSPage = 1;
    $scope.pageSize = 10;

    var resources = $scope.AllResources = [];

    $scope.getResources = function () {
        $http({
            method: 'GET',
            url: 'api/Dashboard/GetReferenceData?storageId=' + STORAGE_ID + '&authToken=' + $scope.UserIdentity + "-" + $scope.UserPassword
        }).
        success(function (data, status, headers, config) {
            if (data != null) {
                resources = $scope.AllResources = JSON.parse(JSON.parse(data));
                $scope.UpdateChart();
            }
        }).
        error(function (data, status, headers, config) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.
            $scope.AllResources = -1;
        });

        $http({
            method: 'GET',
            url: 'api/Dashboard/GetReferenceData?storageId=GSSResources&authToken=' + $scope.UserIdentity + "-" + $scope.UserPassword

        }).
       success(function (data, status, headers, config) {
           if (data != null) {
               $scope.AllGSSResources = JSON.parse(JSON.parse(data));
               $scope.$parent.ActiveResources = $scope.AllGSSResources.length;
               $scope.UpdateChart();
           }
       }).
       error(function (data, status, headers, config) {
           // called asynchronously if an error occurs
           // or server returns response with an error status.
           $scope.AllResources = -1;
       });

    };

    //$scope.setResources = function (resourcesToBeSaved) {
    //    var referenceData = new Object();
    //    referenceData.storageId = STORAGE_ID;
    //    referenceData.storageData = JSON.stringify(resourcesToBeSaved);
    //    $http({
    //        url: 'api/Dashboard/SetReferenceData',
    //        method: "POST",
    //        data: JSON.stringify(JSON.stringify(referenceData))
    //    })
    //        .then(function (response) {
    //            $scope.getResources();
    //        },
    //            function (response) { // optional
    //            }
    //        );
    //};

    //$scope.$watch('AllResources', function (newValue, oldValue) {
    //    if (newValue !== oldValue) { // This prevents unneeded calls to the local storage
    //        $scope.setResources(resources);
    //    }
    //}, true);

    $scope.EditResource = function (resource, index) {
        //debugger;
        resource.index = index;
        $scope.EditMode = "true";
        //Shallow Copy - $scope.ResourceEntity = resource;
        $scope.ResourceEntity = jQuery.extend(true, {}, resource); // deep copy
        $scope.OriginalResourceEntity = jQuery.extend(true, {}, resource); // deep copy
    };

    $scope.addResource = function (resource, action) {
        var resourceRequest = new Object();
        resourceRequest.Resources = resources;
        resourceRequest.Resource = resource;
        resourceRequest.Action = action;
        resourceRequest.authToken = $scope.UserIdentity + "-" + $scope.UserPassword;

        $http({
            method: 'POST',
            url: 'api/Dashboard/UpsertResource',
            data: resourceRequest
        }).
       success(function (data, status, headers, config) {
           //success logic
           if (data != null) {
               resources = $scope.AllResources = JSON.parse(JSON.parse(data));

               $scope.UpdateChart();

               $scope.AllResources = resources;
               $scope.ResourceEntity = '';

           }
       }).
       error(function (data, status, headers, config) {
           //error handling logic
       });

    };

    var chartLabels, chartData;

    $scope.UpdateChart = function () {
        $http({
            method: 'GET',
            url: 'api/Dashboard/GetResourceChartData?authToken=' + $scope.UserIdentity + "-" + $scope.UserPassword
        }).
      success(function (data, status, headers, config) {
          if (data != null) {
              chartLabels = JSON.parse(data[0]);
              chartData = JSON.parse(data[1]);
          }
      }).
      error(function (data, status, headers, config) {
          // called asynchronously if an error occurs
          // or server returns response with an error status.
          $scope.AllGSSResources = -1;
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

    $scope.ShowCurrent = true;

    $scope.DisplayContent = function (activeTab) {
        if (activeTab == 'Unallocated') {
            $scope.ShowCurrent = false;
        }
        else {
            $scope.ShowCurrent = true;
        }
        $scope.searchResource = '';
    };

    $scope.OpenAddResource = function () {
        $scope.ResourceEntity = '';
        $scope.IsHidden = true;
    };

    $scope.getResources();

}]);

CICPApp.controller('NewActionItemsController', ['$scope', '$filter', '$http', function ($scope, $filter, $http) {
    var STORAGE_ID = 'NewToDoItems'; // To be passed
    //$scope.EditMode = "false";
    $scope.currentPage = 1;
    $scope.currentToDoPage = 1;
    $scope.pageSize = 5;
    $scope.ToDoPerPage = 5;

    var NewToDos = $scope.NewToDos = [];

    $scope.getNewToDos = function () {
        $http({
            method: 'GET',
            url: 'api/Dashboard/GetReferenceData?storageId=' + STORAGE_ID + '&authToken=' + $scope.UserIdentity + "-" + $scope.UserPassword
        }).
        success(function (data, status, headers, config) {

            if (data != 'null') {
                NewToDos = $scope.NewToDos = JSON.parse(JSON.parse(data));
                $scope.$parent.OpenActionItems = $filter('filter')(NewToDos, { Status: "Open" }).length;
                $scope.UpdateChart($filter('filter')(NewToDos, { Status: "Open" }).length, $filter('filter')(NewToDos, { Status: "Closed" }).length, $filter('filter')(NewToDos, { Status: "Pending" }).length);
            }
        }).
        error(function (data, status, headers, config) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.

        });

    };

    $scope.EditToDoItem = function (ToDoItem, index) {
        debugger;
        ToDoItem.index = index;
        $scope.ToDoItem = jQuery.extend(true, {}, ToDoItem); // deep copy
        $scope.OriginalToDoItem = jQuery.extend(true, {}, ToDoItem); // deep copy
    }

    $scope.setToDoItems = function (NewToDos) {
        var referenceData = new Object();
        referenceData.storageId = STORAGE_ID;
        referenceData.storageData = JSON.stringify(NewToDos);
        referenceData.authToken = $scope.UserIdentity + "-" + $scope.UserPassword;
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
        debugger;

        if (ToDoItem.index >= 0) {
            NewToDos[ToDoItem.index] = ToDoItem;
        } else {
            ToDoItem.Status = 'Open';
            NewToDos.push(ToDoItem);
        }

        $scope.NewToDos = NewToDos;
        $scope.NewToDoItem = '';
    };

    $scope.DeleteToDoItem = function (ToDoItem) {
        for (var i = 0; i < NewToDos.length; i++) {
            if (NewToDos[i].Desc === ToDoItem.Desc && NewToDos[i].AssignedTo === ToDoItem.AssignedTo && NewToDos[i].Status === ToDoItem.Status && NewToDos[i].Comments === ToDoItem.Comments) {
                NewToDos.splice(i, 1);
                break;
            }
        }

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
            animationEasing: 'easeOutQuint',

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

CICPApp.controller('OperationsController', ['$scope', '$http', function ($scope, $http) {

    //Start Key Updates
    var keyUpdates = $scope.keyUpdates = [];

    $scope.getKeyUpdates = function () {
        ////
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

    $scope.UpdateActualUSIEngmntByIMLead = function () {
        $http({
            method: 'GET',
            url: 'api/Dashboard/GetActualUSIEngmntByIMLead?authToken=' + $scope.UserIdentity + "-" + $scope.UserPassword
        }).
      success(function (data, status, headers, config) {
          if (data != null) {
              //
              debugger;

              $scope.USIEngmntByIMLead = JSON.parse(data[1]);

              $scope.USIEngmntByIMLeadData.datasets[0].data = $scope.USIEngmntByIMLead;
              $scope.USIEngmntByIMLeadData.labels = JSON.parse(data[0]);

              $scope.barData.labels = JSON.parse(data[0]);
              $scope.barData.series = JSON.parse(data[1]);
          }
      }).
      error(function (data, status, headers, config) {
          // called asynchronously if an error occurs
          // or server returns response with an error status.

      });
    };

    $scope.USIEngmntByIMLeadData = {
        labels: ['Pineda,Raymond', 'Finklestein,Perry', 'D-Ercole,Nat', 'IM House'],
        datasets: [
          {
              label: 'IM Lead',
              fillColor: '#0cc09f',
              strokeColor: 'rgba(220,220,220,0.8)',
              highlightFill: '#0aac8e',
              highlightStroke: 'rgba(220,220,220,1)',
              data: $scope.USIEngmntByIMLead
          }
        ]
    };






    $scope.USIEngmntByIMLeadDataOptions = {

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
        barValueSpacing: 50,

        //Number - Spacing between data sets within X values
        barDatasetSpacing: 1,

        //String - A legend template
        legendTemplate: '<div class="tc-chart-js-legend"><% for (var i=0; i<datasets.length; i+=3){%><span style="background-color:<%=datasets[i].fillColor%>"></span><%if(i<datasets.length){%><%=datasets[i].label%><%}%><%if(i+1<datasets.length){%><span style="background-color:<%=datasets[i+1].fillColor%>"></span> &nbsp; <%=datasets[i+1].label%><%}%><%if(i+2<datasets.length){%><span style="background-color:<%=datasets[i+2].fillColor%>"></span><%=datasets[i+2].label%><%}%><%}%></div>'
    };

    $scope.UpdateQualifiedPursuitsByCustomer = function () {
        $http({
            method: 'GET',
            url: 'api/Dashboard/GetQualifiedPursuitsByCustomer?authToken=' + $scope.UserIdentity + "-" + $scope.UserPassword
        }).
      success(function (data, status, headers, config) {
          if (data != null) {
              //

              $scope.QualifiedPursuitsByCustomer = JSON.parse(data[1]);
              $scope.QualifiedPursuitsByCustomerData.datasets[0].data = $scope.QualifiedPursuitsByCustomer;
              $scope.QualifiedPursuitsByCustomerData.labels = JSON.parse(data[0]);

              $scope.PBCbarData.labels = JSON.parse(data[0]);
              $scope.PBCbarData.series[0] = JSON.parse(data[1]);

          }
      }).
      error(function (data, status, headers, config) {
          // called asynchronously if an error occurs
          // or server returns response with an error status.

      });
    };

    $scope.UpdateQualifiedPursuitsByCustomer();


    $scope.QualifiedPursuitsByCustomerData = {
        labels: ['CC', 'FRHII', 'HOOL', 'CBB', 'UA', 'HC', 'ICBC', 'HEI', 'SEI', '3SH'],
        datasets: [
          {
              label: 'Account',
              fillColor: '#0cc09f',
              strokeColor: 'rgba(220,220,220,0.8)',
              highlightFill: '#0aac8e',
              highlightStroke: 'rgba(220,220,220,1)',
              data: $scope.QualifiedPursuitsByCustomer
          }
        ]
    };

    $scope.QualifiedPursuitsByCustomerDataDataOptions = {

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
        barValueSpacing: 30,

        //Number - Spacing between data sets within X values
        barDatasetSpacing: 1,

        //String - A legend template
        legendTemplate: '<div class="tc-chart-js-legend"><% for (var i=0; i<datasets.length; i+=3){%><span style="background-color:<%=datasets[i].fillColor%>"></span><%if(i<datasets.length){%><%=datasets[i].label%><%}%><%if(i+1<datasets.length){%><span style="background-color:<%=datasets[i+1].fillColor%>"></span> &nbsp; <%=datasets[i+1].label%><%}%><%if(i+2<datasets.length){%><span style="background-color:<%=datasets[i+2].fillColor%>"></span><%=datasets[i+2].label%><%}%><%}%></div>'
    };

    $scope.UpdateActualUSIEngmntByClient = function () {
        $http({
            method: 'GET',
            url: 'api/Dashboard/GetActualUSIEngmntByClient?authToken=' + $scope.UserIdentity + "-" + $scope.UserPassword
        }).
      success(function (data, status, headers, config) {
          if (data != null) {
              //

              $scope.USIEngmntByClient = JSON.parse(data[1]);

              $scope.USIEngmntByClientData.datasets[0].data = $scope.USIEngmntByClient;
              $scope.USIEngmntByClientData.labels = JSON.parse(data[0]);

              $scope.EBCbarData.labels = JSON.parse(data[0]);
              $scope.EBCbarData.series[0] = JSON.parse(data[1]);


          }
      }).
      error(function (data, status, headers, config) {
          // called asynchronously if an error occurs
          // or server returns response with an error status.

      });
    };

    $scope.UpdateActualUSIEngmntByClient();


    $scope.USIEngmntByClientData = {
        labels: ['FRHII', 'ON-MCYS', 'CSI', 'BGC', 'IBMCL.', 'SCAA&T', 'CIBC', 'TDBG', 'CBB', 'CII'],
        datasets: [
          {
              label: 'Service - Client Name',
              fillColor: '#0cc09f',
              strokeColor: 'rgba(220,220,220,0.8)',
              highlightFill: '#0aac8e',
              highlightStroke: 'rgba(220,220,220,1)',
              data: $scope.USIEngmntByClient
          }
        ]
    };

    $scope.USIEngmntByClientDataOptions = {

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
        barValueSpacing: 30,

        //Number - Spacing between data sets within X values
        barDatasetSpacing: 1,

        //String - A legend template
        legendTemplate: '<div class="tc-chart-js-legend"><% for (var i=0; i<datasets.length; i+=3){%><span style="background-color:<%=datasets[i].fillColor%>"></span><%if(i<datasets.length){%><%=datasets[i].label%><%}%><%if(i+1<datasets.length){%><span style="background-color:<%=datasets[i+1].fillColor%>"></span> &nbsp; <%=datasets[i+1].label%><%}%><%if(i+2<datasets.length){%><span style="background-color:<%=datasets[i+2].fillColor%>"></span><%=datasets[i+2].label%><%}%><%}%></div>'
    };




    $scope.UpdateProjectDistributionChart = function () {
        $http({
            method: 'GET',
            url: 'api/Dashboard/GetProjectDistributionChartData'
        }).
      success(function (data, status, headers, config) {
          if (data != null) {
              //

              $scope.ProjectDistLabels = JSON.parse(data[0]);
              $scope.ProjectDistData = JSON.parse(data[1]);

              $scope.ProjectDistributionData.labels = JSON.parse(data[0]);
              $scope.ProjectDistributionData.datasets[0].data = JSON.parse(data[1]);

          }
      }).
      error(function (data, status, headers, config) {
          // called asynchronously if an error occurs
          // or server returns response with an error status.

      });
    };

    //$scope.UpdateProjectDistributionChart();


    $scope.ProjectDistributionData = {
        labels: $scope.ProjectDistLabels,
        datasets: [
          {
              label: 'No. of projects by month',
              fillColor: '#0cc09f',
              strokeColor: 'rgba(220,220,220,0.8)',
              highlightFill: '#0aac8e',
              highlightStroke: 'rgba(220,220,220,1)',
              data: $scope.ProjectDistData
          }
        ]
    };

    $scope.ProjectDistributionOptions = {

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
    };

    //Resource Deployment

    $scope.UpdateResourceDeploymentChart = function () {
        $http({
            method: 'GET',
            url: 'api/Dashboard/GetResourceDeploymentChartData'
        }).
      success(function (data, status, headers, config) {
          if (data != null) {
              //

              $scope.ResourceMonthLabels = JSON.parse(data[0]);
              $scope.ResourceMonthData = JSON.parse(data[1]);
              $scope.ResourceDeploymentData.labels = JSON.parse(data[0]);
              $scope.ResourceDeploymentData.datasets[0].data = JSON.parse(data[1]);

          }
      }).
      error(function (data, status, headers, config) {
          // called asynchronously if an error occurs
          // or server returns response with an error status.

      });
    };

    //$scope.UpdateResourceDeploymentChart();



    $scope.ResourceDeploymentData = {
        labels: $scope.ResourceMonthLabels,
        datasets: [
          {
              label: 'No. of resources by month',
              fillColor: '#0cc09f',//rgba(69,201,102,0.75)',
              strokeColor: 'rgba(220,220,220,0.8)',
              highlightFill: '#0aac8e',
              highlightStroke: 'rgba(220,220,220,1)',
              data: $scope.ResourceMonthData
          }
        ]
    };

    $scope.ResourceDeploymentOptions = {
        // Sets the chart to be responsive
        responsive: true,

        //Boolean - Whether the scale should start at zero, or an order of magnitude down from the lowest value
        scaleBeginAtZero: true,

        //Boolean - Whether grid lines are shown across the chart
        scaleShowGridLines: false,

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

    $scope.UpdateActualVsBudgetChart = function () {
        $http({
            method: 'GET',
            url: 'api/Dashboard/GetActualVsBudgetHoursChartData?authToken=' + $scope.UserIdentity + "-" + $scope.UserPassword
        }).
      success(function (data, status, headers, config) {
          if (data != null) {

              $scope.usiHoursActualData = JSON.parse(data[0]);
              $scope.usiHoursBudgetData = JSON.parse(data[1]);

              $scope.actualVsBudgetData.datasets[0].data = $scope.usiHoursActualData;
              $scope.actualVsBudgetData.datasets[1].data = $scope.usiHoursBudgetData;
              $scope.actualVsBudgetData.datasets[0].label = JSON.parse(data[2]);
              $scope.actualVsBudgetData.datasets[1].label = JSON.parse(data[3]);
          }
      }).
      error(function (data, status, headers, config) {
          // called asynchronously if an error occurs
          // or server returns response with an error status.
          $scope.AllResources = -1;
      });
    };

    // Chart.js Data
    $scope.actualVsBudgetData = {
        labels: ['201501', '201502', '201503', '201504', '201505', '201506', '201507', '201508', '201509', '201510', '201511', '201512', '201513'],

        datasets: [
          {
              label: 'Actual',
              fillColor: '#0cc09f',
              strokeColor: '#0cc09f',
              highlightFill: '#0cc09f',
              highlightStroke: '#0cc09f',
              data: $scope.usiHoursActualData
          },
          {
              label: 'Budget',
              fillColor: 'white',
              strokeColor: 'white',
              highlightFill: 'white',
              highlightStroke: 'white',
              data: $scope.usiHoursBudgetData
          }
        ]
    };

    // Chart.js Options
    $scope.actualVsBudgetDataOptions = {

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

    $scope.UpdateActualVsBudgetChart();

    // added by Vibhav. To update skill revenue chart 
    $scope.UpdateRevenueChart = function () {
        $http({
            method: 'GET',
            url: 'api/Dashboard/GetRevenueChartData'
        }).
      success(function (data, status, headers, config) {
          if (data != null) {

              $scope.revenueChartPrevData = JSON.parse(data[1]);
              $scope.revenueChartCurrData = JSON.parse(data[0]);

              $scope.skillChartData.datasets[0].data = $scope.revenueChartCurrData;
              $scope.skillChartData.datasets[1].data = $scope.revenueChartPrevData;
              $scope.skillChartData.datasets[0].label = JSON.parse(data[2]);
              $scope.skillChartData.datasets[1].label = JSON.parse(data[3]);

              $scope.YoYInnerData = JSON.parse(data[4]);
              $scope.YoYLabels = JSON.parse(data[5]);
              $scope.YoYData.datasets[0].data = $scope.YoYInnerData;
              $scope.YoYData.labels = $scope.YoYLabels;
          }
      }).
      error(function (data, status, headers, config) {
          // called asynchronously if an error occurs
          // or server returns response with an error status.
          $scope.AllResources = -1;
      });
    };

    // Chart.js Data
    $scope.skillChartData = {
        labels: ['Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec', 'Jan', 'Feb', 'Mar'],
        datasets: [
          {
              label: 'FY15 Business',
              fillColor: 'rgba(620,201,102,0.75)',
              strokeColor: 'rgba(220,220,220,0.8)',
              highlightFill: 'rgba(69,201,102,0.9)',
              highlightStroke: 'rgba(220,220,220,1)',
              data: $scope.revenueChartCurrData//[65, 59, 80, 81, 56, 55, 40]
              //data: [0, 10764, 7866, 18819, 20674, 7494, 54873, 22355, 147398, 47036, 125764, 126168]
          },
          {
              label: 'FY14 Business',
              fillColor: 'rgba(321, 176, 0, 1)',
              strokeColor: 'rgba(321, 176, 0, 0.8)',
              highlightFill: 'rgba(38, 208, 255, 0.75)',
              highlightStroke: 'rgba(151,187,205,1)',
              //data: [28, 48, 40, 19, 86, 27, 30,12,12,12,12,12]
              data: $scope.revenueChartPrevData
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

    //$scope.UpdateRevenueChart();

    $scope.YoYData = {
        labels: $scope.YoYLabels,
        datasets: [
          {
              label: 'YoY Revenue(million USD)',
              fillColor: '#0cc09f',
              strokeColor: 'rgba(220,220,220,0.8)',
              highlightFill: '#0aac8e',
              highlightStroke: 'rgba(220,220,220,1)',
              data: $scope.YoYInnerData
          }
        ]
    };

    $scope.YoYOptions = {

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
        barStrokeWidth: 1,

        //Number - Spacing between each of the X value sets
        barValueSpacing: 50,

        //Number - Spacing between data sets within X values
        barDatasetSpacing: 5,

        //String - A legend template
        legendTemplate: '<div class="tc-chart-js-legend"><% for (var i=0; i<datasets.length; i+=3){%><span style="background-color:<%=datasets[i].fillColor%>"></span><%if(i<datasets.length){%><%=datasets[i].label%><%}%><%if(i+1<datasets.length){%><span style="background-color:<%=datasets[i+1].fillColor%>"></span> &nbsp; <%=datasets[i+1].label%><%}%><%if(i+2<datasets.length){%><span style="background-color:<%=datasets[i+2].fillColor%>"></span><%=datasets[i+2].label%><%}%><%}%></div>'
    };


    $scope.UpdateActivePursuitsChart = function () {
        $http({
            method: 'GET',
            url: 'api/Dashboard/GetActivePursuitsChartData?authToken=' + $scope.UserIdentity + "-" + $scope.UserPassword
        }).
      success(function (data, status, headers, config) {
          if (data != null) {
              $scope.yesProposalSupport = JSON.parse(data[0]);
              $scope.yesStillQualifying = JSON.parse(data[1]);

              $scope.activePursuitsData.datasets[0].data = $scope.yesProposalSupport;
              $scope.activePursuitsData.datasets[1].data = $scope.yesStillQualifying;
              $scope.activePursuitsData.labels = JSON.parse(data[2]);
              $scope.activePursuitsData.datasets[0].label = JSON.parse(data[3])[0];
              $scope.activePursuitsData.datasets[1].label = JSON.parse(data[3])[1];

          }
      }).
      error(function (data, status, headers, config) {
          // called asynchronously if an error occurs
          // or server returns response with an error status.
          $scope.AllResources = -1;
      });
    };

    // Chart.js Data
    $scope.activePursuitsData = {
        labels: ['2015 - 05', '2015 - 07', '2015 - 09', '2015 - 10', '2015 - 11', '2015 - 12', '2015 - 13', '2016 - 01', '2016 - 02', '2016 - 05', '2017 - 01', '2017 - 08', '2017 - 10'],

        datasets: [
          {
              label: 'Yes - Proposal Support',
              fillColor: '#0cc09f',
              strokeColor: '#0cc09f',
              highlightFill: '#0cc09f',
              highlightStroke: '#0cc09f',
              data: $scope.yesProposalSupport
          },
          {
              label: 'Yes-Still Qualifying',
              fillColor: 'white',
              strokeColor: 'white',
              highlightFill: 'white',
              highlightStroke: 'white',
              data: $scope.yesStillQualifying
          }
        ]
    };

    // Chart.js Options
    $scope.activePursuitsDataOptions = {

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

    $scope.UpdateActivePursuitsChart();




    $scope.UpdateActivePursuitsByLeadChart = function () {
        $http({
            method: 'GET',
            url: 'api/Dashboard/GetPursuitsByLeadChartData?authToken=' + $scope.UserIdentity + "-" + $scope.UserPassword
        }).
      success(function (data, status, headers, config) {
          if (data != null) {

              $scope.yesProposalSupportByLead = JSON.parse(data[0]);
              $scope.yesStillQualifyingByLead = JSON.parse(data[1]);

              $scope.activePursuitsByLeadData.datasets[0].data = $scope.yesProposalSupportByLead;
              $scope.activePursuitsByLeadData.datasets[1].data = $scope.yesStillQualifyingByLead;
              $scope.activePursuitsByLeadData.labels = JSON.parse(data[3]);

              $scope.PBLbarData.labels = JSON.parse(data[3]);
              $scope.PBLbarData.series[0] = JSON.parse(data[0]);
              $scope.PBLbarData.series[1] = JSON.parse(data[1]);
          }
      }).
      error(function (data, status, headers, config) {
          // called asynchronously if an error occurs
          // or server returns response with an error status.
          $scope.AllResources = -1;
      });
    };

    // Chart.js Data
    $scope.activePursuitsByLeadData = {
        labels: ['LEPETERSON', 'MARCIADOUGLAS', 'PFINKLESTEIN', 'RLABUHN', 'NDERCOLE', 'JOWALLACE', 'MBHARADWAJ', 'RPINEDA', 'JJAAJ'],

        datasets: [
          {
              label: 'Yes - Proposal Support',
              fillColor: 'rgba(620,201,102,0.75)',
              strokeColor: 'rgba(220,220,220,0.8)',
              highlightFill: 'rgba(69,201,102,0.9)',
              highlightStroke: 'rgba(220,220,220,1)',
              data: $scope.yesProposalSupportByLead
          },
          {
              label: 'Yes-Still Qualifying',
              fillColor: '#FFFFFF',
              strokeColor: 'rgba(1,187,205,0.8)',
              highlightFill: 'rgba(38, 208, 255, 0.75)',
              highlightStroke: 'rgba(151,187,205,1)',
              data: $scope.yesStillQualifyingByLead
          }
        ]
    };

    // Chart.js Options
    $scope.activePursuitsByLeadDataOptions = {

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

        scaleFontSize: 10,

        //Number - Pixel width of the bar stroke
        barStrokeWidth: 2,

        //Number - Spacing between each of the X value sets
        barValueSpacing: 20,

        //Number - Spacing between data sets within X values
        barDatasetSpacing: 1,

        //String - A legend template
        legendTemplate: '<div class="tc-chart-js-legend"><% for (var i=0; i<datasets.length; i+=3){%><span style="background-color:<%=datasets[i].fillColor%>"></span><%if(i<datasets.length){%><%=datasets[i].label%><%}%><%if(i+1<datasets.length){%><span style="background-color:<%=datasets[i+1].fillColor%>"></span> &nbsp; <%=datasets[i+1].label%><%}%><%if(i+2<datasets.length){%><span style="background-color:<%=datasets[i+2].fillColor%>"></span><%=datasets[i+2].label%><%}%><%}%></div>'
        //legendTemplate: '<ul class="tc-chart-js-legend"><% for (var i=0; i<datasets.length; i++){%><li><span style="background-color:<%=datasets[i].fillColor%>"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>'
    };

    $scope.UpdateActivePursuitsByLeadChart();


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
              //
              $scope.SoldProposedChartLabels = JSON.parse(data[0]);
              $scope.SoldProjectsChartData = JSON.parse(data[1]);
              $scope.ProposedProjectsChartData = JSON.parse(data[2]);

              $scope.SoldProposedData.labels = $scope.SoldProposedChartLabels;
              $scope.SoldProposedData.datasets[0].data = $scope.SoldProjectsChartData;
              $scope.SoldProposedData.datasets[1].data = $scope.ProposedProjectsChartData;
          }
      }).
      error(function (data, status, headers, config) {
          // called asynchronously if an error occurs
          // or server returns response with an error status.
          $scope.AllResources = -1;
      });
    };

    //$scope.UpdateSoldProposedChart();

    // Chart.js Data
    $scope.SoldProposedData = {
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
    $scope.SoldProposedOptions = {

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


    //$scope.USIEngmntByIMLeadData.datasets[0].data = $scope.USIEngmntByIMLead;
    //$scope.USIEngmntByIMLeadData.labels = JSON.parse(data[0]);

    $scope.UpdateActualUSIEngmntByIMLead();

    //Chartist Charts
    $scope.barData = {
        labels: [],
        series: [[]]
    };

    $scope.barOptions = {
        seriesBarDistance: 15,
        horizontalBars: true,
        axisY: { offset: 80 }
    };

    $scope.barResponsiveOptions = [
        ['screen and (min-width: 641px) and (max-width: 1024px)', {
            seriesBarDistance: 10,
            axisX: {
                labelInterpolationFnc: function (value) {
                    return value;
                }
            }
        }],
        ['screen and (max-width: 640px)', {
            seriesBarDistance: 5,
            axisX: {
                labelInterpolationFnc: function (value) {
                    return value[0];
                }
            }
        }]
    ];

    //Chartist USI Pursuits by Lead


    $scope.PBLbarData = {
        labels: [],
        series: [[]]
    };

    $scope.PBLbarOptions = {
        seriesBarDistance: 15,
        horizontalBars: true,
        axisY: { offset: 80 },
        animate: {
            opacity: {
                dur: 3000,
                from: 0,
                to: 1
            },
            x1: {
                dur: '5000ms',
                from: 100,
                to: 200,
                easing: 'easeOutQuart'
            },
            y1: {
                dur: '2s',
                from: 0,
                to: 100
            }
        }
    };

    $scope.PBLbarResponsiveOptions = [
        ['screen and (min-width: 641px) and (max-width: 1024px)', {
            seriesBarDistance: 10,
            axisX: {
                labelInterpolationFnc: function (value) {
                    return value;
                }
            }
        }],
        ['screen and (max-width: 640px)', {
            seriesBarDistance: 5,
            axisX: {
                labelInterpolationFnc: function (value) {
                    return value[0];
                }
            }
        }]
    ];

    //Chartist - Engagement by Client
    $scope.EBCbarData = {
        labels: [],
        series: [[]]
    };

    $scope.EBCbarOptions = {
        seriesBarDistance: 15,
        horizontalBars: true,
        axisY: {
            offset: 160
        }
    };

    $scope.EBCbarResponsiveOptions = [
        ['screen and (min-width: 641px) and (max-width: 1024px)', {
            seriesBarDistance: 10,
            axisX: {
                labelInterpolationFnc: function (value) {
                    return value;
                }
            }
        }],
        ['screen and (max-width: 640px)', {
            seriesBarDistance: 5,
            axisX: {
                labelInterpolationFnc: function (value) {
                    return value[0];
                }
            }
        }]
    ];

    //Chartist - Pursuits by Client
    $scope.PBCbarData = {
        labels: [],
        series: [[]]
    };

    $scope.PBCbarOptions = {
        seriesBarDistance: 15,
        horizontalBars: true,
        axisY: {
            offset: 180
        }
    };

    $scope.PBCbarResponsiveOptions = [
        ['screen and (min-width: 641px) and (max-width: 1024px)', {
            seriesBarDistance: 10,
            axisX: {
                labelInterpolationFnc: function (value) {
                    return value;
                }
            }
        }],
        ['screen and (max-width: 640px)', {
            seriesBarDistance: 5,
            axisX: {
                labelInterpolationFnc: function (value) {
                    return value[0];
                }
            }
        }]
    ];






}]);

CICPApp.controller('InvoicesController', ['$scope', '$filter', '$http', 'FileUploader', function ($scope, $filter, $http, FileUploader) {
    var STORAGE_ID = 'Invoices';
    $scope.EditMode = "false";
    $scope.currentPage = 1;
    $scope.pageSize = 10;

    var InvoiceDetails = $scope.InvoiceDetails = [];

    $scope.getInvoices = function () {
        debugger;
        $http({
            method: 'GET',
            url: 'api/Dashboard/GetReferenceData?storageId=' + STORAGE_ID + '&authToken=' + $scope.UserIdentity + "-" + $scope.UserPassword
        }).
        success(function (data, status, headers, config) {

            if (data != 'null') {
                InvoiceDetails = $scope.InvoiceDetails = JSON.parse(JSON.parse(data));
                $scope.$parent.PendingInvoices = $filter('filter')(InvoiceDetails, { PaymentReceived: "Pending" }).length;

            }
        }).
        error(function (data, status, headers, config) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.

        });

    };

    $scope.EditInvoice = function (invoice, index) {
        debugger;
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
        debugger;
        var referenceData = new Object();
        referenceData.storageId = STORAGE_ID;
        referenceData.storageData = JSON.stringify(InvoiceDetails);
        referenceData.authToken = $scope.UserIdentity + "-" + $scope.UserPassword;
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
        ////debugger;
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

    $scope.predicate;
    $scope.reverse = false;
    $scope.sort = function (item) {
        if ($scope.predicate == 'Period') {
            return new Date(item.Period);
        }
        return item[$scope.predicate];
    };

    //$scope.sortBy = function (field) {
    //    if ($scope.predicate != field) {
    //        $scope.predicate = field;
    //        $scope.reverse = false;
    //    } else {
    //        $scope.reverse = !$scope.reverse;
    //    }
    //};
}]);

angular.module('CICPApp').filter('changeDateFormat', function ($filter) {
    return function (input) {
        if (input == null) { return ""; }
        var _date = $filter('date')(new Date(input), 'MMM-yy');
        return _date.toUpperCase();
        //return new Date(input);
    };
});
