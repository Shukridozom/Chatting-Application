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
                window.location.replace(domain + "/login.html");
            },
            200: function(res){
                let isVerified = res.isVerified;
                if(isVerified)
                    window.replace(domain + "/index.html");
                else {
                    $("#nav-item-register").remove();
                    $("#nav-item-login").remove();
                    $("#nav-item-reset-password").remove();
                    $("body").css("visibility", "visible");

                    $('#confirm-account-form').on('submit', function(event) {
                        var settings = {
                            "url": domain + `/api/confirmAccount?code=${$("#code").val()}`,
                            "method": "POST",
                            "timeout": 0,
                            "headers": {
                              "Authorization": "Bearer " + window.localStorage.getItem('access-token')  
                            },
                            "complete": function(response) {
                                switch(response.status) {
                                    case 200:
                                        window.location.replace(domain + "/index.html");
                                        break;
                                    
                                    default:
                                        $(".validation-message").remove();
                                        var errors = JSON.parse(response.responseText);
                                        if('errors' in errors) {
                                            for(let key in errors.errors)
                                                $(`#${key}`).after(`<small class="form-text text-muted validation-message" style="color: red !important;">${errors.errors[key][0]}</small>`);
                                        }
                                        break;

                                }
                            }
                          };
                          
                          $.ajax(settings);

                        event.preventDefault();
                    });
                }
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