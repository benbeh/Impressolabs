using System;
using Newtonsoft.Json;

namespace ImpressoApp.Models.User
{
    public class SetAsBookmarkedRequestModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("isBookmarked")]
        public bool IsBookmarked { get; set; }
    }
}
