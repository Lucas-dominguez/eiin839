
function sendRequestToRoutingWS(){
    var caller = new XMLHttpRequest();
    var request = "http://localhost:8734/Design_Time_Addresses/Routing/rest/route?start=" 
    + document.getElementById("depart").value + "&end=" + document.getElementById("arrivee").value;
    console.log("Sending request to Proxy : " + request)
    caller.open("GET", request, true);
    caller.setRequestHeader ("Accept", "application/json");
    caller.onload=displayPath;
    caller.send();
}


function displayPath(){
    var path = JSON.parse(this.responseText).GetRoutingResult;
    console.log(path);
    var s = "Tu dois passer par ...\n";
    for(var i=0; i< path.length; i++){
        s += path[i].address;
        s += " puis ... \n";
    }
    s+="arrivée !!";
    document.getElementById("path").innerHTML = s;
}