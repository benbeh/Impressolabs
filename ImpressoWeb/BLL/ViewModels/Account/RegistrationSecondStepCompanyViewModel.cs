using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BLL.ViewModels.Account
{
    public class RegistrationSecondStepCompanyViewModel
    {
        [Required]
        public RegistrationFirstStepViewModel FirstStepModel { get; set; }

        [Required]
        public string CompanyName { get; set; }

        public IFormFile Image { get; set; }
    }
}