# CoffeeScript
app = angular.module "ptOS"
app.factory "SliderService", ->
    sliderTemplate = ""
    open = true
    
    setTemplate: (tmpl) ->
        sliderTemplate = "/WebApp/templates/#{tmpl}.html"
    open: ->
        open = true
    close: ->
        open = false
    toggle: ->
        open = !open
    isOpen: ->
        open
    getTemplate: ->
        sliderTemplate
    