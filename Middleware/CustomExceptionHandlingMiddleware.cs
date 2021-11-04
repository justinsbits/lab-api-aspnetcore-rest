using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
//using Serilog;

namespace CommanderREST.Middleware
{
    public class CustomExceptionHandlingMiddleware 
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public CustomExceptionHandlingMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
 
        // middelware method
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleGlobalExceptionAsync(context, ex);
            }
        }
 
        private Task HandleGlobalExceptionAsync(HttpContext context, Exception exception)
        {
            if (exception is ApplicationException)
            {
                _logger.LogWarning("A validation error occurred. {message}", exception.Message);
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return context.Response.WriteAsJsonAsync(new { exception.Message });
            }
            else
            {
                var errorId = Guid.NewGuid();
                _logger.LogError(exception, "An error occurred: {ErrorId}", errorId);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return context.Response.WriteAsJsonAsync(new
                {
                    ErrorId = errorId,
                    Message = "Something bad happened in our API. " +
                              "Contact our support team with the ErrorId if the issue persists."
                });
            }
        }
    }
}