# CoffeeScript

scale = ['#C7DAED', '#2C3E50']
app = angular.module "ptOS"

app.directive "worldMap", ->
    
    restrict: "E"
    scope:
        mapData: "="
    link: (scope, element, attrs) ->
        chart = null
        $(element).addClass "world-map"
        scope.$watch "mapData", (n, o) ->
            if not chart?
                $(element).width 'auto'
                height = if attrs.mapHeight? then attrs.mapHeight else 400
                $(element).height "#{height}px"
                
                chart = $(element).vectorMap
                    backgroundColor: "#DDDDDD"
                    series: {
                        regions: [
                            values: scope.mapData
                            scale: scale
                            attribute:'fill'
                            normalizeFunction: 'linear'
                        ]
                    }
            else
                region = chart.vectorMap('get', 'mapObject').series.regions[0]
                region.setValues scope.mapData
                region.setNormalizeFunction 'linear'
                region.setScale(scale)
        , true