using System;
namespace ImpressoApp.Models.Authentication
{
    public class SignUpRequestModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int Role { get; set; }
        public string Base64Img { get; set; }
    }
}
