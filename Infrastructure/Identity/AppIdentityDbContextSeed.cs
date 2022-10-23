using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser>userManager)
        {
            if(!userManager.Users.Any())
            {
                var user = new AppUser{
                    DisplayName="Efe",
                    Email="efe@test.com",
                    UserName="efe@test.com",
                    Adress= new Address{
                        FirstName="Efe",
                        LastName="Sarı",
                        Street="4.Sokak",
                        City="Bursa",
                        ZipCode="16"
                    }
                };
                await userManager.CreateAsync(user,"Qazwsx!23"); 
            } 
        }
    }
}