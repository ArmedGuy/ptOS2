# CoffeeScript
app = angular.module "ptOS", ["ngAnimate", "ngRoute", "pageslide-directive"]


app.config ($routeProvider) ->
    @$inject = ["$routeProvider"]
    
    tmpl = (file) ->
        "/WebApp/templates/#{file}.html"
    
    ctrl = (controller) ->
        "ptOS.#{controller}"
        
    go = (name) ->
        templateUrl: tmpl name
        controller: ctrl name
    
    $routeProvider
        .when "/", go "Dashboard"
        .when "/Players", go "Players"
        .when "/Players/:id", go "Player"
        .when "/Servers", go "Servers"
        .when "/Servers/:id", go "Server"
        .otherwise "/"