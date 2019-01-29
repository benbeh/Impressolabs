using System;
using Newtonsoft.Json;

namespace ImpressoApp.Models.Authentication
{
    public class JwtResponseModel
    {
        public string ID { get; set; }

        [JsonProperty(PropertyName = "auth_token")]
        public string AuthToken { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty(PropertyName = "user_name")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "issued")]
        public string Issued { get; set; }
    }
}
