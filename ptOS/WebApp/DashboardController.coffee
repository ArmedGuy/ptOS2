# CoffeeScript

app = angular.module "ptOS"
app.controller "ptOS.Dashboard", ($scope, SliderService, RealtimeService, $http, $window) ->
    @$inject = ["$scope", "SliderService", "Realtime", "$http", "$window"]
    
    byHourChart = null
    byServerChart = null
    
    $scope.systemStats = [
        AltId: "EventsPerHour"
        Value: 200
    ]
    $scope.playerCountries = {}
    $scope.events = []
    $scope.eventsByHour = ["Events processed by Hour (in UTC)"]
    $scope.eventsByHourLabels = []
    $scope.eventsByHourValues = []
    $scope.connected = ->
        RealtimeService.connected()
    
    $scope.$on "event.new", (evt, data) ->
        $scope.events.unshift(data)
        $scope.events.pop() if $scope.events.length > 20
    
    $http.get "/api/Statistics/System"
        .then (result) ->
            $scope.systemStats = result.data
    
    $http.get "/api/Statistics/CountriesLastHour"
        .then (result) ->
            _.forEach result.data, (ct) ->
                $scope.playerCountries[ct.Country] = ct.Weight
    
    $http.get "/api/Statistics/EventsByHourLastDay"
        .then (result) ->
            l = []
            v = []
            _.forEach result.data, (e) ->
                l.push e.Key
                v.push e.Value
            ctx = $window.document.getElementById("eventsByHour").getContext "2d"
            
            data = 
                labels: l
                datasets: [
                    label: "Events processed by Hour (in UTC)"
                    fillColor: "rgba(151,187,205,0.2)"
                    strokeColor: "rgba(151,187,205,1)"
                    pointColor: "rgba(151,187,205,1)"
                    pointStrokeColor: "#fff"
                    pointHighlightFill: "#fff"
                    pointHighlightStroke: "rgba(151,187,205,1)"
                    data: v
                ]
            
            byHourChart = new Chart(ctx).Line data
    colors = ["#97BBCD", "#DCDCDC", "#F7464A", "#46BFBD", "#FDB45C", "#949FB1", "#4D5360"]
    colorIndex = 0
    $http.get "/api/Statistics/EventsByServerLastDay"
        .then (result) ->
            data = []
            _.forEach result.data, (e) ->
                data.push
                    value: e.Value
                    label: e.Key
                    color: colors[colorIndex]
                    highlight: "#EEEEEE"
                colorIndex++
                colorIndex = 0 if colorIndex == colors.length
                    
            ctx = $window.document.getElementById("eventsByServer").getContext "2d"
            
            byServerChart = new Chart(ctx).Doughnut data