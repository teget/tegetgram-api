using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Tegetgram.Api.RequestModels;
using Tegetgram.Api.Filters;
using Tegetgram.Api.Models;
using Tegetgram.Data.Entities;
using Tegetgram.Services.Interfaces;

namespace Tegetgram.Api.Controllers
{
    [Authorize]
    [ValidateModel]
    [ApiController]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(GlobalExceptionFilter))]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly SignInManager<ApiUser> _signInManager;
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;
        private readonly JWTSettings _options;

        public UsersController(UserManager<ApiUser> userManager,
                               SignInManager<ApiUser> signInManager,
                               IUserService userService,
                               ILogger<UsersController> logger,
                               IOptions<JWTSettings> optionsAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _logger = logger;
            _options = optionsAccessor.Value;
        }

        [AllowAnonymous]
        [HttpGet]
        [ActionName("Authenticate")]
        public async Task<IActionResult> Get([FromBody] UserRequestModel credentials)
        {
            ApiUser apiUser = await _userManager.FindByNameAsync(credentials.UserName);
            if (apiUser == null)
                throw new ApplicationException("Invalid user name or password.");

            var result = await _signInManager.CheckPasswordSignInAsync(apiUser, credentials.Password, false);
            if (!result.Succeeded)
                throw new ApplicationException("Invalid user name or password.");

            var now = DateTime.Now;
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.SecretKey));
            var tokenHandler = new JsonWebTokenHandler();
            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _options.Issuer,
                IssuedAt = now,
                NotBefore = now,
                Expires = now.AddHours(1),
                Claims = new Dictionary<string, object> { { JwtRegisteredClaimNames.Sub, apiUser.UserName } },
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };

            return Ok(new { access_token = tokenHandler.CreateToken(descriptor) });
        }

        [AllowAnonymous]
        [HttpPost]
        [ActionName("Register")]
        public async Task<IActionResult> Post([FromBody] UserRequestModel credentials)
        {
            ApiUser apiUser = new ApiUser(credentials.UserName);
            var result = await _userManager.CreateAsync(apiUser, credentials.Password);
            if (!result.Succeeded)
            {
                _logger.LogError($"User creation error: {String.Join(" | ", result.Errors.Select(x => x.Description).ToArray())}");
                throw new ApplicationException("Could not create user.");
            }

            var now = DateTime.Now;
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.SecretKey));
            var tokenHandler = new JsonWebTokenHandler();
            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _options.Issuer,
                IssuedAt = now,
                NotBefore = now,
                Expires = now.AddHours(1),
                Claims = new Dictionary<string, object> { { JwtRegisteredClaimNames.Sub, apiUser.UserName } },
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };

            return Ok(new { access_token = tokenHandler.CreateToken(descriptor) });
        }

        [HttpPut("{username}/block")]
        [ActionName("BlockUser")]
        public async Task<IActionResult> Block(string userName)
        {
            string forUserName = User.Identity.Name;
            await _userService.BlockUser(forUserName, userName);
            //return Created();
            return NoContent();
        }

        [HttpDelete("{username}/block")]
        [ActionName("UnblockUser")]
        public async Task<IActionResult> Unblock(string userName)
        {
            string forUserName = User.Identity.Name;
            await _userService.UnblockUser(forUserName, userName);
            //return Created();
            return NoContent();
        }
    }
}
