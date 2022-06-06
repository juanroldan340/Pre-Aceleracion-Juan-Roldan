using DisneyAPI.Models;
using DisneyAPI.Services;
using DisneyAPI.ViewModels.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DisneyAPI.Controllers
{
    [ApiController]
    [Route("auth")]
    [AllowAnonymous]

    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMailService _mail;
        private readonly IConfiguration _config;

        public AuthenticationController(UserManager<User> userManager, SignInManager<User> signInManager, IMailService mail, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mail = mail;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            var list = await _userManager.FindByNameAsync(model.Username);

            if (list != null)
                return BadRequest();

            var user = new User()
            {
                UserName = model.Username,
                Email = model.Email
            };

            var userCreated = await _userManager.CreateAsync(user, model.Password);

            if (!userCreated.Succeeded)
                return StatusCode(500, new Response {
                    Status = "Error",
                    Message = $"Failed to create user! Error: {userCreated.Errors.Select(e => e.Description)}"
                });

            await _mail.SendWelcomeMail(model.Email);

            return StatusCode(201, new Response
            {
                Status = "Created",
                Message = "User created correctly!"
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest model) 
        {
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);

            if (result.Succeeded) {
                var currentUser = await _userManager.FindByNameAsync(model.Username);

                if (currentUser.IsActive) 
                {
                    return Ok(await GetToken(currentUser));
                }
            }

            return StatusCode(StatusCodes.Status401Unauthorized, new Response { Status = "Error", Message = "You are unauthorized." });
        }

        private async Task<LoginResponse> GetToken(User currentUser)
        {
            var userRoles = await _userManager.GetRolesAsync(currentUser);

            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, currentUser.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            authClaims.AddRange(userRoles.Select(u => new Claim(ClaimTypes.Role, u)));

            var authSigingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["SecretKey"]));

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7000",
                audience: "https://localhost:7000",
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigingKey, SecurityAlgorithms.HmacSha256)
            );

            return new LoginResponse {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ValidTo = token.ValidTo
            };
        }
    }
}
