$(document).ready(function() {
    let domain = "https://localhost:7000"
    var settings = {
        "url": domain + "/api/Profile",
        "method": "GET",
        "timeout": 0,
        "headers": {
            "Authorization": "Bearer " + window.localStorage.getItem('access-token')
    },
        "statusCode": {
            401: function(){
                window.location.replace(domain + "/login.html")
            },
            200: function(res){
                let isVerified = res.isVerified;
                if(isVerified)
                    $("main").css("visibility", "visible");
                else
                    window.location.replace(domain + "/confirm-account.html");
            }
        }
      };
      
      $.ajax(settings);


      $("#nav-item-logout").on('click', function(event) {
        window.localStorage.clear();
        window.location.replace(domain + '/login.html');
        event.preventDefault();
        });
});