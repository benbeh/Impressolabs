using System.Collections.Generic;
using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace BLL.ViewModels.Account
{
    public class EditRoleViewModel
    {
        public IdentityRole Role { get; set; }

        public IEnumerable<AppUser> Members { get; set; }

        public IEnumerable<AppUser> NonMembers { get; set; }
    }
}