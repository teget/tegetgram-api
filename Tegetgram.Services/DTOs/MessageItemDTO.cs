using System;
namespace Tegetgram.Services.DTOs
{
    public class MessageItemDTO
    {
        public Guid Id { get; set; }

        public string From { get; set; }

        public string To{ get; set; }
    }
}
