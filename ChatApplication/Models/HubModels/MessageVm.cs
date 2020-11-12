using System;

namespace ChatApplication.Models.HubModels
{
    public class MessageVm
    {
        public string Id { get; set; }
        public string Text { get; set; }

        public string ReceiverId { get; set; }
        public string SenderId { get; set; }
        public DateTimeOffset SendingTime { get; set; }
    }
}
