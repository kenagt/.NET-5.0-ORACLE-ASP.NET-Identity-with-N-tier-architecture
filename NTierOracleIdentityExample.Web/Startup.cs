using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NTierOracleIdentityExample.Bll.Services.Abstract;
using NTierOracleIdentityExample.Bll.Services.Implementation;
using NTierOracleIdentityExample.Dll.Context;
using NTierOracleIdentityExample.Dll.Entities;
using NTierOracleIdentityExample.Dll.Repositories.Abstract;
using NTierOracleIdentityExample.Dll.Repositories.Implementation;
using NTierOracleIdentityExample.Web.AutoMapperConfigurations;
using NTierOracleIdentityExample.Web.Extensions;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NTierOracleIdentityExmple.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //adding custom roles
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "BasicRole", "Administrator" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                //creating the roles and seeding them to the database
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            //AutoMapper setup
            services.AddAutoMapper(typeof(Startup));

            //Custom claims transformer
            services.AddTransient<IClaimsTransformation, ClaimsTransformer>();

            //DI
            services.AddTransient<EXAMPLE_SCHEMA_Context>();

            //DI Repositories
            services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddTransient(typeof(ILogRepository), typeof(LogRepository));
            services.AddTransient(typeof(IExampleTableRepository), typeof(ExampleTableRepository));

            //DI Services
            services.AddTransient<IUserAdministrationService, UserAdministrationService>();
            services.AddTransient<IRoleAdministrationService, RoleAdministrationService>();
            services.AddTransient<IUserRoleAdministrationService, UserRoleAdministrationService>();
            services.AddTransient<ILogService, LogService>();

            //Add Identity
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.AllowedUserNameCharacters = null;
            })
             .AddEntityFrameworkStores<EXAMPLE_SCHEMA_Context>()
             .AddDefaultTokenProviders();

            //To call claims transformer for .NET CORE >=3.0 
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IISDefaults.AuthenticationScheme;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdministrator", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser()
                        .RequireAssertion(context => context.User.HasClaim(ClaimTypes.Role, "Administrator"))
                        .Build();
                });

                options.AddPolicy("RequiredBasic", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser()
                        .RequireAssertion(context => context.User.HasClaim(ClaimTypes.Role, "BasicRole"))
                        .Build();
                });
            });

            //AutoMapper: create the mapper
            services.AddSingleton(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new EXAMPLE_SCHEMA_Profile());
            }).CreateMapper());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            CreateRoles(serviceProvider).Wait();
        }
    }
}
