# CoffeeScript

app = angular.module "ptOS"
app.controller "ptOS.Server", ($scope, $routeParams, $http, CurrentService) ->
    @$inject = ["$scope", "$routeParams", "$http", "CurrentService"]
    id = $routeParams.id
    
    $scope.serverStats = []
    $scope.events = []
    $scope.server = null
    
    $scope.$on "event.new", (evt, data) ->
        if data.Server? and data.Server.Id == $scope.server.Id
            $scope.events.unshift(data)
            $scope.events.pop() if $scope.events.length > 20
    
    $http.get "/api/Statistics/Server/#{id}"
        .then (result) ->
            throw err if err?
            $scope.serverStats = result.data
            
    $http.get "/api/Servers/#{id}"
        .then (result) ->
            CurrentService.setCurrentServer result.data
            $scope.server = result.data