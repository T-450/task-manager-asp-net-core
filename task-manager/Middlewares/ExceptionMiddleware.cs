using System.Net;

namespace task_manager.Middlewares
{
    internal record ErrorDetails(int StatusCode, string Message) { }

    /// <summary>
    ///     Global Error Handler Middleware
    ///     All exceptions thrown in the controllers ends up here
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        private ILogger<ExceptionMiddleware> _logger { get; }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError("Something went wrong: {ExMessage}", ex.Message);
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

            ErrorDetails errorDetails = new(context.Response.StatusCode, "Internal Server Error.");
            await context.Response.WriteAsync(errorDetails.ToString()).ConfigureAwait(false);
        }
    }
}
