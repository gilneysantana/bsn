'use strict';


// Declare app level module which depends on filters, and services
angular.module('myApp', ['myApp.filters', 'myApp.services', 'myApp.directives', 'myApp.controllers']).
  config(['$routeProvider', function($routeProvider) {
    $routeProvider.when('/anuncios', {templateUrl: 'partials/anuncios.html', controller: 'anunciosCtrl'});
    $routeProvider.when('/alvos', {templateUrl: 'partials/alvos.html', controller: 'alvosCtrl'});
    $routeProvider.when('/alvos/novo', {templateUrl: 'partials/alvo-novo.html', controller: 'alvosNovoCtrl'});
    $routeProvider.when('/alvos/:site_id', {templateUrl: 'partials/alvo-detail.html', controller: 'alvosDetailCtrl'});
    $routeProvider.when('/sites', {templateUrl: 'partials/sites.html', controller: 'sitesCtrl'});
    $routeProvider.otherwise({redirectTo: '/alvos'});
  }]);
