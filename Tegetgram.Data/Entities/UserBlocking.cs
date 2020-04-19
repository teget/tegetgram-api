using System;
namespace Tegetgram.Data.Entities
{
    public class UserBlocking
    {
        public Guid BlockerId { get; set; }
        public TegetgramUser Blocker { get; set; }

        public Guid BlockedId { get; set; }
        public TegetgramUser Blocked { get; set; }
    }
}
