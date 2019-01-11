using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.ApiAuthentication.Auth;
using Web.ApiAuthentication.Models;

namespace Web.ApiAuthentication.Helpers
{
    public class Tokens
    {
        public static async Task<string> GenerateJwt(ClaimsIdentity identity, IJwtFactory jwtFactory, string userName, string firstName, string lastName, IList<string> roles, JwtIssuerOptions jwtOptions, JsonSerializerSettings serializerSettings)
        {
            var response = new
            {
                id = identity.Claims.Single(c => c.Type == "id").Value,
                auth_token = await jwtFactory.GenerateEncodedToken(userName, identity, roles),
                expires_in = (int)jwtOptions.ValidFor.TotalSeconds,
                user_name = userName,
                issued = DateTime.Now,
                full_name = firstName + " " + lastName
            };
            return JsonConvert.SerializeObject(response, serializerSettings);
        }
    }
}
