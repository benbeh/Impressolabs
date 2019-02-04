using System;
using System.Linq;
using Core.Enum;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web.Infrastructure
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(bool isBearerScheme = false, params RoleEnum[] roles)
        {
            if (isBearerScheme)
            {
                AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
            }

            if (roles != null && roles.Length != 0)
            {
                Roles = string.Join(",", roles.Select(r => Enum.GetName(r.GetType(), r)));
            }
        }
    }
}