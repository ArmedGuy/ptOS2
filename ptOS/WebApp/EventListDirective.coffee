# CoffeeScript

app = angular.module "ptOS"

app.filter "take", ->
    (input, num) ->
        _.take input, num

app.filter "skip", ->
    (input, num) ->
        _.slice input, num

app.directive "eventList", ->
    
    restrict: "E"
    scope:
        listEvents: "=events"
        perPage: "="
        
    templateUrl: "/WebApp/templates/directives/EventList.html"
    link: (scope, element, attrs) ->
        scope.pages = ->
            Math.ceil(scope.listEvents.length / scope.perPage)
        scope.eventTemplate = (event) ->
            "/WebApp/templates/events/#{event.Type}.html"
        scope.page = 0
        scope.nextPage = ->
            scope.page++
            scope.page = scope.pages() - 1 if scope.page >= scope.pages()
        scope.prevPage = ->
            scope.page--
            scope.page = 0 if scope.page < 0