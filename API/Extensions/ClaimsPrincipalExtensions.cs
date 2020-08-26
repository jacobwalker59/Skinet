using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string RetrieveEmailFromPrincipal(this ClaimsPrincipal user)
        {
            return user?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?
            .Value;
        }
    }
}