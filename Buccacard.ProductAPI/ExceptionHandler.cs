using Buccacard.Infrastructure.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Buccacard.ProductAPI
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Net;
    using System.Text.Json;
    using System.Threading.Tasks;
    public class CustomAppException : Exception
    {
        public CustomAppException() { }

        public CustomAppException(string message) : base(message) { }

        public CustomAppException(string message, Exception innerException) : base(message, innerException) { }

        // Add custom properties and methods as needed
    }

    public class ProductCustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ProductCustomExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (CustomAppException customException)
            {
                await HandleCustomExceptionAsync(context, customException);
            }
            catch (Exception ex)
            {
                await HandleGlobalExceptionAsync(context, ex);
            }
        }

        private async Task HandleCustomExceptionAsync(HttpContext context, CustomAppException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync(new ResponseService().ErrorResponse<string>($"An error occured. {exception.Message}").ToJson());
        }

        private async Task HandleGlobalExceptionAsync(HttpContext context, Exception exception)
        {
            // Log the exception (you can use a logging library like Serilog, NLog, etc.)

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var result = JsonSerializer.Serialize(new { error = "An unexpected error occurred." });
            await context.Response.WriteAsync(new ResponseService().ErrorResponse<string>($"An unexpected error occurred.{exception.Message}").ToJson());
        }
    }

}
