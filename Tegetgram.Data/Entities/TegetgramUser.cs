using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Tegetgram.Data.Entities
{
    public class TegetgramUser : BaseEntity
    {
        //public ICollection<Conversation> Conversations { get; set; }

        public virtual ICollection<Message> SentMessages { get; set; }

        public virtual ICollection<Message> Inbox { get; set; }

        public virtual ICollection<UserBlocking> BlockedByUsers { get; set; }

        public virtual ICollection<UserBlocking> BlockingUsers { get; set; }

        public string OwnerId { get; set; }

        public virtual ApiUser Owner { get; set; }

        [NotMapped]
        public ICollection<Message> Messages
        {
            get { return SentMessages.Concat(Inbox).ToList(); }
        }
    }
}
