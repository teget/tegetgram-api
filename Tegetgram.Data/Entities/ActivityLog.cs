using System;
namespace Tegetgram.Data.Entities
{
    public class ActivityLog : BaseEntity
    {
        public Guid UserId { get; set; }

        public string ActionName { get; set; }

        public string Message { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
