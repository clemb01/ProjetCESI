using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjetCESI.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Outils
{
    public static class IHostExtensions
    {
        public static IHost MigrateDatabase(this IHost webHost)
        {
            // Manually run any outstanding migrations if configured to do so
            var envAutoMigrate = "true";
            if (envAutoMigrate != null && envAutoMigrate == "true")
            {
                var serviceScopeFactory = (IServiceScopeFactory)webHost.Services.GetService(typeof(IServiceScopeFactory));

                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var dbContext = services.GetRequiredService<MainContext>();

                    dbContext.Database.Migrate();
                }
            }

            return webHost;
        }
    }
}
