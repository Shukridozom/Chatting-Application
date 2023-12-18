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
                $("#nav-item-logout").remove();
                $("body").css("visibility", "visible");

                $('#request-code-form').on('submit', function(event) {
                    var settings = {
                        "url":  domain + `/api/requestCode?email=${$("#email").val()}`,
                        "method": "POST",
                        "timeout": 0,
                        "headers": {
                          "Content-Type": "application/json"
                        },
                        "complete": function(response) {
                            switch (response.status) {
                                case 200:
                                    window.location.replace(domain + "/reset-password.html");
                                    break;
                                default:
                                    $(".validation-message").remove();
                                    var errors = JSON.parse(response.responseText);
                                    if('errors' in errors) {
                                        for(let key in errors.errors) {
                                            if(key === 'alert') {
                                                $(".request-code-alert").css("bottom", "0rem");
                                                setTimeout(function(){
                                                    $(".request-code-alert").css("bottom", "-10rem");
                                                },2000);
                                                
                                                continue;
                                            }
                                            $(`#${key}`).after(`<small class="form-text text-muted validation-message" style="color: red !important;">${errors.errors[key][0]}</small>`);
                                        }
                                    }
                                    break;
                                }
                                $("#btn-request-code").prop("disabled", false);
                            }
                        };
                        
                        $.ajax(settings);
                        $("#btn-request-code").prop("disabled", true);

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