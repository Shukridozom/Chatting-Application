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

                $('#reset-password-form').on('submit', function(event) {
                    var settings = {
                        "url":  domain + "/api/resetPassword",
                        "method": "POST",
                        "timeout": 0,
                        "headers": {
                          "Content-Type": "application/json"
                        },
                        "data": JSON.stringify({
                          "code": $("#code").val(),
                          "email": $("#email").val(),
                          "password": $("#password").val(),
                          "confirmPassword": $("#confirmPassword").val()
                        }),
                        "complete": function(response) {
                            switch (response.status) {
                                case 200:
                                    window.location.replace(domain + "/login.html");
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
                            $('#btn-reset-password').prop('disabled', false);
                        }
                      };
                      
                      $.ajax(settings);
                      $('#btn-reset-password').prop('disabled', true);

                    event.preventDefault();
                  });
            },
            200: function(res){
                window.location.replace(domain + "/index.html");
            }
        }
      };
      
      $.ajax(settings);

});