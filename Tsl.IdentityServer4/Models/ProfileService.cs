using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;

namespace Tsl.IdentityServer4.Models
{
    public class ProfileService : IProfileService
    {
        protected UserManager<ApplicationUser> _userManager;

        public ProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            //>Processing
            var user = _userManager.GetUserAsync(context.Subject).Result;

            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, user.ContactName));
            if (user.EmailConfirmed)
            {
                claims.Add(new Claim(ClaimTypes.Email, user.Email));
            }


            context.IssuedClaims.AddRange(claims);

            //>Return
            return Task.FromResult(0);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            //>Processing
            var user = _userManager.GetUserAsync(context.Subject).Result;

            //context.IsActive = (user != null) && user.IsActive;

            //>Return
            return Task.FromResult(0);
        }
    }
}