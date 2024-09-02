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
            catch (ValidationException ex)
            {
                var result = Results.BadRequest(ex.Message);
                await result.ExecuteAsync(context);
            }
            catch (Exception)
            {
                var result = Results.Problem("An unexpected error occurred.", statusCode: StatusCodes.Status500InternalServerError);
                await result.ExecuteAsync(context);
            }
        }
    }


}
