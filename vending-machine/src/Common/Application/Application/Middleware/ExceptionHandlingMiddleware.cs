using Application.DTO;
using Application.Exceptions;
using Application.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Application.Middleware
{
    /// <summary>
    /// Custom middleware to handle the exceptions.
    /// Approach to handling the exception global level. We could be caught all unhandled exceptions using this exception handler. 
    /// The benefit of using this approach is we could be caught all exceptions in one place and don’t need to use try-catch in every action
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;
            string result = JsonSerializer.Serialize(new ErrorResponse(exception.Message));

            switch (exception)
            {
                case ValidationException ex:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(ex.ToErrorResponse());
                    break;
                case NotFoundException ex:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            _logger.LogError(exception, exception.Message);
            await context.Response.WriteAsync(result);
        }
    }
}
