using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Tegetgram.Api.Controllers;
using Tegetgram.Services.DTOs;
using Tegetgram.Services.Interfaces;
using Xunit;

namespace Tegetgram.Tests
{
    public class MessagesControllerTests
    {
        [Fact]
        public async Task GetMessages_Happy()
        {
            var messageService = new Mock<IMessageService>();
            var controller = new MessagesController(messageService.Object);
            var userName = "TegetTest";
            var user = new ClaimsPrincipal(new GenericIdentity(userName));
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            messageService.Setup(x => x.GetMessages(userName)).ReturnsAsync(new List<MessageItemDTO>());

            OkObjectResult result = await controller.Get() as OkObjectResult;

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task GetMessage_Happy()
        {
            var messageService = new Mock<IMessageService>();
            var controller = new MessagesController(messageService.Object);
            var userName = "TegetTest";
            var user = new ClaimsPrincipal(new GenericIdentity(userName));
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            messageService.Setup(x => x.GetMessage(userName, It.IsAny<Guid>())).ReturnsAsync(new MessageDTO());

            OkObjectResult result = await controller.Get(Guid.NewGuid()) as OkObjectResult;

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
        }
    }
}