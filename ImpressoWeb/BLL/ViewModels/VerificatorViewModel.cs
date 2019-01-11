using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Core.Entities;

namespace BLL.ViewModels
{
    public class VerificatorViewModel
    {
        public int TestimonialId { get; set; }
        public string AppUserId { get; set; }
        public string AppUserPhoto { get; set; }
    }
}
