using System;
using System.ComponentModel.DataAnnotations;

namespace Tegetgram.Api.DTOs
{
    public class MessageDTO
    {
        [Required]
        public string ToUserName { get; set; }

        [Required]
        public string Text { get; set; }
    }
}
