using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ChatApplication.Domain.Entities
{
    public class User : IdentityUser
    {
        public ICollection<Message> Messages { get; set; }
        //public ICollection<Message> MessagesSent { get; set; }
        //public ICollection<Message> ReceivedMessages { get; set; }
    }
}
