using AutoMapper;
using ChattingApplication.Core.EmailService;
using ChattingApplication.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChattingApplication.Dtos;
using ChattingApplication.Core.Domains;

namespace ChattingApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : AppBaseController
    {

        public ProfileController(IConfiguration config, IUnitOfWork unitOfWork, IMapper mapper, IEmailService mailService)
            : base(config, unitOfWork, mapper, mailService)
        {

        }

        [HttpGet]
        public IActionResult Get()
        {
            var credentials = GetUserCredentials();
            var userFromDb = _unitOfWork.Users.SingleOrDefault(u => u.Id == credentials.Id);
            var getProfileDto = _mapper.Map<User, GetProfileDto>(userFromDb);
            return Ok(getProfileDto);
        }

        [HttpPost]
        public IActionResult Post(EditProfileDto profile)
        {
            var credentials = GetUserCredentials();

            User userFromDb = _unitOfWork.Users.SingleOrDefault(u => u.Id == credentials.Id);
           _mapper.Map(profile,userFromDb);
         
            _unitOfWork.Complete();
            
            return Ok(_mapper.Map<User, GetProfileDto>(userFromDb));
        }
    }
}
