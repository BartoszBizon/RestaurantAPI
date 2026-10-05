using Microsoft.AspNetCore.Diagnostics;
using RestaurantAPI.Exceptions;

namespace RestaurantAPI.Presentation.ExceptionHandlers
{
    public class NotFoundExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not NotFoundException)
                return false;

            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsync(exception.Message, cancellationToken);
            return true;
        }
    }
}
