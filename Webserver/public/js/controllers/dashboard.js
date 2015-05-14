/*global angular, Bloodhound*/
'use strict';

var db = angular.module('controllers.dashboard', [
  'wu.masonry',
  'ui.bootstrap',
  'angularSmoothscroll',
  'ui.slider',
  'siyfion.sfTypeahead'
]);

function dbCtrl($scope, $rootScope, $http, $window) {
  $scope.movies = [];
  $scope.allMovies = [];
  var movieHash = {};
  /*
    $scope.features is a json object composed of fields:
      - name
      - weight (between 0 and 100)
      - relevancy (between 0 and 100)
   */
  $scope.features = [];
  var selectedMovieTitle;
  $scope.uiSliderOptions = {
    range: 'min'
  };
  $scope.exampleOptions = {};
  $('.tt-query').css('background-color','#fff');    

  $scope.searchMovie = function() {
    if (!(selectedMovieTitle in movieHash)) {
      return;
    }
    var selectedMovie = movieHash[selectedMovieTitle];
    for (var i = 0; i < selectedMovie.Features.length; ++i) {
      $scope.features[i].weight = Math.round(selectedMovie.Features[i]*100);
    }
  };

  $http.get($rootScope.baseUrl+'/data')
    .success(function(data) {
      $scope.features = data.FeatureNames.map(function(featureName) {
        return {
          name: featureName,
          relevancy: 50,
          weight: 50
        };
      });
      $scope.allMovies = data.Movies;
      $('#query').typeahead({
        local: data.Movies.map(function(movie) { return movie.Title; })
      }).on('typeahead:selected', function (obj, datum) {
        selectedMovieTitle = datum.value;
      });
      $scope.movies = data.Movies.slice(0, 15);
      for (var i = 0; i < data.Movies.length; ++i) {
        movieHash[data.Movies[i].Title] = data.Movies[i];
      }
    })
    .error(function(data) {
      console.log('An error occurred with the get request!');
    });

  $scope.setRelevancyTo = function(value) {
    $scope.features = $scope.features.map(function(element) {
      element.relevancy = value;
      return element;
    });
  };

  $scope.gotoImdb = function(imdbID) {
    $window.open('http://www.imdb.com/title/'+imdbID);
  };

  $scope.recommendMovies = function() {
    var postData = $scope.features;

    //Get into right format
    var newbody = {};
    newbody.nresults = 20;
    newbody.features = [];
    newbody.weights = [];
    for (var i = 0; i < postData.length; i++) {
      newbody.features[i] = postData[i].weight;
      newbody.weights[i] = postData[i].relevancy;
    }

    postData = newbody;

    $http.post($rootScope.baseUrl+'/data', postData)
      .success(function(data) {
        $scope.movies = JSON.parse(data.movies);
        $('#gotoMovies')[0].click();
      })
      .error(function(data) {
        console.log('An error occurred with the post request!');
      });
  }
}

db.controller('DashboardCtrl', 
  [
    '$scope',
    '$rootScope',
    '$http',
    '$window',
    dbCtrl
  ]);