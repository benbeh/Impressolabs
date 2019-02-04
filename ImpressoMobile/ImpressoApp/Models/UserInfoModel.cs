using System;
using ImpressoApp.Enums;

namespace ImpressoApp.Models
{
    public class UserInfoModel
    { 
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserType UserType { get; set; }
        public string UserProfileImage { get; set; }
    }
}

