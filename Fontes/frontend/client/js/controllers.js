'use strict';

/* Controllers */

angular.module('myApp.controllers', []).
   controller('MyCtrl1', [function() {}])
  .controller('MyCtrl2', [function() {}])
  .controller('anunciosCtrl', function ($scope, $http) {
	  $http.defaults.useXDomain = true;	
	  $http.get(restApi + '/anuncios').success(function(data) {
	    $scope.anuncios = data;
	  });
  })
  .controller('alvosCtrl', function ($scope, $http) {
	  $http.defaults.useXDomain = true;	
	  $http.get(restApi + '/alvos').success(function(data) {
	    $scope.anuncios = data;
	  });
  })
  .controller('alvosDetailCtrl', function ($scope, $http, $routeParams) {
	  $http.defaults.useXDomain = true;	
	  $http.get(restApi + '/alvos/' + $routeParams.site_id).success(function(data) {
	    $scope.alvo = data;
	  });
  })
  .controller('alvosNovoCtrl', function ($scope, $http) {
      $http.defaults.useXDomain = true;	
      $scope.site = {};
      $scope.createUser = function() {
        $http({
            method : 'POST',
            url : restApi + '/sites/novo',
            data : $scope.site }).success(function(data) {
	    	$scope.resultado = data;
	    });
        }
  })
  .controller('sitesCtrl', function ($scope, $http) {
	  $http.defaults.useXDomain = true;	
	  $http.get(restApi + '/sites').success(function(data) {
	    $scope.sites = data;
	  });
  })
;
