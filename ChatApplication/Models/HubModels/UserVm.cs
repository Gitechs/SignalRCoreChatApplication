namespace ChatApplication.Models.HubModels
{
    public class UserVm
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        public int UnreadMessageCount { get; set; }
        public bool IsOnline { get; set; }
        //public string[] ConnectionIds { get; set; }
    }
}
