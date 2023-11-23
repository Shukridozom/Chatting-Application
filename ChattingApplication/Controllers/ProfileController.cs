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
            var user = GetUserCredentials();
            var getProfileDto = _mapper.Map<User, GetProfileDto>(user);
            return Ok(getProfileDto);
        }

        [HttpPost]
        public IActionResult Post(EditProfileDto profile)
        {
            var usernameFromDb = _unitOfWork.Users.SingleOrDefault
                (u => u.Username == profile.Username && u.Email == profile.Email);
            if(usernameFromDb != null)
            {
                if (usernameFromDb.Email == profile.Email)
                    return BadRequest("This Email address is used");
                else
                    return BadRequest("This username is used");
            }

            User user = GetUserCredentials();
            user = _mapper.Map<EditProfileDto, User>(profile);
         
            _unitOfWork.Complete();
            
            return Ok(_mapper.Map<User, GetProfileDto>(user));
        }
    }
}
