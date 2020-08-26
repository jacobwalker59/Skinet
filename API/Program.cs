using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            //executes a process 
            var host = CreateHostBuilder(args).Build();
           //essentially you need to get the services, which is part of the host
           //these are available in the startup class too
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    //serices provider gets access to all things dependency injection
                    var context = services.GetRequiredService<StoreContext>();
                    await context.Database.MigrateAsync();
                    await StoreContextSeed.SeedAsync(context, loggerFactory);

                    var userManager = services.GetRequiredService<UserManager<AppUser>>();
                    // need reference to identity db context
                    var identityContext = services.GetRequiredService<AppIdentityDbContext>();
                    await identityContext.Database.MigrateAsync();
                    // applies all pending migrations to the database
                    await AppIdentityDbContextSeed.SeedUsersAsync(userManager);
                    


                }
                catch(Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "An error occured during migration");
                }
            }



            host.Run();
        }
        

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
