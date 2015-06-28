# CoffeeScript
app = angular.module "ptOS"
app.factory "NavSliderService", ->
    open = true
    
    open: ->
        open = true
    close: ->
        open = false
    toggle: ->
        open = !open
    isOpen: ->
        open
    getTemplate: ->
        "/WebApp/templates/NavSlider.html"
        
    