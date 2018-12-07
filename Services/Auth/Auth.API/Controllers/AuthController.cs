using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Auth.API.Data;
using Auth.API.Models;
using Auth.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Auth.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;
        //private IOptions<Audience> _settings;

        public AuthController(UserManager<ApplicationUser> userManager, ILogger<AuthController> logger, SignInManager<ApplicationUser> signInManager,IConfiguration config, ApplicationDbContext context, IOptions<Audience> settings)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._logger = logger;
            this._config = config;
            this._context = context;
            //this._settings = settings;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                DateTime nowDate = DateTime.UtcNow;

                if (result.Succeeded)
                {
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                    var now = DateTime.UtcNow;
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64),
                        new Claim(JwtRegisteredClaimNames.Aud,"employee"),
                        new Claim(JwtRegisteredClaimNames.Aud,"department")
                    };

                    var tokeOptions = new JwtSecurityToken(
                        issuer: _config["Tokens:Issuer"],
                        claims: claims,
                        notBefore:nowDate,
                        expires: nowDate.AddMinutes(100),
                        signingCredentials: signinCredentials
                    );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                    return Ok(new { Token = tokenString });
                }
            }

            return Unauthorized();
        }
    }

    public class Audience
    {
        public string Secret { get; set; }
        public string Iss { get; set; }
        public string Aud { get; set; }
    }
}