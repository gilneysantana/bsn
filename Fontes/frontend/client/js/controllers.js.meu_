angular.module('Test', ['ngResource']).controller('corsCtrl', function ($scope, $http, $resource) {

  $http.defaults.useXDomain = true;	

  $http.get('http://localhost:8888/phones').success(function(data) {
    $scope.phones = data;
  });

  $scope.orderProp = 'age';

});


