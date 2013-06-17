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
  .controller('alvosNovoCtrl', function ($scope, $http) {
      $http.defaults.useXDomain = true;	
      $scope.site = {};
      $scope.createUser = function() {
        $http({
            method : 'POST',
            url : 'http://localhost:8888/sites/novo',
            data : $scope.site }).success(function(data) {
	    	$scope.resultado = data;
	    });
        }
  });
