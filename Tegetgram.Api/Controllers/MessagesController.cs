using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tegetgram.Api.DTOs;
using Tegetgram.Api.Filters;
using Tegetgram.Data.Entities;
using Tegetgram.Services.Interfaces;

namespace Tegetgram.Api.Controllers
{
    [Authorize]
    [ValidateModel]
    [ApiController]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(GlobalExceptionFilter))]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet]
        [ActionName("GetMessages")]
        public async Task<IActionResult> Get()
        {
            string userName = User.Identity.Name;
            ICollection<Message> messages = await _messageService.GetMessages(userName);
            return Ok(messages);
        }

        [HttpPost]
        [ActionName("SendMessage")]
        public async Task<IActionResult> Post([FromBody] MessageDTO message)
        {
            string sendingUserName = User.Identity.Name;
            await _messageService.SendMessage(sendingUserName, message.ToUserName, message.Text);
            return Ok();
            //return Created();
        }
    }
}