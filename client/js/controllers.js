'use strict';

/* Controllers */

angular.module('myApp.controllers', [])
  .controller('anunciosCtrl', function ($scope, $http) {
	  $http.defaults.useXDomain = true;	
	  $http.get(restApi + '/anuncios').success(function(data) {
	    $scope.anuncios = data;
	  });
  })
  .controller('alvosCtrl', function ($scope, $http) {
	  $http.defaults.useXDomain = true;	

	  $http.get(restApi + '/alvos').success(function(data) {
	    $scope.alvos = data;
	  });

	  $scope.updateHtmlAlvo = function(site_id) {
		  $scope.statusAlvo = '{"novoStatusAlvo":"[r]"}';
		  $http({
			  method : 'PUT',
			  url : restApi + '/alvos/' + site_id,
			  data : $scope.statusAlvo }).success(function(data) {
			  	$scope.resultadoUpdate = data;
			  });
	  }

	  $scope.getAnuncio = function(site_id) {
		  $http({
			  method : 'GET',
			  url : restApi + '/alvos/' + site_id + '/anuncio'})
			  .success(function(data) {
			  	$scope.resultadoUpdate = data;
			  });
	  }
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
