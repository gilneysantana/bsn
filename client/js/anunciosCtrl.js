angular.module('Test', ['ngResource']).controller('sitesCtrl', function ($scope, $http, $resource) {

  $http.defaults.useXDomain = true;	

  $http.get('http://localhost:8888/anuncios').success(function(data) {
    $scope.sites = data;
  });
});


