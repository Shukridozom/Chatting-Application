$(document).ready(function() {
    let domain = 'https://' + window.location.host;
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
                if(isVerified) {
                    $("#nav-item-register").remove();
                    $("#nav-item-login").remove();
                    $("#nav-item-reset-password").remove();
                    $("body").css("visibility", "visible");
                }
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