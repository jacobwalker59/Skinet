using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next,
        //request delegate process http request, if there is no exception, 
        //we move onto the next piece of middleware
        ILogger<ExceptionMiddleware> logger,
        IHostEnvironment env)
        {
            _env = env;
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try{
                await _next(context);
            }
            catch(Exception ex)
            {
                //obviously in the event of an exception
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment()? 
                new ApiException((int)HttpStatusCode.InternalServerError, ex.Message,ex.StackTrace.ToString()):
                new ApiException((int)HttpStatusCode.InternalServerError);
                //remember this inherits from the response so it still gets passed in regardless

                var options = new JsonSerializerOptions{PropertyNamingPolicy = 
                JsonNamingPolicy.CamelCase};

                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);

                //http context. write async??
                //the response is the current http response and that response is in json
                //so we're literally writing the errors to json

            }
        }
    }
}