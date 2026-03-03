using System.Net;
using System.Text.Json;

namespace IdentityService.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                await WriteErrorResponse(context, ex);
            }
        }

        private static Task WriteErrorResponse(HttpContext context, Exception ex)
        {
            var (statusCode, title) = ex switch
            {
                InvalidOperationException => (HttpStatusCode.Conflict, "Conflict"),
                UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized"),
                ArgumentException => (HttpStatusCode.BadRequest, "Bad Request"),
                _ => (HttpStatusCode.InternalServerError, "Server Error")
            };

            var problem = new
            {
                title,
                status = (int)statusCode,
                detail = ex.Message
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
    }
}
