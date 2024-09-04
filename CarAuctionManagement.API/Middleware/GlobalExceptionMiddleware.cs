using CarAuctionManagement.Application.Exceptions;

namespace CarAuctionManagement.API.Middleware
{
    public class GlobalExceptionMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            int statusCode;
            string message;

            switch (exception)
            {
                case ArgumentException:
                    statusCode = StatusCodes.Status400BadRequest;
                    message = exception.Message;
                    break;
                case ValidationException:
                    statusCode = StatusCodes.Status400BadRequest;
                    message = exception.Message;
                    break;
                case EntityNotFoundException:
                    statusCode = StatusCodes.Status404NotFound;
                    message = exception.Message;
                    break;
                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    message = "An unexpected error occurred.";
                    break;
            }

            var result = Results.Problem(message, statusCode: statusCode);
            return result.ExecuteAsync(context);
        }
    }


}
