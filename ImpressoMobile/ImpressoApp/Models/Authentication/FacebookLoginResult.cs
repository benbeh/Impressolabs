using System;
using ImpressoApp.Enums;

namespace ImpressoApp.Models.Authentication
{
    public class FacebookLoginResult
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public DateTimeOffset ExpireAt { get; set; }
        public FacebookLoginState LoginState { get; set; }
        public string ErrorString { get; set; }
    }
}
