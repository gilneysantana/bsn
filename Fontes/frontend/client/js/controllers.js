'use strict';

/* Controllers */

angular.module('myApp.controllers', []).
   controller('MyCtrl1', [function() {}])
  .controller('MyCtrl2', [function() {}])
  .controller('anunciosCtrl', function ($scope, $http) {
	  $http.defaults.useXDomain = true;	
	  $http.get('http://localhost:8888/anuncios').success(function(data) {
	    $scope.anuncios = data;
	  });
  })
  .controller('alvosCtrl', function ($scope, $http) {
	  $http.defaults.useXDomain = true;	
	  $http.get('http://localhost:8888/alvos').success(function(data) {
	    $scope.anuncios = data;
	  });
  })
  ;
