    //using System;
    //using System.Collections.Generic;
    //using System.Linq;
    //using System.Security.Claims;
    //using System.Text;
    //using System.Threading.Tasks;
    //using Microsoft.AspNetCore.Http;
    //using Microsoft.AspNetCore.Identity;
    //using Microsoft.AspNetCore.Mvc;
    //using Microsoft.Extensions.Options;
    //using Microsoft.IdentityModel.Tokens;
    //using Microsoft.IdentityModel.JsonWebTokens;
    //using Tegetgram.Api.DTOs;
    //using Tegetgram.Api.Models;
    //using Tegetgram.Data.Entities;
    //using Tegetgram.Services.Interfaces;

    //namespace Tegetgram.Api.Controllers
    //{
    //    [Route("api/[controller]/[action]")]
    //    [ApiController]
    //    public class AccountController : ControllerBase
    //    {
    //        private readonly UserManager<ApiUser> _userManager;
    //        private readonly SignInManager<ApiUser> _signInManager;
    //        private readonly IUserService _userService;
    //        private readonly JWTSettings _options;

    //        public AccountController(UserManager<ApiUser> userManager, SignInManager<ApiUser> signInManager, IUserService userService, IOptions<JWTSettings> optionsAccessor)
    //        {
    //            _userManager = userManager;
    //            _signInManager = signInManager;
    //            _userService = userService;
    //            _options = optionsAccessor.Value;
    //        }

    //        [HttpPost]
    //        public async Task<IActionResult> SignIn([FromBody] UserDTO credentials)
    //        {
    //            if (ModelState.IsValid)
    //            {
    //                var result = await _signInManager.PasswordSignInAsync(credentials.UserName, credentials.Password, false, false);
    //                if (result.Succeeded)
    //                {
    //                    var apiUser = await _userManager.FindByNameAsync(credentials.UserName);

    //                    var now = DateTime.Now;
    //                    var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.SecretKey));
    //                    var tokenHandler = new JsonWebTokenHandler();
    //                    var descriptor = new SecurityTokenDescriptor
    //                    {
    //                        Issuer = _options.Issuer,
    //                        IssuedAt = now,
    //                        NotBefore = now,
    //                        Expires = now.AddHours(1),
    //                        Claims = new Dictionary<string, object> { { JwtRegisteredClaimNames.Sub, apiUser.UserName } },
    //                        SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
    //                    };

    //                    return Ok(new { access_token = tokenHandler.CreateToken(descriptor)});
    //                }
    //                return new JsonResult("Unable to sign in.") { StatusCode = 401 };
    //            }
    //            return Error("Unexpected error.");
    //        }

    //        [HttpPost]
    //        public async Task<IActionResult> SignUp([FromBody] UserDTO credentials)
    //        {
    //            if (ModelState.IsValid)
    //            {
    //                var apiUser = new ApiUser(credentials.UserName);

    //                var result = await _userManager.CreateAsync(apiUser, credentials.Password);
    //                if (result.Succeeded)
    //                {
    //                    await _userService.CreateAsync(apiUser.UserName, apiUser.Id);
    //                    await _signInManager.SignInAsync(apiUser, false);

    //                    var now = DateTime.Now;
    //                    var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.SecretKey));
    //                    var tokenHandler = new JsonWebTokenHandler();
    //                    var descriptor = new SecurityTokenDescriptor
    //                    {
    //                        Issuer = _options.Issuer,
    //                        IssuedAt = now,
    //                        NotBefore = now,
    //                        Expires = now.AddHours(1),
    //                        Claims = new Dictionary<string, object> { { JwtRegisteredClaimNames.Sub, apiUser.UserName } },
    //                        SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
    //                    };

    //                    return Ok(new { access_token = tokenHandler.CreateToken(descriptor) });
    //                }
    //                return Errors(result);
    //            }
    //            return Error("Unexpected Error.");
    //        }

    //        private JsonResult Error(string message)
    //        {
    //            return new JsonResult(message) { StatusCode = 400 };
    //        }

    //        private JsonResult Errors(IdentityResult result)
    //        {
    //            var items = result.Errors
    //                .Select(x => x.Description)
    //                .ToArray();
    //            return new JsonResult(items) { StatusCode = 400 };
    //        }
    //    }
    //}