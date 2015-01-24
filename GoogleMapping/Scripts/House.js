var map;

function initialize() {        
    var mapOptions = {
        center: new google.maps.LatLng(53.89, -4.07),
        zoom: 6,
    };
    map = new google.maps.Map(document.getElementById("map-canvas"),
        mapOptions);   
}

google.maps.event.addDomListener(window, 'load', initialize);