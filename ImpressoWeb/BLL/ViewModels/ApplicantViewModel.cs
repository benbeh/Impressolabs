using System;
using System.Collections.Generic;
using Core.Entities;

namespace BLL.ViewModels
{
    public class ApplicantViewModel
    {
        public string Id { get; set; }
        public string Photo { get; set; }
        public string UserName { get; set; }
        public string Profession { get; set; }
        public string JobName { get; set; }
        public string ProjectName { get; set; }
        public DateTime DateOfPost { get; set; }
        public AppUser AppUser { get; set; }
    }
}