using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjetCESI.Core;
using ProjetCESI.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Data
{
    public class Repository<T> where T : class
    {
        IConfiguration _configuration;
        public IConfiguration Configuration { get => _configuration; }

        public void ConfigureServices(IServiceCollection __services, IConfiguration __configuration)
        {
            _configuration = __configuration;

            __services.AddDbContext<MainContext>(options => options.UseSqlServer(__configuration.GetConnectionString("Main")));
            __services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedEmail = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<MainContext>()
                .AddDefaultTokenProviders()
                .AddUserManager<UserManager<User>>()
                .AddSignInManager<SignInManager<User>>();

            __services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            });

            __services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.FromMinutes(30);
            });

            __services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.LoginPath = "/Account/login";
                options.LogoutPath = "/Account/logOff";
                options.SlidingExpiration = true;
            });

            __services.Configure<PasswordHasherOptions>(options => options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2);
        }
    }
}
