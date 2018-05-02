using System;
using System.Collections.Generic;
using IdentityServer4;
using Microsoft.AspNetCore.Identity;

namespace Tsl.IdentityServer4.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string ContactName { get; set; }
    }
}
