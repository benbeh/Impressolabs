using System;
using System.Collections.Generic;
using System.Text;
using Core.Enum;
using Microsoft.AspNetCore.Http;

namespace BLL.ViewModels.API
{
    public class CreateEventViewModel
    {
        public string PictureSource { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public PrivacyTypeEnum PrivacyType { get; set; }
        public IFormFile PhotoFile { get; set; }
    }
}
