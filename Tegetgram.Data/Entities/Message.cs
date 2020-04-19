using System;
namespace Tegetgram.Data.Entities
{
    public class Message : BaseEntity
    {
        public Guid SenderId { get; set; }

        public virtual TegetgramUser Sender { get; set; }

        public Guid RecepientId { get; set; }

        public virtual TegetgramUser Recepient { get; set; }

        public string Text { get; set; }

        public bool IsRead { get; set; }

        public DateTime? ReadOn { get; set; }

        public DateTime SentOn { get; set; }

        //public Guid ConversationId { get; set; }

        //public virtual Conversation Conversation { get; set; }
    }
}
