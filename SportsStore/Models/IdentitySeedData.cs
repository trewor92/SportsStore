using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;


namespace SportsStore.Models
{
    public static class IdentitySeedData
    {
        private const string adminUser = "Admin";
        private const string adminPassword = "Secret123$";

        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            var serviceScope = app.ApplicationServices.CreateScope();

            UserManager<IdentityUser> userManager = serviceScope.ServiceProvider
                .GetService<UserManager<IdentityUser>>();

            
            IdentityUser user = await userManager.FindByIdAsync(adminUser);

            if (user == null)
            {
                user = new IdentityUser(adminUser);
                await userManager.CreateAsync(user, adminPassword);
            }
            
        }
    }
}

