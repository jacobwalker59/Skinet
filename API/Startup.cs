using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Errors;
using API.Extensions;
using API.Helpers;
using API.Middleware;
using AutoMapper;
using Core.Interfaces;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //responsible for all dependency injection
            services.AddControllers();
            //need to specify the location of AutoMapper and the startup
            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddDbContext<StoreContext>(x => x.UseSqlite(_configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<AppIdentityDbContext>(x => 
            {
                x.UseSqlite(_configuration.GetConnectionString("IdentityConnection"));
            });
            
            services.AddSingleton<IConnectionMultiplexer>(
                c => {
                    var configuration = ConfigurationOptions.Parse(_configuration.GetConnectionString("Redis"),
                    true);
                    return ConnectionMultiplexer.Connect(configuration);
                }
            );

            services.AddApplicationServices();
          //  services.AddSwaggerDocumentation();
            services.AddIdentityServices(_configuration);
            services.AddCors(opt => 
            {
                opt.AddPolicy("CorsPolicy", policy => {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                    //if its running on insecure port, browser wont display applicaiton.
                    //we do this in angular too
                });
            });
            //because its a static class we can just leave it here
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {  
            //middle ware for applicaiton all goes in here
             /*
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            */

            app.UseMiddleware<ExceptionMiddleware>();
            //using our own middle ware

            //if we dont have an end point which matches the above request, we bounce
            //down to the below and are redirected here with the status code pages

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            
            app.UseHttpsRedirection();
            //if request comes into server on server https//5001 then it gets redirected here
            app.UseRouting();
            //gets directed to the router
            app.UseStaticFiles();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

           // app.UseSwaggerDocumentation();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //maps all endpoints in the controllers so it is known where to send request
            });
        }
    }
}
