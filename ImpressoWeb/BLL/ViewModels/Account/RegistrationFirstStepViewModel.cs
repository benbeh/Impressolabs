using System.ComponentModel.DataAnnotations;
using Core.Enum;
using Newtonsoft.Json.Serialization;

namespace BLL.ViewModels.Account
{
    public class RegistrationFirstStepViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "The password is too short")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public bool IsCompanyChecked { get; set; }

        public RoleEnum Role { get; set; }
    }
}