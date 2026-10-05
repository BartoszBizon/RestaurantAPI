using Microsoft.AspNetCore.Diagnostics;
using RestaurantAPI.Exceptions;

namespace RestaurantAPI.Presentation.ExceptionHandlers
{
    public class BadRequestExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not BadRequestException)
                return false;

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync(exception.Message, cancellationToken);
            return true;
        }
    }
}
