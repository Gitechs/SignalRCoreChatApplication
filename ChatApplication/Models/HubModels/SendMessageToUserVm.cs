using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApplication.Models.HubModels
{
    public class SendMessageToUserVm
    {
        public string ReceiverId { get; set; }
        public string Text { get; set; }
        public string ReceiverUserName { get; set; }
    }
}
