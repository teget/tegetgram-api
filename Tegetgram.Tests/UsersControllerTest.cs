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
using Tegetgram.Services.Interfaces;
using Xunit;

namespace Tegetgram.Tests
{
    public class UsersControllerTest
    {
        [Fact]
        public async Task GetUser_Happy()
        {
            var userService = new Mock<IUserService>();
            var userName = "TegetTest";
            var user = new GenericPrincipal(new GenericIdentity(userName), new string[] { });
            var httpContext = new DefaultHttpContext() { User = user };
            var controllerContext = new ControllerContext() { HttpContext = httpContext };
            var controller = new UsersController(null, null, userService.Object, null, null) { ControllerContext = controllerContext };

            OkObjectResult result = await controller.Get(userName) as OkObjectResult;

            userService.Verify(x => x.GetUser(userName, userName));
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
            OkObjectResult result = await controller.Get(credentials) as OkObjectResult;

            userService.Verify(x => x.GetUser(credentials.UserName, credentials.UserName));
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task Block_Happy()
        {
            var userService = new Mock<IUserService>();
            var userName = "TegetTest";
            var user = new GenericPrincipal(new GenericIdentity(userName), new string[] { });
            var httpContext = new DefaultHttpContext() { User = user };
            var controllerContext = new ControllerContext() { HttpContext = httpContext };
            var controller = new UsersController(null, null, userService.Object, null, null) { ControllerContext = controllerContext };

            NoContentResult result = await controller.Block(userName) as NoContentResult;

            userService.Verify(x => x.BlockUser(userName, userName));
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Unblock_Happy()
        {
            var userService = new Mock<IUserService>();
            var userName = "TegetTest";
            var user = new GenericPrincipal(new GenericIdentity(userName), new string[] { });
            var httpContext = new DefaultHttpContext() { User = user };
            var controllerContext = new ControllerContext() { HttpContext = httpContext };
            var controller = new UsersController(null, null, userService.Object, null, null) { ControllerContext = controllerContext };

            NoContentResult result = await controller.Block(userName) as NoContentResult;

            userService.Verify(x => x.BlockUser(userName, userName));
            Assert.NotNull(result);
        }
    }
}
