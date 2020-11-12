using System;

namespace ChatApplication.Models.HubModels {
    public class LoginByEmailVm {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginbyPhoneNmberVm
    {
        public string PhoneNumber { get; set; }
    }
}