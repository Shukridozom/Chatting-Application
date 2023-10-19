using AutoMapper;
using ChattingApplication.Core;
using ChattingApplication.Core.Domains;
using ChattingApplication.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChattingApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccountsController(IConfiguration config, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _config = config;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginDto loginCredentials)
        {
            var user = AuthenticateUser(loginCredentials);

            if (user != null)
            {
                var token = GenerateToken(user);
                return Ok(token);
            }

            return NotFound("The username or password is incorrect");
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegisterDto registerDto)
        {
            var userWithSameUsername = _unitOfWork.Users.SingleOrDefault
                (u => u.Username.ToLower() == registerDto.Username.ToLower());
            if (userWithSameUsername != null)
                return BadRequest("Username already exists");

            var userWithSameEmail = _unitOfWork.Users.SingleOrDefault
                (u => u.Email.ToLower() == registerDto.Email.ToLower());
            if (userWithSameEmail != null)
                return BadRequest("Email already exists");


            var user = _mapper.Map<RegisterDto, User>(registerDto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            user.RegisteredDate = DateTime.Now;
            _unitOfWork.Users.Add(user);

            _unitOfWork.Complete();

            return Ok();
        }

        private User AuthenticateUser(LoginDto loginCredentials)
        {
            var user = _unitOfWork.Users.SingleOrDefault(u => u.Username.ToLower()
            == loginCredentials.Username.ToLower());

            if (user == null)
                return null;

            if (BCrypt.Net.BCrypt.Verify(loginCredentials.Password, user.PasswordHash))
                return user;

            return null;
        }

        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.Username),
                new Claim(ClaimTypes.Role, user.IsVerified?"Verified":"Unverified")
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:ValidForInMin"])),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
