using System;
using PropertyChanged;
namespace ImpressoApp.Models.Feeds
{
    [AddINotifyPropertyChangedInterface]
    public class ConnectPeopleModel : IConnectFeedModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string PictureSource { get; set; }
        public string Position { get; set; }
        public string CompanyPosition { get; set; }
        public string CityAddress { get; set; }
        public string Description { get; set; }
        public string YearsOfExperiense { get; set; }
        public string IsConnected { get; set; }
        public bool IsBookmarked { get; set; }
    }
}
