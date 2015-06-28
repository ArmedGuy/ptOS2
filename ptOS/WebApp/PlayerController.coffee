# CoffeeScript

app = angular.module "ptOS"
app.controller "ptOS.Player", ($scope, $routeParams) ->
    @$inject = ["$scope", "$routeParams"]
    $scope.playerConnections = ["US": 4, "SE": 3]