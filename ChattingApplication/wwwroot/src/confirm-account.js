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
                    window.replace(domain + "/index.html");
                else
                    $("main").css("visibility", "visible");
            }
        }
      };
      
      $.ajax(settings);
});