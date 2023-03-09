using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Qizilim.az.Models.Entities.Membreship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Qizilim.az.Models.DataContext
{
    public static class QizilimDbSeed
    {
        static internal IApplicationBuilder InitMembership(this IApplicationBuilder app)
        {

            using (IServiceScope scope = app.ApplicationServices.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetService<UserManager<QizilimUser>>();
                var roleManager = scope.ServiceProvider.GetService<RoleManager<QizilimRole>>();
                var configuration = scope.ServiceProvider.GetService<IConfiguration>();


                var user = userManager.FindByEmailAsync(configuration["membership:email"]).Result;

                if (user == null)
                {
                    user = new QizilimUser
                    {
                        Name = configuration["membership:name"],
                        Surname = configuration["membership:surname"],
                        Email = configuration["membership:email"],
                        UserName = configuration["membership:username"],
                        EmailConfirmed = true
                    };


                    var identityResult = userManager.CreateAsync(user, configuration["membership:password"]).Result;

                    if (!identityResult.Succeeded)
                        return app;

                }



                var roLes = configuration["membership:roles"].Split(",", StringSplitOptions.RemoveEmptyEntries);

                foreach (var roleName in roLes)
                {
                    var role = roleManager.FindByNameAsync(roleName).Result;

                    if (role == null)
                    {
                        role = new QizilimRole
                        {
                            Name = roleName
                        };
                        var roleResult = roleManager.CreateAsync(role).Result;


                        if (roleResult.Succeeded)
                        {
                            userManager.AddToRoleAsync(user, roleName).Wait();
                        }
                    }
                    else if (!userManager.IsInRoleAsync(user, roleName).Result)
                    {
                        userManager.AddToRoleAsync(user, roleName).Wait();
                    }
                }
            }

            return app;
        }
    }
}
