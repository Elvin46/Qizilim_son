using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Qizilim.az.Models.DataContext;
using Qizilim.az.Models.Entities.Membreship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qizilim.az
{
    public class Startup
    {
        readonly IConfiguration configuration;
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(cfg =>
            {

                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();


                cfg.Filters.Add(new AuthorizeFilter(policy));

            });
            services.AddControllersWithViews();

            services.AddRouting(cfg => cfg.LowercaseUrls = true);

            services.AddDbContext<QizilimDbContext>(cfg =>
            {
                
                cfg.UseSqlServer(configuration.GetConnectionString("cString"),
                    options => options.EnableRetryOnFailure(
                        maxRetryCount: 10,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null
                        ));
            });

            services.AddIdentity<QizilimUser, QizilimRole>()
               .AddEntityFrameworkStores<QizilimDbContext>();

            services.AddScoped<UserManager<QizilimUser>>();
            services.AddScoped<SignInManager<QizilimUser>>();
            services.AddScoped<RoleManager<QizilimRole>>();

            services.Configure<IdentityOptions>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.Password.RequireUppercase = false;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequiredLength = 3;
                cfg.Password.RequireDigit = false;
                cfg.Password.RequiredUniqueChars = 1;
                cfg.Lockout.MaxFailedAccessAttempts = 3;
                cfg.Lockout.DefaultLockoutTimeSpan = new TimeSpan(0, 1, 0);
            });


            services.AddMediatR(this.GetType().Assembly);
            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
            services.ConfigureApplicationCookie(cfg =>
            {
                cfg.LoginPath = "/signin.html";
                cfg.AccessDeniedPath = "/accessdenied.html";
                cfg.LogoutPath = "/signout.html";



                cfg.Cookie.Name = "diplomapp";
                cfg.Cookie.HttpOnly = true;
                cfg.ExpireTimeSpan = new TimeSpan(0, 5, 0);
            });
            services.AddAuthentication();
            services.AddAuthorization(cfg =>
            {

                foreach (var item in Program.principals)
                {
                    cfg.AddPolicy(item, p =>
                    {
                        p.RequireAssertion(handler =>
                        {
                            return handler.User.IsInRole("Superadmin") || handler.User.HasClaim(item, "1");
                        });

                    });
                }



            });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();
            app.UseRouting();
            

            app.InitMembership();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(cfg =>
            {
                cfg.MapAreaControllerRoute(
                    name: "defaultAdmin",
                    areaName: "Admin",
                    pattern: "Admin/{controller=dashboard}/{action=index}/{id?}"
                    );


                cfg.MapControllerRoute("default", pattern: "{controller=homepage}/{action=index}/{id?}");
            });
        }
    }
}
