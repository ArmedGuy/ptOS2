# CoffeeScript

app = angular.module "ptOS"

app.controller "ptOS.Login", ($scope, $http) ->
    @$inject = ["$scope", "$http"]
    
    