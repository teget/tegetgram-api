using System;
using System.ComponentModel.DataAnnotations;

namespace Tegetgram.Api.RequestModels
{
    public class MessageRequestModel
    {
        [Required]
        public string ToUserName { get; set; }

        [Required]
        public string Text { get; set; }
    }
}
