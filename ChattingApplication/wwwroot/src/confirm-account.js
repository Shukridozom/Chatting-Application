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
                                $('#btn-confirm-account').prop('disabled', false);
                                $("#btn-resend-code").prop("disabled", false);
                            }
                        };
                        
                        $.ajax(settings);
                        $('#btn-confirm-account').prop('disabled', true);
                        $("#btn-resend-code").prop("disabled", true);

                        event.preventDefault();
                    });

                    $('#btn-resend-code').on('click', (event) => {
                        var settings = {
                            "url":  domain + `/api/requestCode?email=${res.email}`,
                            "method": "POST",
                            "timeout": 0,
                            "headers": {
                              "Content-Type": "application/json"
                            },
                            "complete": function(response) {
                                switch (response.status) {
                                    case 200:
                                        $(".validation-message").remove();
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
                                    $("#btn-resend-code").prop("disabled", false);
                                }
                            };
                            
                            $.ajax(settings);
                            $("#btn-request-code").prop("disabled", true);
                            $("#btn-resend-code").prop("disabled", true);
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