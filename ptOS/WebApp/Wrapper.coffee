# CoffeeScript

app = angular.module "ptOS"
app.controller "ptOS.Wrapper", ($scope, NavSliderService, SliderService, CurrentService, RealtimeService) ->
    @$inject = ["$scope", "NavSliderService", "SliderService", "CurrentService", "RealtimeService"]
    $scope.navSlider = -> 
        NavSliderService.isOpen()
    $scope.navSliderTemplate = ->
        NavSliderService.getTemplate()
    $scope.toggleNavSlider = ->
        NavSliderService.toggle()
    
    $scope.isCurrentPath = (path) ->
        CurrentService.isCurrentPath(path)
    $scope.isCurrentPathExact = (path) ->
        CurrentService.isCurrentPathExact(path)
    
    $scope.currentPlayer = ->
        CurrentService.getCurrentPlayer()
    $scope.currentServer = ->
        CurrentService.getCurrentServer()
        
    $scope.eventTemplate = (event) ->
        "/WebApp/templates/events/#{event.Type}.html"