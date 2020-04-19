using System;
using System.Collections.Generic;

namespace Tegetgram.Services.DTOs
{
    public class TegetgramUserDTO
    {
        public string UserName { get; set; }

        //public ICollection<MessageItemDTO> SentMessages { get; set; }

        //public ICollection<MessageItemDTO> Inbox { get; set; }

        public ICollection<MessageItemDTO> Messages { get; set; }

        public ICollection<TegetgramUserItemDTO> BlockedUsers { get; set; }
    }
}
