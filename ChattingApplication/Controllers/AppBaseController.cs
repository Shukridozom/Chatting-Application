using AutoMapper;
using ChattingApplication.Core.EmailService;
using ChattingApplication.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChattingApplication.Core.Domains;
using System.Security.Claims;
using System.Text.Json.Nodes;

namespace ChattingApplication.Controllers
{
    public class AppBaseController : ControllerBase
    {
        protected readonly IConfiguration _config;
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;
        protected readonly IEmailService _mailService;

        public AppBaseController(IConfiguration config, IUnitOfWork unitOfWork, IMapper mapper, IEmailService mailService)
        {
            _config = config;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mailService = mailService;
        }

        protected User GetUserCredentials()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                return null;

            var userClaims = identity.Claims;
            return new User
            {
                Id = Convert.ToInt32(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value),
                Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                Username = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.GivenName)?.Value,
                IsVerified = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value == "Verified"
            };
        }

        protected JsonObject GenerateJsonErrorResponse(string property, string message)
        {
            var response = new JsonObject();
            var errors = new JsonObject();
            var messages = new JsonArray();
            messages.Add(message);
            errors.Add(property, messages);
            response.Add("errors", errors);

            return response;
        }


    }
}
