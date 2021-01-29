using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjetCESI.Core;
using ProjetCESI.Data.Context;
using ProjetCESI.Metier;
using ProjetCESI.Web.Outils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using ProjetCESI.Metier.Outils;
using Newtonsoft.Json.Serialization;

namespace ProjetCESI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MainContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Cn1")));
            services.AddIdentity<User, ApplicationRole>(options => options.SignIn.RequireConfirmedEmail = true)
                .AddEntityFrameworkStores<MainContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.SignIn.RequireConfirmedEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            });

            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.FromMinutes(30);
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
                options.LoginPath = "/Account/login";
                options.LogoutPath = "/Account/logOff";
                options.SlidingExpiration = true;
                
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential 
                // cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                // requires using Microsoft.AspNetCore.Http;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<PasswordHasherOptions>(options => options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2);

            services.AddAuthorization(config =>
            {
                config.AddPolicy("AdminUser", policyConfig =>
                {
                    policyConfig.RequireRole("admin");
                });
                config.AddPolicy("StandardUser", policyConfig =>
                {
                    policyConfig.RequireRole("client");
                });
            });

            services.AddViewToStringRendererService();

            services.AddControllersWithViews().AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
                opt.SerializerSettings.MaxDepth = 5;
                opt.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
                opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            })
                .AddRazorRuntimeCompilation();

            Outils.SetConfig(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "api",
                    areaName: "api",
                    pattern: "api/{controller=Ressource}/{action=Ressource}/{id?}");

                endpoints.MapControllerRoute(
                    name: "Statistiques",
                    pattern: "Statistiques",
                    defaults: new { area = "", controller = "Statistiques", action = "Statistiques" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Accueil}/{action=Accueil}/{id?}");
            });

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetService<RoleManager<ApplicationRole>>();

                Helper.Seed(userManager, roleManager).GetAwaiter().GetResult();
            }
        }
    }
}
