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
                $("body").css("visibility", "visible");

                $('#login-form').on('submit', function(event) {
                    var settings = {
                        "url":  domain + "/api/login",
                        "method": "POST",
                        "timeout": 0,
                        "headers": {
                          "Content-Type": "application/json"
                        },
                        "data": JSON.stringify({
                          "username": $("#username").val(),
                          "password": $("#password").val()
                        }),
                        "complete": function(response) {
                            switch (response.status) {
                                case 200:
                                    window.localStorage.setItem('access-token', response.responseText);
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
                            $('#btn-login').prop('disabled', false);
                        }
                      };
                      
                      $.ajax(settings);
                      $('#btn-login').prop('disabled', true);

                    event.preventDefault();
                  });
            },
            200: function(res){
                let isVerified = res.isVerified;
                if(isVerified)
                    window.location.replace(domain + "/index.html");
                else
                    window.location.replace(domain + "/confirm-account.html");
            }
        }
      };
      
      $.ajax(settings);

});


