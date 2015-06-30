# CoffeeScript

app = angular.module "ptOS"
app.factory "EventService", ->
    @$inject = []
    
    parseEvent: (event) ->
        event.EventData = {}
        _.forEach event.EventDatas, (ed) ->
            event.EventData[ed.Key] = ed.Value