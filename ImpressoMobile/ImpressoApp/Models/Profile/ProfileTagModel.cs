using System;
using ImpressoApp.Enums;
namespace ImpressoApp.Models.Profile
{
    public class ProfileTagModel
    {
        public string ID { get; set; }
        public TagType TagType { get; set; }
        public string Title { get; set; }
        public string ImageSource { get; set; }
        public string Role { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
