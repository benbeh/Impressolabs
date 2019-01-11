using System.ComponentModel.DataAnnotations;

namespace BLL.ViewModels.Account
{
    public class RegistrationSecondStepUserViewModel
    {
        [Required]
        public RegistrationFirstStepViewModel FirstStepModel { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Photo { get; set; }
    }
}