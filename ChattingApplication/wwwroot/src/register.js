$(document).ready(function() {
    let domain = "https://localhost:7000"
    var settings = {
        "url": domain + "/api/Profile",
        "method": "GET",
        "timeout": 0,
        "headers": {
        //   "Authorization": "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjYiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ4ZWdpaGUxNzcyQGRwc29scy5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9naXZlbm5hbWUiOiJzdHJpbmciLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJWZXJpZmllZCIsImV4cCI6MTcwMDkzNzUzOSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzAwMCIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjcwMDAifQ.r9Vx2AummxBNUFdiKOubJ5K_Q1wJAR5hfFvN6WupduY"
        //   "Authorization": "Bearer eyhhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjQiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ2aWNhbjMwOTgzQGZyYW5kaW4uY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZ2l2ZW5uYW1lIjoic3RyaW5nIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiVmVyaWZpZWQiLCJleHAiOjE3MDA5MzUwNDMsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcwMDAiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo3MDAwIn0.QFSVjSBlewNumaAYRCOEyt4KAZXmce3418BBEJC1pEI"
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
                    window.location.replace(domain + "/index.html")
                else
                    window.location.replace(domain + "/confirm-account.html");
            }
        }
      };
      
      $.ajax(settings);

});