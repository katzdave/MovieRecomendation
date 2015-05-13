'use strict';
/*global angular*/

var app = angular.module('movieRecommendation', [
  'ngResource',
  'ngSanitize',
  'ngRoute',
  'controllers.dashboard'
]);

function configApp($routeProvider, $locationProvider) {
  $routeProvider
    .when('/', {
      templateUrl: 'partials/dashboard.html',
      controller: 'DashboardCtrl'
    })
    .otherwise({
      redirectTo: '/'
    });
  $locationProvider.html5Mode(true);
}

app.config([
  '$routeProvider', 
  '$locationProvider', 
  configApp
]);

app.run(['$rootScope', function($rootScope) {
  //EDIT THIS URL TO POINT TO YOUR SERVER
  $rootScope.baseUrl = 'http://localhost:3000';
}]);