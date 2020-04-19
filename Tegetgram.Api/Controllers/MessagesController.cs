using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tegetgram.Api.RequestModels;
using Tegetgram.Api.Filters;
using Tegetgram.Data.Entities;
using Tegetgram.Services.Interfaces;
using Tegetgram.Services.DTOs;
using System.Collections.Generic;

namespace Tegetgram.Api.Controllers
{
    [Authorize]
    [ValidateModel]
    [ApiController]
    [Route("api/[controller]/{id}")]
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
            ICollection<MessageItemDTO> messages = await _messageService.GetMessages(userName);
            return Ok(messages);
        }

        [HttpGet]
        [ActionName("GetMessage")]
        public async Task<IActionResult> Get(Guid id)
        {
            string userName = User.Identity.Name;
            MessageDTO message = await _messageService.GetMessage(userName, id);
            return Ok(message);
        }

        [HttpPost]
        [ActionName("SendMessage")]
        public async Task<IActionResult> Post([FromBody] MessageRequestModel message)
        {
            string sendingUserName = User.Identity.Name;
            await _messageService.SendMessage(sendingUserName, message.ToUserName, message.Text);
            return Ok();
            //return Created();
        }
    }
}