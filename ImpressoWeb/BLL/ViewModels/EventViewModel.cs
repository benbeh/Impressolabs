using Core.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace BLL.ViewModels
{
    public class EventViewModel
    {
        public int Id { get; set; }
        public string PictureSource { get; set; }
        public bool IsBookmarked { get; set; }
        public bool IsInterested => IsBookmarked;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public PrivacyTypeEnum PrivacyType { get; set; }
        public int HostedById { get; set; }
        public string HostedByName { get; set; }
        public IFormFile PhotoFile { get; set; }
    }
}
