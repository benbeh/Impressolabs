using System;
using System.ComponentModel;
using PropertyChanged;
using ImpressoApp.Enums;
using Newtonsoft.Json;

namespace ImpressoApp.Models.Feeds
{
    [AddINotifyPropertyChangedInterface]
    public class ConnectEventModel : IConnectFeedModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("pictureSource")]
        public string PictureSource { get; set; }

        [JsonProperty("isBookmarked")]
        public bool IsBookmarked { get; set; }

        [JsonProperty("startDate")]
        [AlsoNotifyFor("Dates")]
        public DateTimeOffset StartDate { get; set; }

        [JsonProperty("endDate")]
        [AlsoNotifyFor("Dates")]
        public DateTimeOffset EndDate { get; set; }

        [JsonProperty("country")]
        [AlsoNotifyFor("Location")]
        public string Country { get; set; }

        [JsonProperty("city")]
        [AlsoNotifyFor("Location")]
        public string City { get; set; }

        [JsonProperty("address")]
        [AlsoNotifyFor("Location")]
        public string Address { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("privacyType")]
        public PrivacyType PrivacyType { get; set; }

        [JsonProperty("hostedById")]
        public long HostedById { get; set; }

        [JsonProperty("hostedByName")]
        public string HostedByName { get; set; }

        [JsonProperty("isInterested")]
        public bool IsInterested { get; set; }


        public DateTime DateNow { get; set; } = DateTime.Now;

        public string Dates { get => GetDates(); }

        public string Location { get => GetLocation(); }

        private string GetDates()
        {
            return string.Format("{0} - {1}", StartDate.ToString("MMM dd"), EndDate.ToString("MMM dd"));
        }

        private string GetLocation()
        {
            return string.Format("{0}, {1}, {2}", Address, City, Country);
        }
    }
}
