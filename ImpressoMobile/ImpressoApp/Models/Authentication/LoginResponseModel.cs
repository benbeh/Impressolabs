using System;
using Newtonsoft.Json;

namespace ImpressoApp.Models.Authentication
{
    public class LoginResponseModel : BaseResponseModel
    {
        [JsonProperty(PropertyName = "jwt")]
        public JwtResponseModel Jwt { get; set; }
    }
}
