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
                $("#nav-item-logout").remove();
                $("main").css("visibility", "visible");
            },
            200: function(res){
                let isVerified = res.isVerified;
                if(isVerified)
                    window.location.replace(domain + "/login.html");
                else
                    window.location.replace(domain + "/confirm-account.html");
            }
        }
      };
      
      $.ajax(settings);

});


