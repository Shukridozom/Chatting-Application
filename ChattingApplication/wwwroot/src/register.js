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

                $('#register-form').on('submit', function(event) {
                    var settings = {
                        "url":  domain + "/api/register",
                        "method": "POST",
                        "timeout": 0,
                        "headers": {
                          "Content-Type": "application/json"
                        },
                        "data": JSON.stringify({
                          "email": $("#email").val(),
                          "username": $("#username").val(),
                          "password": $("#password").val(),
                          "confirmPassword": $("#confirmPassword").val(),
                          "firstName": $("#firstName").val(),
                          "lastName": $("#lastName").val(),
                          "dateOfBirth": $("#dateOfBirth").val()
                        }),
                        "complete": function(response) {
                            switch (response.status) {
                                case 200:
                                    window.location.replace(domain + "/confirm-account.html");
                                    break;
                                default:
                                    $(".validation-message").remove();
                                    var errors = JSON.parse(response.responseText);
                                    if('errors' in errors) {
                                        for(let key in errors.errors){
                                            console.log(`#${key}`);
                                            console.log(`${errors.errors[key][0]}`);
                                            $(`#${key}`).after(`<small class="form-text text-muted validation-message" style="color: red !important;">${errors.errors[key][0]}</small>`);
                                        }
                                    }
                                    break;
                                }
                            $('#btn-register').prop('disabled', false);
                        }
                      };
                      
                      $.ajax(settings);
                      $('#btn-register').prop('disabled', true);

                    event.preventDefault();
                  });
            },
            200: function(res){
                let isVerified = res.isVerified;
                if(isVerified)
                    window.location.replace(domain + "/index.html")
                else
                    window.location.replace(domain + "/confirm-account.html");
            }
        }
      };
      
      $.ajax(settings);

});