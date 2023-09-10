using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Buccacard.Infrastructure.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Middleware
{
    public class ProductExceptionMiddleware 
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ProductExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        public ProductExceptionMiddleware(RequestDelegate next, ILogger<ProductExceptionMiddleware> logger, 
            IHostEnvironment env)
        {
            _env = env;
            _logger = logger;
            _next = next;
        }
         
        public async Task InvokeAsync(HttpContext context)
        {
            try 
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment()
                    ? new  ResponseService().ErrorResponse<string>(ex.StackTrace?.ToString())
                    : new ResponseService().ErrorResponse<string>("Server Error");

                var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}