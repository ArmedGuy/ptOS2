# CoffeeScript

app = angular.module "ptOS"
app.controller "ptOS.Dashboard", ($scope, SliderService, RealtimeService, $http) ->
    @$inject = ["$scope", "SliderService", "$http"]
    
    $scope.systemStats = [
        AltId: "EventsPerHour"
        Value: 200
    ]
    $scope.events = []
    $scope.connected = ->
        RealtimeService.connected()
    
    $scope.$on "event.new", (evt, data) ->
        $scope.events.unshift(data)
        $scope.events.pop() if $scope.events.length > 20
    
    $http.get "/api/Statistics/System"
        .then (result) ->
            throw err if err?
            $scope.systemStats = result.data