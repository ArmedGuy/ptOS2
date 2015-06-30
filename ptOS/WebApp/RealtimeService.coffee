# CoffeeScript

app = angular.module "ptOS"

app.factory "RealtimeService", ($rootScope, EventService) ->
    @$inject = ["$rootScope", "EventService"]
    
    hub = $.connection.hub
    realtime = $.connection.realtimeHub
    connected = false
    reconnecting = false
    
    isConnected = ->
        connected
    
    hub.start().done ->
        connected = true
    .fail ->
        connected = false
    
    hub.reconnecting ->
        reconnecting = true
        connected = false
    .reconnected ->
        reconnecting = false
        connected = true
    .disconnected ->
        connected = false
        reconnecting = false
    
    listener = (event, data) ->
        EventService.parseEvent data
        $rootScope.$apply () ->
            $rootScope.$broadcast(event, data)
    
    realtime.client.Broadcast = listener
    
    connected: isConnected
    