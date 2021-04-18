
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

function displayInstruction(r){
    tripInstructions = ""
    for(var i =0; i< r.length; i++){
        if(r.length < 2){
            tripInstructions  += "Trajet : <br>"
        }
        else{
            if(i == 0) tripInstructions  += "Trajet jusqu'à la station :<br>"
            if(i == 1) tripInstructions  += "Trajet en vélo :<br>"
            if(i == 2) tripInstructions  += "Trajet en jusqu'à la destination :<br>"
        }
        var steps = r[i].routes[0].legs[0].steps;
        for (var j = 0; j < steps.length; j++) {
            tripInstructions += '<li>' + steps[j].maneuver.instruction + '</li>';
          }
         tripInstructions += "<br><br>"
    }
    document.getElementById("modalTxt").innerHTML = tripInstructions
}

function displayInfos(r){
    s = ""
    t = 0
    d = 0
    if(r.length > 2){
        for(var i = 0 ; i < r.length; i++){
            if(i==0){
                s+= "<br>Station la plus proche : "
            }
            else if(i==1){
                s+= "<br>Distance en vélo : "
            }
            else if(i==2){
                s+= "<br>Distance à pied jusqu'à l'arrivée : "
            }
            tmp = Math.round(r[i].routes[0].distance)/1000
            if(tmp>=1){
                s+= tmp + " km"
            }
            else{
                s+= tmp*1000 + " m"
            }
            d+= tmp;
            s+= "<br>&emsp;Durée : "
            tmp = r[i].routes[0].duration
            s += convertTime(tmp)
            t += tmp
        }
    }
    else{
        d = Math.round(r[0].routes[0].distance)/1000
        t = r[0].routes[0].duration;
    }
    res = "Distance totale : "
    res += d + " km"
    res += "<br>Durée du trajet : "
    res += convertTime(t)
    res += "<br>" + s
    document.getElementById("infos").innerHTML= res;
}

function convertTime(time){
    hours = Math.floor(time / 3600);
    seconds = time - hours
    minutes = Math.floor(seconds / 60);
    seconds = seconds - minutes * 60;
    seconds = Math.round(seconds)
    ppTime = ""
    if(hours !=0){
        ppTime+=str_pad_left(hours,'0',2) +" h " 
    }
    ppTime+= str_pad_left(minutes,'0',2)+' min '+str_pad_left(seconds,'0',2) + " s ";
    return ppTime
}
function str_pad_left(string,pad,length) {
    return (new Array(length+1).join(pad)+string).slice(-length);
}
function displayMap(){
    var data = JSON.parse(this.responseText).GetRoutingMapResult
    if(data.infos!="OK"){
        displayError(data.infos);
        return;
    }
    data = data.routes;
    console.log(data)
    routesForMap = []
    for(var i = 0; i <data.length;i++){
        routesForMap.push(JSON.parse(data[i]))
    }
    start = routesForMap[0].waypoints[0].location
    end = routesForMap[routesForMap.length -1].waypoints[1].location
    path = [start]
    for(var i = 1; i < routesForMap.length - 1;i++){
        path.push(routesForMap[i].waypoints[0].location)
    }
    path.push(end);

    mapboxgl.accessToken = 'pk.eyJ1IjoibHVjYXNwb2x5dGVjaCIsImEiOiJja25lazgxemYyNjZ6MnVtcWNuY2ltMTU5In0.PRHFI72a0818EQvpX_VLzA'; 
    var map = new mapboxgl.Map({
        container: 'map',
        style: 'mapbox://styles/mapbox/streets-v9',
        center: [start[0], start[1]],
        zoom: 11
    });
    
    map.on('load', function () {
         for(var i=0; i<path.length;i++){
                var icon = null;
                if(path.length > 2 && (i == 1 || i == 2)){
                    icon = "bicycle-15"
                }
                else{
                    icon =  "circle-stroked-11"
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
                                    "description": "<strong>Checkpoint</strong>"
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
        for(var i=0; i<routesForMap.length;i++){   
            if(routesForMap[i].routes[0].weight_name == "cyclability"){
                paint = {
                    'line-color': '#cf0029',
                    'line-width': 5,
                    'line-opacity': 0.9
                }
            }
            else{
                paint = {
                    'line-color': '#3887be',
                    'line-width': 2,
                    'line-opacity': 0.75
                }
            }
            map.addLayer({
                'id': "route"+ i,
                'type': 'line',
                'source': {
                    'type': 'geojson',
                    'data': {
                        'type': 'Feature',
                        'properties': {},
                        'geometry': {
                            'type': 'LineString',
                            'coordinates': routesForMap[i].routes[0].geometry.coordinates
                        }
                    }
                },
                'layout': {
                    'line-join': 'round',
                    'line-cap': 'round'
                },
                'paint': paint
                });
        }
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
    displayInfos(routesForMap)
    displayInstruction(routesForMap)
    document.getElementById("affichInfo").style.display = ""
    document.getElementById("error").innerHTML = "";

}


var modal = document.getElementById("modalInfo");
var btn = document.getElementById("affichInfo");
var span = document.getElementsByClassName("close")[0];
btn.onclick = function() {
  modal.style.display = "block";
}

span.onclick = function() {
  modal.style.display = "none";
}

window.onclick = function(event) {
  if (event.target == modal) {
    modal.style.display = "none";
  }
}