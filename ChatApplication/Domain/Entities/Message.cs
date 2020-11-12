using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApplication.Domain.Entities
{
    public class Message
    {
        public Message()
        {
            IsReed = false;
            SendingTime = DateTimeOffset.Now;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [StringLength(2 * 1024)]
        public string Text { get; set; }
        public DateTimeOffset SendingTime { get; set; }
        public bool IsReed { get; set; }

        public string ReceiverId { get; set; }
        public string SenderId { get; set; }

        [ForeignKey(nameof(SenderId))]
        public User User { get; set; }

        //public string SenderUserId { get; set; }

        //[ForeignKey(nameof(SenderUserId))]
        //public User Sender { get; set; }



        //public string ReceiverUserId { get; set; }

        //[ForeignKey(nameof(ReceiverUserId))]
        //public User Receiver { get; set; }
    }
}
