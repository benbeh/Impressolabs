using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Web.ApiAuthentication.Auth
{
    public interface IJwtFactory
    {
        Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity, IList<string> role);
        ClaimsIdentity GenerateClaimsIdentity(string userName, string id);
    }
}
