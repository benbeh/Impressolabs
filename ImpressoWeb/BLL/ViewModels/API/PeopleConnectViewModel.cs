using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.ViewModels.API
{
    public class PeopleConnectViewModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string PictureSource { get; set; }
        public bool IsBookmarked { get; set; }
        public string Position { get; set; }
        public string CompanyPosition { get; set; }
        public string CityAddress { get; set; }
        public string Description { get; set; }
        public string YearsOfExperiense { get; set; }
        public bool IsConnected { get; set; }
    }
}
