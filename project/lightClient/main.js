
function sendRequestToRoutingWS(){
    var caller = new XMLHttpRequest();
    var request = "http://localhost:8734/Design_Time_Addresses/Routing/rest/maproute?start=" 
    + document.getElementById("depart").value + "&end=" + document.getElementById("arrivee").value;
    console.log("Sending request to Proxy : " + request)
    caller.open("GET", request, true);
    caller.setRequestHeader ("Accept", "application/json");
    caller.onload=displayMap;
    caller.send();
}

function displayError(error){
    document.getElementById("error").innerHTML = error;
}

function displayMap(){
    var data = JSON.parse(this.responseText).GetRoutingMapResult
    if(data.infos!="OK"){
        displayError(data.infos);
        return;
    }
    data = data .routes;
    console.log(data)
    route1 = JSON.parse(data[0])
    route2 = JSON.parse(data[1])
    route3 = JSON.parse(data[2])
    mapboxgl.accessToken = 'pk.eyJ1IjoibHVjYXNwb2x5dGVjaCIsImEiOiJja25lazgxemYyNjZ6MnVtcWNuY2ltMTU5In0.PRHFI72a0818EQvpX_VLzA'; 
    start = route1.waypoints[0].location
    end = route3.waypoints[1].location
    path = [start, route2.waypoints[0].location, route3.waypoints[0].location, end]
    var map = new mapboxgl.Map({
        container: 'map',
        style: 'mapbox://styles/mapbox/streets-v9',
        center: [start[0], start[1]],
        zoom: 11
    });
    
    map.on('load', function () {
         for(var i=0; i<path.length;i++){
                var icon = null;
                if(i == 0 || i ==3){
                    icon =  "circle-stroked-11"
                }
                else{
                    icon = "bicycle-15"
                }
                map.addLayer({
                    "id": "layer"+i,
                    "type": "symbol",
                    "source": {
                        "type": "geojson",
                        "data": {
                            "type": "FeatureCollection",
                            "features": [{
                                "type": "Feature",
                                "properties": {
                                    "description": "<strong>Pick up station</strong>"
                                },
                                "geometry": {
                                    "type": "Point",
                                    "coordinates": [path[i][0],path[i][1]]
                                }
                            }]
                        }
                    },
                    "layout": {
                        "icon-image": icon,
                        "icon-allow-overlap": true
                    }
                });
            }
            
    map.addLayer({
        'id': 'route1',
        'type': 'line',
        'source': {
            'type': 'geojson',
            'data': {
                'type': 'Feature',
                'properties': {},
                'geometry': {
                    'type': 'LineString',
                    'coordinates': route1.routes[0].geometry.coordinates
                }
            }
        },
        'layout': {
            'line-join': 'round',
            'line-cap': 'round'
        },
        'paint': {
            'line-color': '#3887be',
            'line-width': 2,
            'line-opacity': 0.75
        }
        });
    map.addLayer({
        'id': 'route2',
        'type': 'line',
        'source': {
            'type': 'geojson',
            'data': {
                'type': 'Feature',
                'properties': {},
                'geometry': {
                    'type': 'LineString',
                    'coordinates': route2.routes[0].geometry.coordinates
                }
            }
        },
        'layout': {
            'line-join': 'round',
            'line-cap': 'round'
        },
        'paint': {
            'line-color': '#cf0029',
            'line-width': 5,
            'line-opacity': 0.9
        }
        });
        map.addLayer({
            'id': 'route3',
            'type': 'line',
            'source': {
                'type': 'geojson',
                'data': {
                    'type': 'Feature',
                    'properties': {},
                    'geometry': {
                        'type': 'LineString',
                        'coordinates': route3.routes[0].geometry.coordinates
                    }
                }
            },
            'layout': {
                'line-join': 'round',
                'line-cap': 'round'
            },
            'paint': {
                'line-color': '#3887be',
                'line-width': 2,
                'line-opacity': 0.75
            }
            });
        map.on('click', 'places', function (e) {
            var coordinates = e.features[0].geometry.coordinates.slice();
            var description = e.features[0].properties.description;
            while (Math.abs(e.lngLat.lng - coordinates[0]) > 180) {
                coordinates[0] += e.lngLat.lng > coordinates[0] ? 360 : -360;
            }

            new mapboxgl.Popup()
                .setLngLat(coordinates)
                .setHTML(description)
                .addTo(map);
        });
        map.on('mouseenter', 'places', function () {
            map.getCanvas().style.cursor = 'pointer';
        });
        
        map.on('mouseleave', 'places', function () {
            map.getCanvas().style.cursor = '';
        });
    });   
}