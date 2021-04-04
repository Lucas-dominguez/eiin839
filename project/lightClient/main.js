var stations = [];

key = "ac428b37563fe08de2eeea03fe75f27e4dce458a"

function setApiKey(){
    key = document.getElementById("apikey").value;
}
function retrieveAllContracts() {
    var url = "https://api.jcdecaux.com/vls/v3/contracts?apiKey=" + key;
    sendRequestToApi(url, printContracts);
}

function sendRequestToApi(url, methodName){
    var caller = new XMLHttpRequest();
    var proxyRequest = "http://localhost:8733/Design_Time_Addresses/ProxyCache/GetDelay?request="+url+"&expirationTime=1000";
    console.log("Sending request to Proxy : " + proxyRequest)
    caller.open("GET", proxyRequest, true);
    caller.setRequestHeader ("Accept", "application/json");
    caller.onload=methodName;
    caller.send();
}

function printContracts() {
    var contracts = JSON.parse(this.responseText).GetDelaySecResult;
    contractsList = document.getElementById("contracts-list");
    for(var i=0; i< contracts.length; i++){
        var option=document.createElement("option");
        option.value = contracts[i].name;
        contractsList.appendChild(option);
    }
}

function retrieveContractStations(){
    contractname = document.getElementById("contracts").value
    url = "https://api.jcdecaux.com/vls/v1/stations?contract="+contractname+ "%26apiKey="+key;
    sendRequestToApi(url,printStations);
}

function printStations(){
    stations = JSON.parse(this.responseText).GetDelaySecResult;
    console.log(stations);
    str = '';
    for(var i=0; i< stations.length; i++){
        str+=stations[i].name + " | ";
    }
    document.getElementById("modalTxt").innerHTML= str;
    document.getElementById("modaltitre").innerHTML = "Liste des stations de " + stations[0].contract_name;
    modal.style.display = "block";
}

function nearestStation(){
    nearest = null
    dmin = 1000000000000;
    latitude = document.getElementById("latitude").value
    longitude = document.getElementById("longitude").value
    for(var i=0; i< stations.length; i++){
        d = getDistanceFrom2GpsCoordinates(latitude, longitude, stations[i].position.lat, stations[i].position.lng);
        if (d < dmin){
            dmin = d;
            nearest = stations[i];
        }
    }
    if (nearest == null){
        document.getElementById("NearestStationResult").innerHTML = "----------->   Il n'y a pas de stations proches de chez vous";
    }
    else{
        document.getElementById("NearestStationResult").innerHTML = "----------->   La station la plus proche est " + nearest.name;
    }   
}

function getDistanceFrom2GpsCoordinates(lat1, lon1, lat2, lon2) {
    var earthRadius = 6371;
    var dLat = deg2rad(lat2-lat1);
    var dLon = deg2rad(lon2-lon1);
    var a =
        Math.sin(dLat/2) * Math.sin(dLat/2) +
        Math.cos(deg2rad(lat1)) * Math.cos(deg2rad(lat2)) *
        Math.sin(dLon/2) * Math.sin(dLon/2)
    ;
    var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1-a));
    var d = earthRadius * c;
    return d;
}

function deg2rad(deg) {
    return deg * (Math.PI/180)
}


var modal = document.getElementById("modalStations");
var btn = document.getElementById("affichStations");
var span = document.getElementsByClassName("close")[0];

span.onclick = function() {
  modal.style.display = "none";
}

window.onclick = function(event) {
  if (event.target == modal) {
    modal.style.display = "none";
  }
}