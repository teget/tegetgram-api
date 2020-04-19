using System;
namespace Tegetgram.Services.DTOs
{
    public class MessageDTO
    {
        public Guid Id { get; set; }

        public string From { get; set; }

        public string To{ get; set; }

        public string Text { get; set; }

        public bool IsNew { get; set; }

        public DateTime? ReadOn { get; set; }

        public DateTime SentOn { get; set; }
    }
}
