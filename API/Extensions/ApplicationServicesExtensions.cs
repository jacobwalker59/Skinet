using System.Linq;
using API.Errors;
using Core.Interfaces;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            //IServices collection is basically just where the dependency injection occurs.
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped(typeof(IGenericRepository<>),(typeof(GenericRepository<>)));
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddScoped<IPaymentService, PaymentService>();
             services.Configure<ApiBehaviorOptions>(options =>
             {
                 //access to api controller
                options.InvalidModelStateResponseFactory = actionContext => 

                {
                    var errors = actionContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToArray();

                    var errorResponse = new ApiValidationErrorResponse 
                    {
                        Errors = errors
                    };
                    //so here the errors is combined with the other object
                    //into one bad request object down below, 
                    //sounds obvious but youve got to think of the ramifications
                    //about why it works
                    return new BadRequestObjectResult(errorResponse);
                };
             });
             return services;
        }
    }
}