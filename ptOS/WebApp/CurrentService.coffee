# CoffeeScript

app = angular.module "ptOS"
app.factory "CurrentService", ($location) ->
    @$inject = ["$location"]
    _currentPlayer = null
    _currentServer = null
    
    setCurrentPlayer: (plr) ->
        _currentPlayer = plr
    getCurrentPlayer: ->
        _currentPlayer
    
    setCurrentServer: (srv) ->
        _currentServer = srv
    getCurrentServer: ->
        _currentServer
    
    isCurrentPath: (path) ->
        $location.path().indexOf(path) != -1
    isCurrentPathExact: (path) ->
        angular.equals($location.path(), path)
    
    