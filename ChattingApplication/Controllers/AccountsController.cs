using AutoMapper;
using ChattingApplication.Core;
using ChattingApplication.Core.Domains;
using ChattingApplication.Core.EmailService;
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
    [Route("api")]
    [ApiController]
    public class AccountsController : AppBaseController
    {
        public AccountsController(IConfiguration config, IUnitOfWork unitOfWork, IMapper mapper, IEmailService _mailService)
            : base(config, unitOfWork, mapper, _mailService)
        {
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

            // Creating a confirmation code for the newly registered user:
            var code = CreateOrUpdateVerificationCode(user.Id);

            if(code != null)
                _mailService.SendConfirmationCode
                    (user.Email, user.FirstName + " " + user.LastName, code, EmailType.ConfirmAccount);

            return Ok();
        }

        [HttpPost("confirmAccount")]
        [Authorize]
        public IActionResult ConfirmAccount(string code)
        {
            var userCredentials = GetUserCredentials();
            var confirmationCode = _unitOfWork.ConfirmationCodes
                .SingleOrDefault(cc => cc.UserId == userCredentials.Id);

            if (confirmationCode == null)
                return BadRequest("This code is not valid anymore, request a new one");

            if (confirmationCode.ExpireDate < DateTime.Now || confirmationCode.Trials == 0)
                return BadRequest("This code is not valid anymore, request a new one");

            if (confirmationCode.Code != code)
            {
                confirmationCode.Trials--;
                _unitOfWork.Complete();

                return BadRequest("Unvalid confirmation code");
            }

            confirmationCode.Trials = 0;
            var userFromDb = _unitOfWork.Users.SingleOrDefault(u => u.Id == userCredentials.Id);
            userFromDb.IsVerified = true;
            _unitOfWork.Complete();

            return Ok("Account has been activated");
        }

        [HttpPost("resetPassword")]
        [AllowAnonymous]
        public IActionResult ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var user = _unitOfWork.Users.SingleOrDefault(u => u.Email == resetPasswordDto.Email);

            if (user == null)
                return BadRequest("Unavailable Email address");

            var confirmationCode = _unitOfWork.ConfirmationCodes
                .SingleOrDefault(cc => cc.UserId == user.Id);

            if (confirmationCode == null)
                return BadRequest("This code is not valid anymore, request a new one");

            if (confirmationCode.ExpireDate < DateTime.Now || confirmationCode.Trials == 0)
                return BadRequest("This code is not valid anymore, request a new one");

            if (confirmationCode.Code != resetPasswordDto.Code)
            {
                confirmationCode.Trials--;
                _unitOfWork.Complete();

                return BadRequest("Unvalid confirmation code");
            }

            confirmationCode.Trials = 0;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.Password);
            _unitOfWork.Complete();

            return Ok("Password has been reset");
        }

        [HttpPost("requestCode")]
        [AllowAnonymous]
        public IActionResult RequestCode(string email)
        {
            var user = _unitOfWork.Users.SingleOrDefault(u => u.Email == email);
            if (user == null)
                return BadRequest("Unavailable Email ");

            var code = CreateOrUpdateVerificationCode(user.Id);

            if (code != null)
                _mailService.SendConfirmationCode
                    (user.Email, user.FirstName + " " + user.LastName, code, EmailType.ConfirmAccount);

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
                new Claim(ClaimTypes.Role, user.IsVerified?RoleName.Verified:RoleName.Unverified)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:ValidForInMin"])),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //private User GetUserCredentials()
        //{
        //    var identity = HttpContext.User.Identity as ClaimsIdentity;

        //    if (identity == null)
        //        return null;

        //    var userClaims = identity.Claims;
        //    return new User
        //    {
        //        Id = Convert.ToInt32(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value),
        //        Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
        //        Username = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.GivenName)?.Value,
        //        IsVerified = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value == "Verified"
        //    };
        //}

        private string CreateOrUpdateVerificationCode(int userId)
        {
            var code = GenerateRandomString(Convert.ToInt32(_config["ConfirmationCodes:Length"]));
            var confirmationCode = _unitOfWork.ConfirmationCodes
                .SingleOrDefault(cc => cc.UserId == userId);
            if (confirmationCode == null)
            {
                var newConfirmationCode = new ConfirmationCode()
                {
                    UserId = userId,
                    Code = code,
                    ExpireDate = DateTime.Now.AddMinutes(Convert.ToInt32(_config["ConfirmationCodes:ExpireDurationInMinutes"])),
                    Trials = Convert.ToByte(_config["ConfirmationCodes:Trials"]),
                    RemainingCodesForThisDay = 
                    (byte)(Convert.ToInt32(_config["ConfirmationCodes:RemainingNumberOfCodesForThisDay"]) - 1)
                };
                _unitOfWork.ConfirmationCodes.Add(newConfirmationCode);
            }
            else
            {
                if (confirmationCode.RemainingCodesForThisDay > 0)
                {
                    confirmationCode.Code = code;
                    confirmationCode.ExpireDate = DateTime.Now.AddMinutes(Convert.ToInt32(_config["ConfirmationCodes:ExpireDurationInMinutes"]));
                    confirmationCode.Trials = Convert.ToByte(_config["ConfirmationCodes:Trials"]);
                    confirmationCode.RemainingCodesForThisDay--;

                }
                else
                    return null;
            }

            _unitOfWork.Complete();

            return code;
        }

        private string GenerateRandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
