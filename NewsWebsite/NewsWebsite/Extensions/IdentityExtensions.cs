using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace NewsWebsite.Extensions
{
    public static class IdentityExtensions
    {
        public static int GetPermission(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Permission");
            // Test for null to avoid issues during local testing
            return (claim != null) ? Convert.ToInt32(claim.Value) : 0;
        }
    }
}