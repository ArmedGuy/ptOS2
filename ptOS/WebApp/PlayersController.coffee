# CoffeeScript

app = angular.module "ptOS"
app.controller "ptOS.Players", ($scope, $http) ->
    @$inject = ["$scope", "$http"]
    $scope.players = []
    $scope.query = ""
    
    $scope.search = ->
        q = $scope.query
        
        $http.get "/api/Players/Search/#{q}"
            .then (result) ->
                $scope.players = result.data