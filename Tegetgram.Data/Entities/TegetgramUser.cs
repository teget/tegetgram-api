using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Tegetgram.Data.Entities
{
    public class TegetgramUser : BaseEntity
    {
        //public virtual ICollection<Conversation> Conversations { get; set; }

        public virtual ICollection<Message> SentMessages { get; set; }

        public virtual ICollection<Message> Inbox { get; set; }

        public virtual ICollection<UserBlocking> BlockedByUsers { get; set; }

        public virtual ICollection<UserBlocking> BlockingUsers { get; set; }

        public string OwnerId { get; set; }

        public virtual ApiUser Owner { get; set; }
    }
}
