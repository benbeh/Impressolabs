using System.ComponentModel.DataAnnotations;

namespace BLL.ViewModels.Account
{
    public class ModificateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }

        public string RoleId { get; set; }

        public string[] IdsToAdd { get; set; }

        public string[] IdsToDelete { get; set; }
    }
}