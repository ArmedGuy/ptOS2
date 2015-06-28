# CoffeeScript

app = angular.module "ptOS"
app.controller "ptOS.Servers", ($scope, $http) ->
    @$inject = ["$scope", "$http"]
    $scope.servers = []
    $scope.serverName = ""
    
    $http.get "/api/Servers"
        .then (result) ->
            $scope.servers = result.data;
    
    $scope.add = ->
        $http.post "/api/Servers", Name: $scope.serverName
            .then (result) ->
                $scope.servers.push result.data
    
    $scope.remove = (server) ->
        if confirm("Are you sure you want to remove #{server.Name}?")
            $http.delete "/api/Servers/#{server.Id}"
                .then (result) ->
                    index = _.indexOf($scope.servers, server)
                    $scope.servers.splice(index, 1)