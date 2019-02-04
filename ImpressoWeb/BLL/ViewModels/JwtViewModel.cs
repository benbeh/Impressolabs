using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BLL.ViewModels
{
    public class JwtViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("auth_token")]
        public string AuthToken { get; set; }
        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }
        [JsonProperty("user_name")]
        public string UserName { get; set; }
        [JsonProperty("issued")]
        public string Issued { get; set; }
        [JsonProperty("full_name")]
        public string FullName { get; set; }
    }
}
