# CoffeeScript

app = angular.module "ptOS"
app.controller "ptOS.Player", ($scope, $routeParams, EventService, $http) ->
    @$inject = ["$scope", "$routeParams", "EventService", "$http"]
    
    $scope.player = null
    $scope.events = []
    $scope.playerFrom = {}
    $scope.playerChat = []
    $scope.playerAdminEvents = []
    $scope.playerIpChanges = []
    $scope.playerNameChanges = []
    id = parseInt $routeParams.id
    
    $scope.$on "event.new", (evt, data) ->
        if data.Player? and data.Player.Id == $scope.player.Id
            $scope.events.unshift(data)
            $scope.events.pop() if $scope.events.length > 20
            
    $http.get "/api/Players/#{id}"
        .then (result) ->
            $scope.player = result.data
            $scope.playerFrom[result.data.LastCountry] = 1
            
            $http.get "/api/Players/#{id}/Chat"
                .then (result) ->
                    _.forEach result.data, (e) ->
                        EventService.parseEvent e
                    $scope.playerChat = result.data
            
            $http.get "/api/Players/#{id}/AdminEvents"
                .then (result) ->
                    _.forEach result.data, (e) ->
                        EventService.parseEvent e
                    $scope.playerAdminEvents = result.data
            
            $http.get "/api/Players/#{id}/IpChanges"
                .then (result) ->
                    _.forEach result.data, (e) ->
                        EventService.parseEvent e
                    $scope.playerIpChanges = result.data
            
            $http.get "/api/Players/#{id}/NameChanges"
                .then (result) ->
                    _.forEach result.data, (e) ->
                        EventService.parseEvent e
                    $scope.playerNameChanges = result.data