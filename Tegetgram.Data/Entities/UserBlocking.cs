using System;
namespace Tegetgram.Data.Entities
{
    public class UserBlocking
    {
        public Guid BlockerId { get; set; }
        public virtual TegetgramUser Blocker { get; set; }

        public Guid BlockedId { get; set; }
        public virtual TegetgramUser Blocked { get; set; }
    }
}
