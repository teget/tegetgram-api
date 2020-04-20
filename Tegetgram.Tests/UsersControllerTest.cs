using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Tegetgram.Api.Controllers;
using Tegetgram.Api.Models;
using Tegetgram.Api.RequestModels;
using Tegetgram.Data.Entities;
using Tegetgram.Services.DTOs;
using Tegetgram.Services.Interfaces;
using Xunit;

namespace Tegetgram.Tests
{
    public class UsersControllerTest
    {
        [Fact]
        public async Task GetUser_Happy()
        {
            IOptions<JWTSettings> options = Options.Create(new JWTSettings
            {
                SecretKey = "myverysecretkeyofsecrets",
                Issuer = "theissuer"
            });
            var userStore = new Mock<IUserStore<ApiUser>>();
            var userManager = new Mock<UserManager<ApiUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            var contextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<ApiUser>>();
            var signInManager = new Mock<SignInManager<ApiUser>>(userManager.Object, contextAccessor.Object, userPrincipalFactory.Object, null, null, null);
            var userService = new Mock<IUserService>();

            var controller = new UsersController(userManager.Object, signInManager.Object, userService.Object, null, options);
            var userName = "TegetTest";
            var user = new ClaimsPrincipal(new GenericIdentity(userName));
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            userService.Setup(x => x.GetUser(userName, userName))
                .ReturnsAsync(new TegetgramUserDTO { UserName = userName });

            OkObjectResult result = await controller.Get(userName) as OkObjectResult;

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task Authenticate_Happy()
        {
            var credentials = new UserRequestModel()
            {
                UserName = "TegetTest",
                Password = "TestPwd123"
            };
            IOptions<JWTSettings> options = Options.Create(new JWTSettings
            {
                SecretKey = "myverysecretkeyofsecrets",
                Issuer = "theissuer"
            });
            var userStore = new Mock<IUserStore<ApiUser>>();
            var userManager = new Mock<UserManager<ApiUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            var contextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<ApiUser>>();
            userManager.Setup(x => x.FindByNameAsync(credentials.UserName))
                .ReturnsAsync(new ApiUser { UserName = credentials.UserName });
            var signInManager = new Mock<SignInManager<ApiUser>>(userManager.Object, contextAccessor.Object, userPrincipalFactory.Object, null, null, null);
            signInManager.Setup(x => x.CheckPasswordSignInAsync(It.Is<ApiUser>(u => u.UserName == credentials.UserName), credentials.Password, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
            var userService = new Mock<IUserService>();

            var controller = new UsersController(userManager.Object, signInManager.Object, userService.Object, null, options);
            OkObjectResult result = await controller.Get(credentials) as OkObjectResult;

            userService.Verify(x => x.GetUser(credentials.UserName, credentials.UserName));
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task Register_Happy()
        {
            var credentials = new UserRequestModel()
            {
                UserName = "TegetTest",
                Password = "TestPwd123"
            };
            IOptions<JWTSettings> options = Options.Create(new JWTSettings
            {
                SecretKey = "myverysecretkeyofsecrets",
                Issuer = "theissuer"
            });
            var userStore = new Mock<IUserStore<ApiUser>>();
            var userManager = new Mock<UserManager<ApiUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            var contextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<ApiUser>>();
            userManager.Setup(x => x.CreateAsync(It.Is<ApiUser>(u => u.UserName == credentials.UserName), credentials.Password))
                .ReturnsAsync(IdentityResult.Success);
            var signInManager = new Mock<SignInManager<ApiUser>>(userManager.Object, contextAccessor.Object, userPrincipalFactory.Object, null, null, null);
            var userService = new Mock<IUserService>();

            var controller = new UsersController(userManager.Object, signInManager.Object, userService.Object, null, options);
            OkObjectResult result = await controller.Post(credentials) as OkObjectResult;

            userService.Verify(x => x.GetUser(credentials.UserName, credentials.UserName));
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task Block_Happy()
        {
            IOptions<JWTSettings> options = Options.Create(new JWTSettings
            {
                SecretKey = "myverysecretkeyofsecrets",
                Issuer = "theissuer"
            });
            var userStore = new Mock<IUserStore<ApiUser>>();
            var userManager = new Mock<UserManager<ApiUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            var contextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<ApiUser>>();
            var signInManager = new Mock<SignInManager<ApiUser>>(userManager.Object, contextAccessor.Object, userPrincipalFactory.Object, null, null, null);
            var userService = new Mock<IUserService>();

            var controller = new UsersController(userManager.Object, signInManager.Object, userService.Object, null, options);
            var userName = "TegetTest";
            var user = new ClaimsPrincipal(new GenericIdentity(userName));
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            NoContentResult result = await controller.Block(userName) as NoContentResult;

            userService.Verify(x => x.BlockUser(userName, userName));
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Unblock_Happy()
        {
            IOptions<JWTSettings> options = Options.Create(new JWTSettings
            {
                SecretKey = "myverysecretkeyofsecrets",
                Issuer = "theissuer"
            });
            var userStore = new Mock<IUserStore<ApiUser>>();
            var userManager = new Mock<UserManager<ApiUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            var contextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<ApiUser>>();
            var signInManager = new Mock<SignInManager<ApiUser>>(userManager.Object, contextAccessor.Object, userPrincipalFactory.Object, null, null, null);
            var userService = new Mock<IUserService>();

            var controller = new UsersController(userManager.Object, signInManager.Object, userService.Object, null, options);
            var userName = "TegetTest";
            var user = new ClaimsPrincipal(new GenericIdentity(userName));
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            NoContentResult result = await controller.Unblock(userName) as NoContentResult;

            userService.Verify(x => x.UnblockUser(userName, userName));
            Assert.NotNull(result);
        }
    }
}
